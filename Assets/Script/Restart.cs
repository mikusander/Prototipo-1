using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public ControlloMappa controlloMappa;
    // Funzione per ricaricare la scena attuale
    public void RestartCurrentScene()
    {
        controlloMappa.gameData.correctBoxes.Clear();
        controlloMappa.gameData.wrongBoxes.Clear();
        controlloMappa.gameData.start = "";
        controlloMappa.gameData.finishLine = "";
        controlloMappa.gameData.SaveData();
        // Ottiene il nome della scena attiva
        string currentSceneName = SceneManager.GetActiveScene().name;
        // Ricarica la scena attiva
        SceneManager.LoadScene(currentSceneName);
    }
}
