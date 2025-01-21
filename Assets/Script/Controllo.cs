using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private Camera mainCamera;
    [SerializeField] private GameObject canvasPrefab;

    private float spawnDelay = 1.6f;

    private GameObject canvasInstance;

    public bool gameover = false;

    public bool inCreazione = false;
    private List<Question> questions = new List<Question>();
    private int numeroDomande;
    public Question currentQuestion;
    public int puntiAttuali = 0;
    public bool isUltima = true;
    public GameData gameData;
    public int totaleDomande;
    private int vitt;

    // Start is called before the first frame update
    void Start()
    {
        // Load saved data (if it exists)
        gameData = GetComponent<GameData>();
        if (gameData != null)
        {
            gameData.LoadData();
        }

        // Assign true to the variable game to indicate that the game has been played, it is used to subsequently start the animation
        TempData.game = true;
        TempData.vittoria = false;

        // Load questions from text file
        LoadQuestionsFromFile();
        numeroDomande = questions.Count;
        totaleDomande = questions.Count;
        vitt = (int)(totaleDomande / 2);

        // Recover the Main Camera
        mainCamera = Camera.main;

        // takes a random question
        int randomIndex = Random.Range(0, numeroDomande);
        currentQuestion = questions[randomIndex];

        // Removes the used question from the list
        questions.RemoveAt(randomIndex);
        numeroDomande = questions.Count;

        // Instantiate the Canvas prefab
        canvasInstance = Instantiate(canvasPrefab);

        // Assign the Main Camera as the Render Camera
        Canvas canvas = canvasInstance.GetComponent<Canvas>();

        canvas.worldCamera = mainCamera;
    }

    private void Update()
    {
        // checks whether the player won or lost
        if (gameover)
        {
            if (puntiAttuali >= vitt + 1)
            {
                gameData.correctBoxes.Add(TempData.lastBox);
                gameData.SaveData();
                TempData.vittoria = true;
            }
            else
            {
                gameData.wrongBoxes.Add(TempData.lastBox);
                gameData.SaveData();
                TempData.vittoria = false;
            }
            SceneManager.LoadScene("Mappa");
        }
        else if (questions.Count == 0 && !isUltima)
        {
            gameover = true;
            if (puntiAttuali >= vitt + 1)
            {
                gameData.correctBoxes.Add(TempData.lastBox);
                gameData.SaveData();
                TempData.vittoria = true;
            }
            else
            {
                gameData.wrongBoxes.Add(TempData.lastBox);
                gameData.SaveData();
                TempData.vittoria = false;
            }
            SceneManager.LoadScene("Mappa");
        }

        // Check if the current prefab is null
        if (!TempData.animazione && canvasInstance == null && !inCreazione && !gameover && !isUltima)
        {
            inCreazione = true;
            StartCoroutine(SpawnPrefab());
        }
    }

    private IEnumerator SpawnPrefab()
    {
        yield return new WaitForSeconds(spawnDelay);
        canvasInstance = Instantiate(canvasPrefab);

        // choose a random question
        int randomIndex = Random.Range(0, numeroDomande);
        currentQuestion = questions[randomIndex];
        questions.RemoveAt(randomIndex);
        numeroDomande = questions.Count;

        // If it is the last question it makes the isUltima variable true
        if (questions.Count == 0)
        {
            isUltima = true;
        }

        Canvas canvas = canvasInstance.GetComponent<Canvas>();
        canvas.worldCamera = mainCamera;
        inCreazione = false;
    }

    void LoadQuestionsFromFile()
    {

        // Load the text file from the Resources folder
        TextAsset questionData = Resources.Load<TextAsset>(TempData.difficolta);

        // Check if the file was found
        if (questionData == null)
        {
            Debug.LogError("File Domande.txt non trovato in Resources!");
            return;
        }

        // Read each line and add questions to the list
        string[] lines = questionData.text.Split('\n');
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split(';');
            if (parts.Length == 2)
            {
                string questionText = parts[0].Trim();
                bool correctAnswer = bool.Parse(parts[1].Trim());

                questions.Add(new Question(questionText, correctAnswer));
            }
            else
            {
                Debug.LogWarning("Formato domanda non valido: " + line);
            }
        }
    }

    IEnumerator AnimationMano(Animator animator)
    {
        TempData.animazione = true;
        animator.SetTrigger("Attivazione");

        AnimatorStateInfo animationInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = animationInfo.length;

        float dura = 0.6f;
        yield return new WaitForSeconds(dura);
        TempData.animazione = false;
    }
}
