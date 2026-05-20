using System.Collections.Generic;
using UnityEngine;

public class RelicDataManager : MonoBehaviour
{
    public static RelicDataManager Instance;

    [SerializeField] private TextAsset relicDataJson;

    private Dictionary<string, Relic> spells = new();
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
        spells = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Relic>>(relicDataJson.text);
    }

    public Relic Get(string key)
    {
        spells.TryGetValue(key, out var data);
        return data;
    }
}