using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GameData : MonoBehaviour
{
    public List<string> correctBoxes = new List<string>();
    public List<string> wrongBoxes = new List<string>();
    public int start;
    public string[] lastLose = new string[3];

    private string filePath;

    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "gameData.json");
        LoadData();
    }

    public void SaveData()
    {
        DataToSave data = new DataToSave
        {
            correctBoxes = correctBoxes,
            wrongBoxes = wrongBoxes,
            start = start,
            lastLose = lastLose
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            DataToSave data = JsonUtility.FromJson<DataToSave>(json);

            if (data != null)
            {
                correctBoxes = data.correctBoxes ?? new List<string>();
                wrongBoxes = data.wrongBoxes ?? new List<string>();
                start = data.start;
                lastLose = data.lastLose ?? new string[3];
            }
        }
        else
        {
            CreateDefaultFile();
        }
    }

    private void CreateDefaultFile()
    {
        DataToSave defaultData = new DataToSave
        {
            correctBoxes = new List<string>(),
            wrongBoxes = new List<string>(),
            start = -1,
            lastLose = new string[3] { "", "", "" }
        };

        string json = JsonUtility.ToJson(defaultData, true);
        File.WriteAllText(filePath, json);

        correctBoxes = defaultData.correctBoxes;
        wrongBoxes = defaultData.wrongBoxes;
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
    [SerializeField] public int start;
    [SerializeField] public string[] lastLose;
}

