using TMPro;
using UnityEngine;

public class RewardScreenManager : MonoBehaviour
{
    public GameObject rewardUI;
    public TextMeshProUGUI enemiesKilled;
    public TextMeshProUGUI waveDuration;

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
}
