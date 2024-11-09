using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GameData : MonoBehaviour
{
    // Lista di stringhe di dimensione variabile
    public List<string> stringValues = new List<string>();

    public List<string> caselleSbagliate = new List<string>();

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
        // Crea una struttura che contiene sia la lista di stringhe che l'array di interi
        DataToSave data = new DataToSave
        {
            stringValues = stringValues,
            caselleSbagliate = caselleSbagliate
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
            }
        }
    }
}

// Classe per memorizzare i dati da salvare
[System.Serializable]
public class DataToSave
{
    [SerializeField] public List<string> stringValues;  // Lista del percorso del personaggio percorso
    [SerializeField] public List<string> caselleSbagliate; // Lista delle caselle che si sono sbagliate
}
