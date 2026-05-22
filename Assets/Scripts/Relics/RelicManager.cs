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
            List<RelicData> offerList = new List<RelicData>();
            List<RelicData> available = data.FindAll(d =>
                !currentRelics.Exists(r => r.name == d.name));
            for (int i = 0; i < Mathf.Min(data.Count - currentRelics.Count, 3); i++)
            {
                int position = Random.Range(0, available.Count);
                offerList.Add(data[position]);
            }
        }
        // interact with UI

    }
}
