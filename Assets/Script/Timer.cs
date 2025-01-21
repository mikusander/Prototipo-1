using UnityEngine;
using UnityEngine.UI; // Importa il namespace UI per usare Text

public class TimerScript : MonoBehaviour
{
    private float tempoTotale = 60f; // Durata del timer in secondi
    private float tempoRimanente;
    [SerializeField] private Text testoTimer;

    [SerializeField] private Controllo controllo;

    void Start()
    {
        tempoRimanente = tempoTotale; // Initialize the remaining time
    }

    void Update()
    {
        if (tempoRimanente > 1 && !controllo.gameover)
        {
            if (!controllo.inCreazione)
            {
                tempoRimanente -= Time.deltaTime; // Reduce the remaining time
                int minuti = Mathf.FloorToInt(tempoRimanente / 60); // Calculate minutes
                int secondi = Mathf.FloorToInt(tempoRimanente % 60); // Calculate seconds

                // Update the timer text with minutes and seconds remaining
                testoTimer.text = string.Format("{0:00}:{1:00}", minuti, secondi);
            }
        }
        else
        {
            controllo.gameover = true;
        }
    }
}
