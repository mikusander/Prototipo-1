using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GameData : MonoBehaviour
{
    // Lista di stringhe di dimensione variabile
    public List<string> stringValues = new List<string>();

    // Array di interi con due valori
    public int[] intValues = new int[2];

    // Percorso del file JSON
    private string filePath;

    void Start()
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
        DataToSave data = new DataToSave();
        data.stringValues = stringValues;
        data.intValues = intValues;

        // Serializza l'oggetto in formato JSON
        string json = JsonUtility.ToJson(data);
        
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

            // Assegna i dati alla variabile
            stringValues = data.stringValues;
            intValues = data.intValues;
        }
        else
        {
            // Impostazioni di default se non esistono i dati salvati
            stringValues.Add("Default String");
            intValues[0] = 10;
            intValues[1] = 20;
        }
    }
}

// Classe per memorizzare i dati da salvare
[System.Serializable]
public class DataToSave
{
    public List<string> stringValues;  // Lista di stringhe
    public int[] intValues;            // Array di interi (2 valori)
}
