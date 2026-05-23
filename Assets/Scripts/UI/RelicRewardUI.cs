using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RelicRewardUI : MonoBehaviour
{
    private RelicData relic;
    public Image icon;
    public TextMeshProUGUI label;
    public Image tooltipBackground;

    public void Awake()
    {
        GameManager.OnINWAVE += destroy;
    }
    public void SetRelic(RelicData r)
    {
        relic = r;
        GameManager.Instance.relicIconManager.PlaceSprite(r.sprite, icon);
        label.text = relic.name + ":\n" + relic.trigger.description + " " + relic.effect.description;
    }

    public void TakeRelic()
    {
        RelicManager.Instance.addRelic(relic);
        destroy();
    }

    private void destroy()
    {
        GameManager.OnINWAVE -= destroy;
        Destroy(this.gameObject);
    }
}
