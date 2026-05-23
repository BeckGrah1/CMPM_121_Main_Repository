using UnityEngine;
using TMPro;

public class MenuSelectorController : MonoBehaviour
{
    public TextMeshProUGUI label;
    public string level;
    public string playerClass;
    public EnemySpawner spawner;

    public void SetLevel(string text)
    {
        level = text;
        label.text = text;
    }

    public void ButtonPress()
    {
        if (level != "not set")
        {
            spawner.StartLevel(level);
        }
        if (playerClass != "not set")
        {
            GameManager.Instance.player.GetComponent<PlayerController>().setPlayerClass(playerClass);
            this.transform.parent.gameObject.SetActive(false);
        }
    }

    public void SetClass(string text)
    {
        playerClass = text;
        label.text = text;
    }
}
