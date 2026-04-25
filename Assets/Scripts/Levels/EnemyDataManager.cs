using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDataManager : MonoBehaviour
{
    public static EnemyDataManager Instance;
    public TextAsset enemyJSON;
    private EnemyList enemyList;

    // singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadEnemyData();
        }
    }

    // loads the enemy data from the enemy JSON File into an enemy list
    private void LoadEnemyData()
    {
        enemyList = JsonUtility.FromJson<EnemyList>(enemyJSON.text);
    }

    // Takes a string, and returns the EnemyData parsed from the enemy JSON file with that name
    // returns null if no enemy is found with that name
    public EnemyData GetEnemyByName(string name)
    {
        foreach (EnemyData enemy in enemyList.enemies)
        {
            if (enemy.name == name)
            {
                return enemy;
            }
        }
        Debug.Log("Enemy name " + name + "not found in JSON file");
        return null;
    }
}

// wrapper class for EnemyData
[System.Serializable]
public class EnemyList
{
    public EnemyData[] enemies;
}
