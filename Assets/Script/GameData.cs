using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GameData : MonoBehaviour
{
    // Liste di stringhe di dimensione variabile
    public List<string> stringValues = new List<string>();
    public List<string> caselleSbagliate = new List<string>();

    // Variabile aggiuntiva per "traguardo"
    public string traguardo;
    public string inizio;

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
        // Crea una struttura che contiene le liste e il valore traguardo
        DataToSave data = new DataToSave
        {
            stringValues = stringValues,
            caselleSbagliate = caselleSbagliate,
            traguardo = traguardo, // Salva anche il valore di traguardo
            inizio = inizio
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
                stringValues = data.stringValues ?? new List<string>();
                caselleSbagliate = data.caselleSbagliate ?? new List<string>();
                traguardo = data.traguardo ?? string.Empty; // Carica il valore di traguardo
                inizio = data.inizio ?? string.Empty;
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
            stringValues = new List<string>(),      // Lista vuota di stringhe
            caselleSbagliate = new List<string>(),  // Lista vuota di caselle sbagliate
            traguardo = "",                         // Valore iniziale di traguardo
            inizio = ""
        };

        // Serializza i dati iniziali in formato JSON
        string json = JsonUtility.ToJson(defaultData, true);

        // Scrivi i dati nel file
        File.WriteAllText(filePath, json);

        // Assegna i dati iniziali alle variabili per usarli subito
        stringValues = defaultData.stringValues;
        caselleSbagliate = defaultData.caselleSbagliate;
        traguardo = defaultData.traguardo;
        inizio = defaultData.inizio;
    }
}

// Classe per memorizzare i dati da salvare
[System.Serializable]
public class DataToSave
{
    [SerializeField] public List<string> stringValues;     // Lista del percorso del personaggio percorso
    [SerializeField] public List<string> caselleSbagliate; // Lista delle caselle che si sono sbagliate
    [SerializeField] public string traguardo;             // Variabile aggiuntiva "traguardo"
    [SerializeField] public string inizio;
}
