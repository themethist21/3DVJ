using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI level1Perc;

    [SerializeField] private TextMeshProUGUI level2Perc;

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        level1Perc.text = PlayerPrefs.GetFloat("level1Best").ToString("0") + "%";

        level2Perc.text = PlayerPrefs.GetFloat("level2Best").ToString("0") + "%";
    }

    public void SetLevel(int lvl)
    {
        PlayerPrefs.SetInt("level", lvl);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Cargando Escena selección personaje
    }
}
