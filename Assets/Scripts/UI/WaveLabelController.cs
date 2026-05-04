using UnityEngine;
using TMPro;

public class WaveLabelController : MonoBehaviour
{
    enum DisplayMode { None, EnemyCount, Countdown }
    DisplayMode mode;

    TextMeshProUGUI tmp;

    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        GameManager.OnINWAVE += ShowEnemyCount;
        GameManager.OnCOUNTDOWN += ShowCountdown;
        GameManager.OnWAVEEND += HideLabel;
        GameManager.OnPREGAME += HideLabel;
    }

    void OnDisable()
    {
        GameManager.OnINWAVE -= ShowEnemyCount;
        GameManager.OnCOUNTDOWN -= ShowCountdown;
        GameManager.OnWAVEEND -= HideLabel;
        GameManager.OnPREGAME -= HideLabel;
    }

    void ShowEnemyCount() => mode = DisplayMode.EnemyCount;
    void ShowCountdown() => mode = DisplayMode.Countdown;
    void HideLabel() => mode = DisplayMode.None;

    void Update()
    {
        if (mode == DisplayMode.EnemyCount)
            tmp.text = "Enemies left: " + GameManager.Instance.enemy_count;
        else if (mode == DisplayMode.Countdown)
            tmp.text = "Starting in " + GameManager.Instance.countdown;
    }
}
