using UnityEngine;

public class PlayerClassManager : MonoBehaviour
{
    public static PlayerClassManager Instance;
    public TextAsset playerJSON;
    private PlayerClassList playerClassList;

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
            loadPlayerData();
        }
    }

    void loadPlayerData()
    {
        playerClassList = JsonUtility.FromJson<PlayerClassList>(playerJSON.text);
    }

    public PlayerClass getPlayerClass(string name)
    {
        foreach (PlayerClass playerClass in playerClassList.playerClasses)
        {
            if (playerClass.name == name)
            {
                return playerClass;
            }
        }
        return null;
    }
}

[System.Serializable]
public class PlayerClassList
{
    public PlayerClass[] playerClasses;
}
