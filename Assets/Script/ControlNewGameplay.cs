using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;

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

    // Variabile per memorizzare la lista di domande
    private Domande listaDomande;
    private List<Domanda> domande;
    public bool presenceOfBox = false;
    public Domanda currentQuestion;
    public int totalQuestion;
    public int numberQuestion;
    private System.Random random = new System.Random();
    [SerializeField] private GameObject questionSpace;
    public int totalScore = 0;

    void Start()
    {
        // Carica le domande dal file JSON
        TempData.difficolta = "Easy"; // da aggiungere in controllo mappa
        fileName = Path.Combine(TempData.difficolta, "DomandeRisposte");
        CaricaDomande();
        totalQuestion = listaDomande.domande.Length;
        numberQuestion = totalQuestion;
    }

    void Update()
    {
        if (!presenceOfBox)
        {
            presenceOfBox = true;
            int randomQuestion = random.Next(numberQuestion);
            currentQuestion = domande[randomQuestion];
            domande.RemoveAt(randomQuestion);
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
