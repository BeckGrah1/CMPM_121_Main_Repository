using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RelicDataManager : MonoBehaviour
{
    public static RelicDataManager Instance;

    [SerializeField] private TextAsset relicDataJson;

    private List<RelicData> relics = new();
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
            loadRelicData();
        }
    }

    private void loadRelicData()
    {
        relics = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RelicData>>(relicDataJson.text);
    }

    public RelicData Get(string key)
    {
        foreach(RelicData relic in relics) {
            if (relic.name == key)
            {
                return relic;
            }
        }
        return null;
    }

    public List<RelicData> GetAll()
    {
        return relics;
    }
}