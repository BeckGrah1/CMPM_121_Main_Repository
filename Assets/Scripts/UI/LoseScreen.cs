using UnityEngine;

public class LoseScreen : MonoBehaviour
{
    public GameObject loseScreen;

    private void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.GAMEOVER)
        {
            loseScreen.SetActive(true);
        }
        else
        {
            loseScreen.SetActive(false);
        }
    }

}
