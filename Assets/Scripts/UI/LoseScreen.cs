using UnityEngine;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour
{
    public GameObject loseScreen;
    public Button returnButton;

    void Start()
    {
        returnButton.onClick.AddListener(GameManager.BroadcastGameReset);
    }

    void OnEnable()
    {
        GameManager.OnGAMEOVER += Show;
        GameManager.OnPREGAME += Hide;
    }

    void OnDisable()
    {
        GameManager.OnGAMEOVER -= Show;
        GameManager.OnPREGAME -= Hide;
    }

    void Show() { Debug.Log("LoseScreen Show"); loseScreen.SetActive(true); }
    void Hide() => loseScreen.SetActive(false);
}
