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
    LevelData[] levels;

    // dummy fields for class diagram
    private GameManager _gameManager = GameManager.Instance;
    private EnemyDataManager _enemyDataManager = EnemyDataManager.Instance;
    private LevelDataManager _levelDataManager = LevelDataManager.Instance;
    private CalculateSpawnExpressions _calculateSpawnExpressions;

    void OnEnable()
    {
        GameManager.OnNextWave += NextWave;
        GameManager.OnGameReset += ResetSpawner;
        GameManager.OnGAMEOVER += StopWave;
    }

    void OnDisable()
    {
        GameManager.OnNextWave -= NextWave;
        GameManager.OnGameReset -= ResetSpawner;
        GameManager.OnGAMEOVER -= StopWave;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // LevelDataManager.Awake() will ensure Instance exists before this Start() runs- all Awake() must run before any Start()
        levels = LevelDataManager.Instance.GetAllLevels();

        // loop over the list of levels as read in from the json
        // add a button for each level, 60px down from the last
        for (int i = 0; i < levels.Length; i++)
        {
            GameObject selector = Instantiate(button, level_selector.transform);
            selector.transform.localPosition = new Vector3(0, 130 - (i * 60));
            selector.GetComponent<MenuSelectorController>().spawner = this;
            selector.GetComponent<MenuSelectorController>().SetLevel(levels[i].name);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartLevel(string levelname)
    {
        GameManager.BroadcastLevelStart();
        levelData = LevelDataManager.Instance.GetDifficultyByName(levelname);
        StartCoroutine(SpawnWave());

    }

    public void NextWave()
    {
        StartCoroutine(SpawnWave());
    }

    void StopWave()
    {
        StopAllCoroutines();
    }


    IEnumerator SpawnWave()
    {
        GameManager.Instance.SetState(GameManager.GameState.COUNTDOWN);
        GameManager.Instance.countdown = 3;
        for (int i = 3; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            GameManager.Instance.countdown--;
        }
        GameManager.Instance.SetState(GameManager.GameState.INWAVE);
        GameManager.Instance.ResetWaveStats();
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

        // check for game win
        bool isFiniteMode = levelData.waves > 0;
        if (isFiniteMode && waveNum >= levelData.waves)
            GameManager.Instance.SetState(GameManager.GameState.WIN);
        else
            GameManager.Instance.SetState(GameManager.GameState.WAVEEND);
        Debug.Log("SpawnWave ended. waveNum=" + waveNum + " levelData.waves=" + levelData.waves + " state=" + GameManager.Instance.GetState());
    }

    IEnumerator SpawnEnemyType(SpawnEntry spawnEntry, int waveNum)
    {
        EnemyData enemyData = EnemyDataManager.Instance.GetEnemyByName(spawnEntry.enemy);
        RPNReturns rpnResults = CalculateSpawnExpressions.CalculateEnemyStats(spawnEntry, enemyData);
        float waitTime = 2;
        if (float.TryParse(spawnEntry.delay, out float result))
            waitTime = result;

        // fallback if sequence not defined
        int[] sequence = (spawnEntry.sequence != null && spawnEntry.sequence.Length > 0)
            ? spawnEntry.sequence
            : new int[] { 1 };
        int totalSpawned = 0;
        int seqIndex = 0;

        while (totalSpawned < rpnResults.count)
        {
            int toSpawnThisBatch = sequence[seqIndex % sequence.Length];
            toSpawnThisBatch = Mathf.Min(toSpawnThisBatch, rpnResults.count - totalSpawned);
            for (int i = 0; i < toSpawnThisBatch; i++)
            {
                SpawnPoint spawnPoint = GetRandomSpawnPoint(spawnEntry.location);
                SpawnEnemy(enemyData, rpnResults, spawnPoint);
            }

            totalSpawned += toSpawnThisBatch;
            seqIndex++;

            if (totalSpawned < rpnResults.count)
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

    void ResetSpawner()
    {
        waveNum = 0;
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

    public int giveWaveNum()
    {
        return waveNum;
    }
}
