using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Controllo : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string text;
        public bool correctAnswer;

        public Question(string text, bool correctAnswer)
        {
            this.text = text;
            this.correctAnswer = correctAnswer;
        }
    }

    // Variabile per la camera principale
    private Camera mainCamera;

    // Variabile pubblica per il prefab del Canvas
    public GameObject canvasPrefab;

    public float spawnDelay = 1.6f; // Ritardo prima di spawnare un nuovo prefab

    private GameObject canvasInstance;

    public bool gameover = false;

    public bool inCreazione = false;

    // Lista di domande
    public List<Question> questions = new List<Question>();
    public int numeroDomande;
    public Question currentQuestion;
    public int puntiAttuali = 0;
    public bool isUltima = false;

    // Start is called before the first frame update
    void Start()
    {
        // Carica le domande dal file di testo
        LoadQuestionsFromFile();
        numeroDomande = questions.Count;

        // Recupera la Main Camera
        mainCamera = Camera.main;

        int randomIndex = Random.Range(0, numeroDomande);

        currentQuestion = questions[randomIndex];

        // Rimuove la domanda usata dalla lista
        questions.RemoveAt(randomIndex);

        numeroDomande = questions.Count;

        // Instanzia il prefab del Canvas
        canvasInstance = Instantiate(canvasPrefab);

        // Assegna la Main Camera come Render Camera
        Canvas canvas = canvasInstance.GetComponent<Canvas>();

        canvas.worldCamera = mainCamera;
    }

    private void Update()
    {
        if (questions.Count == 0 && !isUltima) 
        {
            gameover = true;
        }

        // Controlla se il prefab attuale � null o distrutto
        if (canvasInstance == null && !inCreazione && !gameover && !isUltima)
        {
            inCreazione = true;
            // Avvia la coroutine per spawnare un nuovo prefab
            StartCoroutine(SpawnPrefab());
        }
    }

    private IEnumerator SpawnPrefab()
    {
        // Aspetta un certo intervallo prima di spawnare
        yield return new WaitForSeconds(spawnDelay);

        // Instanzia il prefab del Canvas
        canvasInstance = Instantiate(canvasPrefab);

        // Scegli un prefab casuale dalla lista
        int randomIndex = Random.Range(0, numeroDomande);
        currentQuestion = questions[randomIndex];

        // Rimuove la domanda usata dalla lista
        questions.RemoveAt(randomIndex);
        
        numeroDomande = questions.Count;

        if (questions.Count == 0)
        {
            isUltima = true;
        }

        // Assegna la Main Camera come Render Camera
        Canvas canvas = canvasInstance.GetComponent<Canvas>();

        canvas.worldCamera = mainCamera;

        inCreazione = false;
    }

    void LoadQuestionsFromFile()
    {
        // Carica il file di testo dalla cartella Resources
        TextAsset questionData = Resources.Load<TextAsset>("Domande");

        // Controlla se il file � stato trovato
        if (questionData == null)
        {
            Debug.LogError("File Domande.txt non trovato in Resources!");
            return;
        }

        // Leggi ogni riga e aggiungi le domande alla lista
        string[] lines = questionData.text.Split('\n');
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue; // Salta righe vuote

            // Dividi la linea in domanda e risposta
            string[] parts = line.Split(';');
            if (parts.Length == 2)
            {
                string questionText = parts[0].Trim();
                bool correctAnswer = bool.Parse(parts[1].Trim());

                // Aggiungi la domanda alla lista
                questions.Add(new Question(questionText, correctAnswer));
            }
            else
            {
                Debug.LogWarning("Formato domanda non valido: " + line);
            }
        }
    }
}