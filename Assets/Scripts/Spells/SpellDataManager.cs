using System.Collections.Generic;
using UnityEngine;

public class SpellDataManager : MonoBehaviour
{
    public static SpellDataManager Instance;

    [SerializeField] private TextAsset spellDataJson;

    private Dictionary<string, SpellData> spells = new();
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
            loadSpellData();
        }
    }

    private void loadSpellData()
    {
        spells = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, SpellData>>(spellDataJson.text);
    }

    public SpellData Get(string key)
    {
        spells.TryGetValue(key, out var data);
        return data;
    }
}
