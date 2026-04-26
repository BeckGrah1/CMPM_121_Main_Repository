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
using TMPro;

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
        List<Coroutine> spawnCoroutines = new List<Coroutine>();
        foreach (SpawnEntry spawnEntry in levelData.spawns)
        {
            Coroutine coroutine = StartCoroutine(SpawnEnemyType(spawnEntry, waveNum));
            spawnCoroutines.Add(coroutine);
        }

        foreach (Coroutine coroutine in spawnCoroutines)
        {
            yield return coroutine;
        }

        yield return new WaitWhile(() => GameManager.Instance.enemy_count > 0);
        waveNum++;
        GameManager.Instance.state = GameManager.GameState.WAVEEND;
    }
    IEnumerator SpawnEnemyType(SpawnEntry spawnEntry, int waveNum)
    {
        EnemyData enemyData = EnemyDataManager.Instance.GetEnemyByName(spawnEntry.enemy);
        RPNReturns rpnResults = CalculateSpawnExpressions.Calculate(spawnEntry, enemyData, waveNum);
        float waitTime = 2;
        Debug.Log($"=== Spawning {spawnEntry.enemy} ===");
        Debug.Log($"Count from RPN: {rpnResults.count}");
        Debug.Log($"HP: {rpnResults.hp}, Speed: {rpnResults.speed}, Damage: {rpnResults.damage}");
        Debug.Log($"Wave number: {waveNum}");
        Debug.Log($"Raw count string: {spawnEntry.count}");
        if (float.TryParse(spawnEntry.delay, out float result))
        {
            waitTime = result;
        }
        for (int i = 0; i < rpnResults.count; i++)
        {
            SpawnPoint spawnPoint = GetRandomSpawnPoint(spawnEntry.location);
            SpawnEnemy(enemyData, rpnResults, spawnPoint);
            yield return new WaitForSeconds(waitTime);
        }

    }

    void SpawnEnemy(EnemyData enemyData, RPNReturns rpnResults, SpawnPoint spawnPoint)
    {
        Vector2 offset = Random.insideUnitCircle * 1.8f;
                
        Vector3 initial_position = spawnPoint.transform.position + new Vector3(offset.x, offset.y, 0);
        GameObject new_enemy = Instantiate(enemy, initial_position, Quaternion.identity);

        // changed to take sprite data from enemy JSON
        new_enemy.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.enemySpriteManager.Get(enemyData.sprite);
        EnemyController en = new_enemy.GetComponent<EnemyController>();
        // changed to get speed and hp from enemy data. Will NOT work for RPN strings without further work
        en.hp = new Hittable(rpnResults.hp, Hittable.Team.MONSTERS, new_enemy);
        en.speed = rpnResults.speed;
        en.damage = rpnResults.damage;
        GameManager.Instance.AddEnemy(new_enemy);
    }

    // currently only takes random spawns, however could easily be modified to take specific spawn points if needed
    SpawnPoint GetRandomSpawnPoint(string locationFilter)
    {
        if (locationFilter == "random")
        {
            return SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        }
        if (locationFilter == "random red")
        {
            SpawnPoint[] redSpawnPoints = SpawnPoints.Where(sp => sp.kind == SpawnPoint.SpawnName.RED).ToArray();
            return redSpawnPoints[Random.Range(0, redSpawnPoints.Length)];
        }
        if (locationFilter == "random bone")
        {
            SpawnPoint[] boneSpawnPoints = SpawnPoints.Where(sp => sp.kind == SpawnPoint.SpawnName.BONE).ToArray();
            return boneSpawnPoints[Random.Range(0, boneSpawnPoints.Length)];
        }
        if (locationFilter == "random green")
        {
            SpawnPoint[] greenSpawnPoints = SpawnPoints.Where(sp => sp.kind == SpawnPoint.SpawnName.GREEN).ToArray();
            return greenSpawnPoints[Random.Range(0, greenSpawnPoints.Length)];
        }
        Debug.Log("spawn point filter " + locationFilter + " not found");
        return null;
    }
}
