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

    void Awake()
    {
        // Define the save file path
        filePath = Path.Combine(Application.persistentDataPath, "gameData.json");

        // Load saved data at startup, if it exists
        LoadData();
    }

    // Method for saving data to file
    public void SaveData()
    {
        // Creates a structure that contains the lists and the finishLine value
        DataToSave data = new DataToSave
        {
            correctBoxes = correctBoxes,
            wrongBoxes = wrongBoxes,
            finishLine = finishLine, // Salva anche il valore di finishLine
            start = start,
            lastLose = lastLose
        };

        // Serialize the object in JSON format
        string json = JsonUtility.ToJson(data, true); // Usa true per formattazione leggibile

        // Save the JSON file
        File.WriteAllText(filePath, json);
    }

    // Method for loading data from file
    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            // Read the JSON file
            string json = File.ReadAllText(filePath);

            // Deserialize the data
            DataToSave data = JsonUtility.FromJson<DataToSave>(json);

            // Check that the data is not null before assigning it
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
            // The file does not exist, so create it with initial data
            CreateDefaultFile();
        }
    }

    // Method to create a JSON file with initial data
    private void CreateDefaultFile()
    {
        // Initial data (edit as needed)
        DataToSave defaultData = new DataToSave
        {
            correctBoxes = new List<string>(),
            wrongBoxes = new List<string>(),
            finishLine = "",
            start = "",
            lastLose = new string[3] { "", "", "" }
        };

        // Serialize the initial data in JSON format
        string json = JsonUtility.ToJson(defaultData, true);

        // Write the data to the file
        File.WriteAllText(filePath, json);

        // Assign initial data to variables for immediate use
        correctBoxes = defaultData.correctBoxes;
        wrongBoxes = defaultData.wrongBoxes;
        finishLine = defaultData.finishLine;
        start = defaultData.start;
        lastLose = defaultData.lastLose;
    }
}

// Class to store the data to be saved
[System.Serializable]
public class DataToSave
{
    [SerializeField] public List<string> correctBoxes;
    [SerializeField] public List<string> wrongBoxes;
    [SerializeField] public string finishLine;
    [SerializeField] public string start;
    [SerializeField] public string[] lastLose;
}
