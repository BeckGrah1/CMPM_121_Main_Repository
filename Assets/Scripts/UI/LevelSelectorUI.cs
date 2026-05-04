using UnityEngine;

public class LevelSelectorUI : MonoBehaviour
{
    void OnEnable()
    {
        GameManager.OnPREGAME += Show;
        GameManager.OnLevelStart += Hide;
    }

    void OnDisable()
    {
        GameManager.OnPREGAME -= Show;
        GameManager.OnLevelStart -= Hide;
    }

    void Show() => gameObject.SetActive(true);
    void Hide() => gameObject.SetActive(false);
}
