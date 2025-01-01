using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayMusic("ambient", 0.5f);
        }
    }

    [SerializeField] private TextMeshProUGUI level1Perc;

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        level1Perc.text = PlayerPrefs.GetFloat("level1Best").ToString("0") + "%";
    }

    public void SetLevel(int lvl)
    {
        PlayerPrefs.SetInt("level", lvl);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
