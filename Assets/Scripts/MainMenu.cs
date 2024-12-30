using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetLevel(int lvl)
    {
        PlayerPrefs.SetInt("level", lvl);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
