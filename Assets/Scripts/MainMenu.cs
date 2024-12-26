using UnityEngine;

public class MainMenu : MonoBehaviour
{
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetLevel(int lvl)
    {
        PlayerPrefs.SetInt("level", lvl);
    }
}
