using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

using RPNEvaluator;
using System.Data;

public enum Difficulty
{
    Easy,
    Medium,
    Endless
}

public class EnemySpawner : MonoBehaviour
{
    public Image level_selector;
    public GameObject button;
    public GameObject enemy;
    public SpawnPoint[] SpawnPoints;
    public Difficulty difficulty;
    public LevelData levelData;
    public int waveNum = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject selector = Instantiate(button, level_selector.transform);
        selector.transform.localPosition = new Vector3(0, 130);
        selector.GetComponent<MenuSelectorController>().spawner = this;
        selector.GetComponent<MenuSelectorController>().SetLevel("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel(string levelname)
    {
        level_selector.gameObject.SetActive(false);
        // this is not nice: we should not have to be required to tell the player directly that the level is starting
        GameManager.Instance.player.GetComponent<PlayerController>().StartLevel();
        StartCoroutine(SpawnWave());
        // difficulty should be set by player when selecting level, for now I'll just set it to easy
        difficulty = Difficulty.Easy;
        levelData = LevelDataManager.Instance.GetDifficultyByName(difficulty.ToString());
    }

    public void NextWave()
    {
        StartCoroutine(SpawnWave());
    }


    IEnumerator SpawnWave()
    {
        GameManager.Instance.state = GameManager.GameState.COUNTDOWN;
        GameManager.Instance.countdown = 3;
        for (int i = 3; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            GameManager.Instance.countdown--;
        }
        GameManager.Instance.state = GameManager.GameState.INWAVE;
        // new stuff here
        foreach (SpawnEntry spawnEntry in levelData.spawns)
        {
            StartCoroutine(SpawnEnemyType(spawnEntry, waveNum));
        }
        

        yield return new WaitWhile(() => GameManager.Instance.enemy_count > 0);
        GameManager.Instance.state = GameManager.GameState.WAVEEND;
        waveNum++;
    }
    IEnumerator SpawnEnemyType(SpawnEntry spawnEntry, int waveNum)
    {
        EnemyData enemyData = EnemyDataManager.Instance.GetEnemyByName(spawnEntry.enemy);
        RPNReturns rpnResults = CalculateSpawnExpressions.Calculate(spawnEntry, enemyData, waveNum);
        

    }

    IEnumerator SpawnEnemy(string enemyName, RPNReturns rpnResults)
    {
        SpawnPoint spawn_point = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        Vector2 offset = Random.insideUnitCircle * 1.8f;
                
        Vector3 initial_position = spawn_point.transform.position + new Vector3(offset.x, offset.y, 0);
        GameObject new_enemy = Instantiate(enemy, initial_position, Quaternion.identity);

        EnemyData enemyData = EnemyDataManager.Instance.GetEnemyByName(enemyName);
        // changed to take sprite data from enemy JSON
        new_enemy.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.enemySpriteManager.Get(enemyData.sprite);
        EnemyController en = new_enemy.GetComponent<EnemyController>();
        // changed to get speed and hp from enemy data. Will NOT work for RPN strings without further work
        en.hp = new Hittable(rpnResults.hp, Hittable.Team.MONSTERS, new_enemy);
        en.speed = rpnResults.speed;
        en.damage = rpnResults.damage;
        GameManager.Instance.AddEnemy(new_enemy);
        yield return new WaitForSeconds(0.5f);
    }
}
