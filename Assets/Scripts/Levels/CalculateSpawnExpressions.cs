using RPNEvaluator;
using System.Collections.Generic;
using UnityEngine;

// calculates the necessary RPN expressions, gives back a class with the new values
public class CalculateSpawnExpressions : MonoBehaviour
{
    public static RPNReturns Calculate(SpawnEntry spawnEntry, EnemyData enemyData, int waveNum)
    {
        Debug.Log("wave num: " + waveNum);
        RPNReturns returns = new RPNReturns();
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
}

[System.Serializable]
public class RPNReturns
{
    public int count;
    public int hp;
    public int speed;
    public int damage;
}
