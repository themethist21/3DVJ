using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class ClearPlayerPrefsOnBuild : IPreprocessBuildWithReport
{
    public int callbackOrder => 0; // Orden de ejecución (puedes dejarlo en 0)

    public void OnPreprocessBuild(BuildReport report)
    {
        // Limpia los valores de PlayerPrefs antes de construir el juego
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs limpiadas antes de la construcción.");
    }
}
