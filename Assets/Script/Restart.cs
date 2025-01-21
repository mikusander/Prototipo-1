using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public ControlloMappa controlloMappa;
    // Function to reload the current scene
    public void RestartCurrentScene()
    {
        controlloMappa.gameData.correctBoxes.Clear();
        controlloMappa.gameData.wrongBoxes.Clear();
        controlloMappa.gameData.start = "";
        controlloMappa.gameData.finishLine = "";
        controlloMappa.gameData.SaveData();
        // Gets the name of the active scene
        string currentSceneName = SceneManager.GetActiveScene().name;
        // Reload the active scene
        SceneManager.LoadScene(currentSceneName);
    }
}
