using RPNEvaluator;
using System.Collections.Generic;
using UnityEngine;

// calculates the necessary RPN expressions, gives back a class with the new values
public class CalculateSpawnExpressions : MonoBehaviour
{
    static RPNReturns returns;
    static int waveNum = 0;

    private void Awake()
    {
        GameManager.OnWAVEEND += UpdateWaveNum;
        GameManager.OnGAMEOVER += ResetWaveNum;
    }

    //def not the best way to do this but...whatever
    public static RPNReturns CalculateEnemyStats(SpawnEntry spawnEntry, EnemyData enemyData)
    {
        Debug.Log("wave num: " + waveNum);
        returns = new RPNReturns();
        Dictionary<string, int> dict = new Dictionary<string, int>
        {
            {"wave", waveNum },
        };
        returns.count = RPNEvaluator.RPNEvaluator.Evaluate(spawnEntry.count, dict);
        if (spawnEntry.hp != null)
        {
            dict = new Dictionary<string, int>
            {
                {"wave", waveNum },
                {"base", enemyData.hp }
            };
            returns.hp = RPNEvaluator.RPNEvaluator.Evaluate(spawnEntry.hp, dict);
        }
        else
        {
            // default if no hp scaling
            returns.hp = enemyData.hp;
        }
        if (spawnEntry.speed != null)
        {
            dict = new Dictionary<string, int>
            {
                {"wave", waveNum },
                {"base", enemyData.speed }
            };
            returns.speed = RPNEvaluator.RPNEvaluator.Evaluate(spawnEntry.speed, dict);
        }
        else
        {
            // default if no speed scaling
            returns.speed = enemyData.speed;
        }
        if (spawnEntry.damage != null)
        {
            dict = new Dictionary<string, int>
            {
                {"wave", waveNum },
                {"base", enemyData.damage }
            };
            returns.damage = RPNEvaluator.RPNEvaluator.Evaluate(spawnEntry.damage, dict);
        }
        else
        {
            // default if no damage scaling
            returns.damage = enemyData.damage;
        }

        return returns;
    }

    public static RPNReturns CalculatePlayerStats(PlayerClass playerClass)
    {
        returns = new RPNReturns();
        Dictionary<string, int> dict;
        if (playerClass.health != null)
        {
            dict = new Dictionary<string, int>
            {
                {"wave", waveNum },
            };
            returns.hp = RPNEvaluator.RPNEvaluator.Evaluate(playerClass.health, dict);
        }
        if (playerClass.mana != null)
        {
            dict = new Dictionary<string, int>
            {
                {"wave", waveNum },
            };
            returns.mana = RPNEvaluator.RPNEvaluator.Evaluate(playerClass.mana, dict);
        }
        if (playerClass.mana_regeneration != null)
        {
            dict = new Dictionary<string, int>
            {
                {"wave", waveNum },
            };
            returns.mana_regen = RPNEvaluator.RPNEvaluator.Evaluate(playerClass.mana_regeneration, dict);
        }
        if (playerClass.spellpower != null)
        {
            dict = new Dictionary<string, int>
            {
                {"wave", waveNum },
            };
            returns.spellpower = RPNEvaluator.RPNEvaluator.Evaluate(playerClass.spellpower, dict);
        }
        if (playerClass.speed != null)
        {
            dict = new Dictionary<string, int>
            {
                {"wave", waveNum },
            };
            returns.speed = RPNEvaluator.RPNEvaluator.Evaluate(playerClass.speed, dict);
        }
        return returns;
    }

    public void UpdateWaveNum()
    {
        waveNum++;
    }

    public void ResetWaveNum()
    {
        waveNum = 0;
    }
}

[System.Serializable]
public class RPNReturns
{
    public int count;
    public int hp;
    public int speed;
    public int damage;
    public int mana;
    public int mana_regen;
    public int spellpower;
}
