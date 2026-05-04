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

        if (GameManager.Instance.state == GameManager.GameState.WAVEEND)
        {
            enemiesKilled.text = "Enemies Killed: " + GameManager.Instance.enemiesKilledThisWave;
            int seconds = Mathf.FloorToInt(GameManager.Instance.waveDuration);
            waveDuration.text = "Time: " + seconds + "s";
            rewardUI.SetActive(true);
        }
        else
        {
            rewardUI.SetActive(false);
        }
    }

    public void OnNextWaveClicked()
    {
        GameManager.BroadcastNextWave();
    }
}
