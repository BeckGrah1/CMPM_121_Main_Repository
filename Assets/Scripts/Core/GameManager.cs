using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameManager 
{
    public enum GameState
    {
        PREGAME,
        INWAVE,
        WAVEEND,
        COUNTDOWN,
        GAMEOVER,
        WIN
    }
    public GameState state;

    public int countdown;
    private static GameManager theInstance;
    public static GameManager Instance {  get
        {
            if (theInstance == null)
                theInstance = new GameManager();
            return theInstance;
        }
    }

    public GameObject player;
    
    public ProjectileManager projectileManager;
    public SpellIconManager spellIconManager;
    public EnemySpriteManager enemySpriteManager;
    public PlayerSpriteManager playerSpriteManager;
    public RelicIconManager relicIconManager;

    public static event Action OnLevelStart;
    public static event Action OnGameReset;
    public static event Action OnNextWave;

    public static event Action OnPREGAME;
    public static event Action OnINWAVE;
    public static event Action OnWAVEEND;
    public static event Action OnCOUNTDOWN;
    public static event Action OnGAMEOVER;
    public static event Action OnWIN;

    private List<GameObject> enemies;
    public int enemy_count { get { return enemies.Count; } }
    private int _enemiesKilledThisWave = 0;
    public int enemiesKilledThisWave { get { return _enemiesKilledThisWave; } }
    private float _waveDuration = 0f;
    public float waveDuration { get { return _waveDuration; } }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        _enemiesKilledThisWave++;
    }

    // used to track the duration of a wave
    public void TickWaveTimer(float deltaTime)
    {
        if (state == GameManager.GameState.INWAVE)
            _waveDuration += deltaTime;
    }

    // Add this to ResetWaveStats()
    public void ResetWaveStats()
    {
        _enemiesKilledThisWave = 0;
        _waveDuration = 0f;
    }

    public GameObject GetClosestEnemy(Vector3 point)
    {
        if (enemies == null || enemies.Count == 0) return null;
        if (enemies.Count == 1) return enemies[0];
        return enemies.Aggregate((a,b) => (a.transform.position - point).sqrMagnitude < (b.transform.position - point).sqrMagnitude ? a : b);
    }

    public void ClearEnemies()
    {
        enemies.Clear();
    }

    public static void BroadcastLevelStart()
    {
        OnLevelStart?.Invoke();
    }

    public static void BroadcastGameReset()
    {
        OnGameReset?.Invoke();
    }

    public static void BroadcastNextWave()
    {
        OnNextWave?.Invoke();
    }

    public void SetState(GameState newState)
    {
        state = newState;
        switch(newState)
        {
            case GameState.PREGAME:     OnPREGAME?.Invoke();    break;
            case GameState.INWAVE:      OnINWAVE?.Invoke();     break;
            case GameState.WAVEEND:     OnWAVEEND.Invoke();     break;
            case GameState.COUNTDOWN:   OnCOUNTDOWN.Invoke();   break;
            case GameState.GAMEOVER:    OnGAMEOVER.Invoke();    break;
            case GameState.WIN:         OnWIN.Invoke();         break;
        }
    }


    private GameManager()
    {
        enemies = new List<GameObject>();
    }
}
