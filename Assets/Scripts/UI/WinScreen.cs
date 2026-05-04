using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    public GameObject winScreen;
    public Button returnButton;

    void Start()
    {
        returnButton.onClick.AddListener(GameManager.BroadcastGameReset);
    }

    void OnEnable()
    {
        GameManager.OnWIN += Show;
        GameManager.OnPREGAME += Hide;
    }

    void OnDisable()
    {
        GameManager.OnWIN -= Show;
        GameManager.OnPREGAME -= Hide;
    }

    void Show() => winScreen.SetActive(true);
    void Hide() => winScreen.SetActive(false);
}
