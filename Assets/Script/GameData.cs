using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GameData : MonoBehaviour
{
    // Liste di stringhe di dimensione variabile
    public List<string> correctBoxes = new List<string>();
    public List<string> wrongBoxes = new List<string>();

    // Variabili aggiuntive
    public string finishLine;
    public string start;
    public string[] lastLose = new string[3]; // Array di stringhe con due elementi

    // Percorso del file JSON
    private string filePath;

    void Awake() // Cambia da Start a Awake per essere sicuri che venga impostato prima di altri metodi
    {
        // Definire il percorso del file di salvataggio
        filePath = Path.Combine(Application.persistentDataPath, "gameData.json");

        // Carica i dati salvati all'avvio, se esistono
        LoadData();
    }

    // Metodo per salvare i dati su file
    public void SaveData()
    {
        // Crea una struttura che contiene le liste e il valore finishLine
        DataToSave data = new DataToSave
        {
            correctBoxes = correctBoxes,
            wrongBoxes = wrongBoxes,
            finishLine = finishLine, // Salva anche il valore di finishLine
            start = start,
            lastLose = lastLose
        };

        // Serializza l'oggetto in formato JSON
        string json = JsonUtility.ToJson(data, true); // Usa true per formattazione leggibile

        // Salva il file JSON
        File.WriteAllText(filePath, json);
    }

    // Metodo per caricare i dati dal file
    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            // Leggi il file JSON
            string json = File.ReadAllText(filePath);

            // Deserializza i dati
            DataToSave data = JsonUtility.FromJson<DataToSave>(json);

            // Verifica che i dati non siano null prima di assegnarli
            if (data != null)
            {
                correctBoxes = data.correctBoxes ?? new List<string>();
                wrongBoxes = data.wrongBoxes ?? new List<string>();
                finishLine = data.finishLine ?? string.Empty; // Carica il valore di finishLine
                start = data.start ?? string.Empty;
                lastLose = data.lastLose ?? new string[3]; // Imposta un array di default se null
            }
        }
        else
        {
            // Il file non esiste, quindi crealo con dati iniziali
            CreateDefaultFile();
        }
    }

    // Metodo per creare un file JSON con dati iniziali
    private void CreateDefaultFile()
    {
        // Dati iniziali (modifica secondo necessità)
        DataToSave defaultData = new DataToSave
        {
            correctBoxes = new List<string>(),      // Lista vuota di stringhe
            wrongBoxes = new List<string>(),        // Lista vuota di caselle sbagliate
            finishLine = "",                         // Valore iniziale di finishLine
            start = "",
            lastLose = new string[3] { "", "", "" }      // Array di default con due stringhe vuote
        };

        // Serializza i dati iniziali in formato JSON
        string json = JsonUtility.ToJson(defaultData, true);

        // Scrivi i dati nel file
        File.WriteAllText(filePath, json);

        // Assegna i dati iniziali alle variabili per usarli subito
        correctBoxes = defaultData.correctBoxes;
        wrongBoxes = defaultData.wrongBoxes;
        finishLine = defaultData.finishLine;
        start = defaultData.start;
        lastLose = defaultData.lastLose;
    }
}

// Classe per memorizzare i dati da salvare
[System.Serializable]
public class DataToSave
{
    [SerializeField] public List<string> correctBoxes;     // Lista del percorso del personaggio percorso
    [SerializeField] public List<string> wrongBoxes;      // Lista delle caselle che si sono sbagliate
    [SerializeField] public string finishLine;            // Variabile aggiuntiva "finishLine"
    [SerializeField] public string start;                 // Punto di partenza
    [SerializeField] public string[] lastLose;            // Array di stringhe con due elementi
}
