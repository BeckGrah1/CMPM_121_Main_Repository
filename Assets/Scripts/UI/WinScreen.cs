using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    public GameObject winScreen;
    public Button returnButton;
    public TextMeshProUGUI enemiesKilled;
    public TextMeshProUGUI waveDuration;

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

    void Show()
    {
        enemiesKilled.text = "Enemies Killed: " + GameManager.Instance.enemiesKilledThisWave;
        int seconds = Mathf.FloorToInt(GameManager.Instance.waveDuration);
        waveDuration.text = "Time: " + seconds + "s";
        winScreen.SetActive(true);
    }

    void Hide() => winScreen.SetActive(false);
}
