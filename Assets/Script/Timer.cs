using UnityEngine;
using UnityEngine.UI; // Importa il namespace UI per usare Text

public class TimerScript : MonoBehaviour
{
    public float tempoTotale = 60f; // Durata del timer in secondi
    private float tempoRimanente;
    public Text testoTimer; // Trascina qui il GameObject Text

    public Controllo controllo;

    void Start()
    {
        tempoRimanente = tempoTotale; // Inizializza il tempo rimanente
    }

    void Update()
    {
        if (tempoRimanente > 1 && !controllo.gameover)
        {
            if(!controllo.inCreazione)
            {
                tempoRimanente -= Time.deltaTime; // Riduci il tempo rimanente
                int minuti = Mathf.FloorToInt(tempoRimanente / 60); // Calcola minuti
                int secondi = Mathf.FloorToInt(tempoRimanente % 60); // Calcola secondi

                // Aggiorna il testo del timer con i minuti e secondi rimanenti
                testoTimer.text = string.Format("{0:00}:{1:00}", minuti, secondi);
            }
        }
        else
        {
            controllo.gameover = true;
        }
    }
}
