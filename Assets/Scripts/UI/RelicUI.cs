using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class RelicUI : MonoBehaviour
{
    public int index;

    public Image icon;
    public GameObject highlight;
    public TextMeshProUGUI label;
    public Image tooltipBackground;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Relic r = RelicManager.Instance.currentRelics[index];
        GameManager.Instance.relicIconManager.PlaceSprite(r.sprite, icon);
        label.text = r.GetLabel();
    }

    // Update is called once per frame
    void Update()
    {
        Relic r = RelicManager.Instance.currentRelics[index];
        highlight.SetActive(r.effect.IsActive());
        StartCoroutine(SetupTooltip());
    }

    IEnumerator SetupTooltip()
    {
        yield return new WaitForEndOfFrame();
        float borderHeight = tooltipBackground.GetComponent<Image>().sprite.border.w + tooltipBackground.GetComponent<Image>().sprite.border.y;
        Debug.Log("preferredHeight: " + label.preferredHeight + " borderHeight: " + borderHeight);
        tooltipBackground.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, label.preferredHeight + borderHeight * 2);
        label.rectTransform.anchoredPosition = new Vector2(0, (tooltipBackground.rectTransform.rect.height / 2) - borderHeight - (label.rectTransform.rect.height / 2));
        tooltipBackground.rectTransform.anchoredPosition = new Vector2(0, -(icon.rectTransform.rect.height / 2) - (tooltipBackground.rectTransform.rect.height / 2));
    }
}
