using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public enum UIStates
{
    Running,
    Paused,
    Lose,
    Win
}
public class UIController : MonoBehaviour
{
    //Constants
    private const float LOSEMENUTIMER = 0.3f;
    private const float PAUSEEXITTIME = 2f;

    [SerializeField] private TextMeshProUGUI scoreText1, scoreText2;
    [SerializeField] private TextMeshProUGUI loseScore;
    [SerializeField] private TextMeshProUGUI winScore;
    [SerializeField] private TextMeshProUGUI pauseExitText;

    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject pauseMenu;

    private float loseMenuTime = 0.0f;
    private float pauseExitTime = 0.0f;
    private bool playerLost = false;
    private bool exitingPause = true;

    private UIStates state = UIStates.Running;

    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void UpdateScore()
    {
        scoreText1.text = gameController.score.ToString("0");
        scoreText2.text = gameController.score.ToString("0");
    }

    private void Update()
    {
        loseMenuTime -= Time.deltaTime;
        pauseExitTime -= Time.unscaledDeltaTime;

        if (playerLost && loseMenuTime <= 0)
        {
            loseMenu.SetActive(true);
            playerLost = false;
        }
        else if (exitingPause)
        {
            pauseExitText.text = (pauseExitTime + 1).ToString("0");
            if (pauseExitTime <= 0)
            {
                exitingPause = false;
                pauseExitText.gameObject.SetActive(false);
                gameController.PauseTimeScale(false);
                state = UIStates.Running;
            }
        }
    }

    public void ShowLoseMenu()
    {
        state = UIStates.Lose;
        playerLost = true;
        loseScore.text = gameController.score.ToString("0");
        loseMenuTime = LOSEMENUTIMER;
    }

    public void ShowWinMenu()
    {
        state = UIStates.Win;
        winScore.text = gameController.score.ToString("0");
        winMenu.SetActive(true);
    }

    public void PauseGameMenu(bool b)
    {
            if (b && state == UIStates.Running)
            {
                state = UIStates.Paused;
                pauseMenu.SetActive(true);
            }
            else if (state == UIStates.Paused)
            {
                pauseMenu.SetActive(false);
                exitingPause = true;
                pauseExitTime = PAUSEEXITTIME;
                pauseExitText.gameObject.SetActive(true);
            }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ReturnToCharacterSelection()
    {
        SceneManager.LoadScene(1);
    }

}
