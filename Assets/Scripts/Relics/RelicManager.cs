using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
public class RelicManager : MonoBehaviour
{
    List<Relic> currentRelics = new List<Relic>();
    public int wavesPerRelic = 3;

    public static RelicManager Instance;

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
            GameManager.OnWAVEEND += offerRelic;
        }
    }

    public void addRelic(Relic relic)
    {
        currentRelics.Add(relic);
    }

    public void removeRelic(Relic relic)
    {
        currentRelics.Remove(relic);
    }

    void Update()
    {
        foreach (Relic relic in currentRelics)
        {
            relic.trigger.Update();
        }
    }

    void offerRelic()
    {
        if (GameManager.Instance.waveNum % wavesPerRelic == 0)
        {
            List<RelicData> data = RelicDataManager.Instance.GetAll();
            if (data.Count == currentRelics.Count)
            {
                Debug.LogWarning("no relics to offer");
            }
        }
    }
}
