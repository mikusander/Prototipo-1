using System.IO;
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
    private string fileName;

    // Variabile per memorizzare la lista di domande
    private Domande domande;

    void Start()
    {
        // Carica le domande dal file JSON
        TempData.difficolta = "Easy";
        fileName = Path.Combine(TempData.difficolta, "DomandeRisposte");
        CaricaDomande();
    }

    void Update()
    {

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
        }
        else
        {
            Debug.LogError("File non trovato nella cartella Resources!");
        }
    }
}
