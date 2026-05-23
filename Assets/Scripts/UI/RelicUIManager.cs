using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class RelicUIManager : MonoBehaviour
{
    public GameObject relicUIPrefab;
    public GameObject relicRewardUIPrefab;
    public GameObject rewardScreen;
    public static RelicUIManager Instance;

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
        }

        EventBus.Instance.OnRelicPickup += OnRelicPickup;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRelicPickup(Relic r)
    {
        // make a new Relic UI representation
        Debug.Log("new Relic ui code ran");
        GameObject rui = Instantiate(relicUIPrefab, transform);
        rui.transform.localPosition = new Vector3(-450 + 40 * (RelicManager.Instance.currentRelics.Count - 1), 0, 0);
        RelicUI ruic = rui.GetComponent<RelicUI>();
        ruic.index = RelicManager.Instance.currentRelics.Count - 1;
        
    }

    public void DisplayOfferedRelics(List<RelicData> relics)
    {
        for (int i = 0; i < relics.Count; i++)
        {
            GameObject rewardRelic = Instantiate(relicRewardUIPrefab, transform);
            rewardRelic.GetComponent<RelicRewardUI>().SetRelic(relics[i]);
            rewardRelic.transform.SetParent(rewardScreen.transform, false);
            float screenWidth = rewardScreen.GetComponent<RectTransform>().rect.width;
            float spacing = screenWidth / relics.Count;
            float startX = -screenWidth / 2 + spacing / 2;
            rewardRelic.transform.localPosition = new Vector2(startX + spacing * i, 10);
        }
    }
}
