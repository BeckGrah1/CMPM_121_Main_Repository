using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardScreenManager : MonoBehaviour
{
    public GameObject rewardUI;
    public TextMeshProUGUI enemiesKilled;
    public TextMeshProUGUI waveDuration;
    public Button nextWaveButton;

    void Start()
    {
        nextWaveButton.onClick.AddListener(OnNextWaveClicked);
    }

    void Update()
    {
        GameManager.Instance.TickWaveTimer(Time.deltaTime);
    }

    void OnEnable()
    {
        GameManager.OnWAVEEND += Show;
        GameManager.OnCOUNTDOWN += Hide;
    }

    void OnDisable()
    {
        GameManager.OnWAVEEND -= Show;
        GameManager.OnCOUNTDOWN -= Hide;
    }

    void Show()
    {
        enemiesKilled.text = "Enemies Killed: " + GameManager.Instance.enemiesKilledThisWave;
        int seconds = Mathf.FloorToInt(GameManager.Instance.waveDuration);
        waveDuration.text = "Time: " + seconds + "s";
        rewardUI.SetActive(true);
    }

    void Hide() => rewardUI.SetActive(false);

    public void OnNextWaveClicked()
    {
        GameManager.BroadcastNextWave();
    }
}
