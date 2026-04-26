using UnityEngine;

public class LevelDataManager : MonoBehaviour
{
    public static LevelDataManager Instance;
    public TextAsset levelJSON;
    private LevelList levelList;

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
            LoadLevelData();
        }
    }

    private void LoadLevelData()
    {
        levelList = JsonUtility.FromJson<LevelList>(levelJSON.text);
    }

    public LevelData GetDifficultyByName(string name)
    {
        foreach (LevelData level in levelList.enemies)
        {
            if (level.difficulty == name)
            {
                return level;
            }
        }
        Debug.Log("Level name / difficulty " + name + "not found in JSON file");
        return null;
    }
}

[System.Serializable]
public class LevelList
{
    public LevelData[] enemies;
}
