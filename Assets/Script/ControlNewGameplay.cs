using UnityEngine;

[System.Serializable]
public class Domanda
{
    public string Tipo;
    public string Contenuto;
    public bool Risposta;
}

[System.Serializable]
public class Domande
{
    public Domanda[] domande;
}

public class ControlNewGameplay : MonoBehaviour
{
    // Nome del file senza estensione (Unity gestirà la cartella Resources)
    public string fileName = "Easy/DomandeRisposte";

    // Variabile per memorizzare la lista di domande
    private Domande domande;

    void Start()
    {
        // Carica le domande dal file JSON
        CaricaDomande();
    }

    void CaricaDomande()
    {
        // Carica il file JSON come TextAsset dalla cartella Resources/Easy
        TextAsset fileJSON = Resources.Load<TextAsset>(fileName);

        // Verifica se il file è stato caricato correttamente
        if (fileJSON != null)
        {
            // Deserializza il contenuto JSON
            domande = JsonUtility.FromJson<Domande>(fileJSON.text);

            // Stampa tutte le domande nella console
            foreach (var domanda in domande.domande)
            {
                Debug.Log("Tipo: " + domanda.Tipo);
                Debug.Log("Contenuto: " + domanda.Contenuto);
                Debug.Log("Risposta: " + domanda.Risposta);
            }
        }
        else
        {
            Debug.LogError("File non trovato nella cartella Resources!");
        }
    }
}
