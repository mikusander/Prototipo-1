using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

    // Metodo per rimuovere un elemento dall'array
    public void RimuoviDomanda(Domanda domandaDaRimuovere)
    {
        // Se l'array è vuoto, non fare nulla
        if (domande == null || domande.Length == 0)
            return;

        // Calcolare la nuova dimensione
        int nuovaLunghezza = domande.Length - 1;
        Domanda[] nuovoArray = new Domanda[nuovaLunghezza];

        int indice = 0;
        foreach (var domanda in domande)
        {
            // Copia solo gli elementi diversi da quello da rimuovere
            if (domanda != domandaDaRimuovere)
            {
                if (indice >= nuovaLunghezza) break; // Evita errori di fuoriuscita
                nuovoArray[indice] = domanda;
                indice++;
            }
        }

        // Aggiorna l'array originale
        domande = nuovoArray;
    }
}

public class ControlNewGameplay : MonoBehaviour
{
    // Nome del file senza estensione (Unity gestirà la cartella Resources)
    private string fileName;
    public Camera mainCamera;
    [SerializeField] private GameData gameData;

    // Variabile per memorizzare la lista di domande
    private Domande listaDomande;
    private List<Domanda> domande = new List<Domanda>();
    public bool presenceOfBox = false;
    public bool inCreation = false;
    public Domanda currentQuestion;
    public int totalQuestion;
    public int numberQuestion;
    private System.Random random = new System.Random();
    public GameObject questionSpace;
    public int totalScore = 0;
    public bool gameover = false;
    private int questionCount = 0;


    void Start()
    {
        TempData.game = true;
        // Carica le domande dal file JSON
        fileName = Path.Combine(TempData.difficolta, "DomandeRisposte");
        CaricaDomande();
        totalQuestion = listaDomande.domande.Length;
        numberQuestion = totalQuestion;
    }

    void Update()
    {
        if (questionCount == 5 && !presenceOfBox)
        {
            gameover = true;

            if (totalScore > 2)
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

        if (!presenceOfBox && !gameover)
        {
            questionCount++;
            presenceOfBox = true;
            int randomQuestion = random.Next(numberQuestion);
            numberQuestion--;
            currentQuestion = domande[randomQuestion];
            domande.RemoveAt(randomQuestion);

            GameObject canvasGameobject = Instantiate(questionSpace);
            Canvas canvas = canvasGameobject.GetComponent<Canvas>();
            canvas.worldCamera = mainCamera;
            inCreation = true;
        }
    }

    void CaricaDomande()
    {
        // Carica il file JSON come TextAsset dalla cartella Resources/Easy
        TextAsset fileJSON = Resources.Load<TextAsset>(fileName);

        // Verifica se il file è stato caricato correttamente
        if (fileJSON != null)
        {
            // Deserializza il contenuto JSON
            listaDomande = JsonUtility.FromJson<Domande>(fileJSON.text);
            foreach (Domanda x in listaDomande.domande)
            {
                domande.Add(x);
            }
        }
        else
        {
            Debug.LogError("File non trovato nella cartella Resources!");
        }
    }
}
