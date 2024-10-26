using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllo : MonoBehaviour
{

    // Variabile per la camera principale
    private Camera mainCamera;

    // Variabile pubblica per il prefab del Canvas
    public GameObject[] canvasPrefab;

    public float spawnDelay = 1.6f; // Ritardo prima di spawnare un nuovo prefab

    private GameObject canvasInstance;

    private int[] numeriUsati;
    private int countNumeriUsati;

    public bool gameover = false;

    private bool inCreazione = false;

    // Start is called before the first frame update
    void Start()
    {
        numeriUsati = new int[canvasPrefab.Length];
        // Recupera la Main Camera
        mainCamera = Camera.main;


        int randomIndex = Random.Range(0, canvasPrefab.Length);

        numeriUsati[countNumeriUsati] = randomIndex;

        countNumeriUsati += 1;

        GameObject prefabToSpawn = canvasPrefab[randomIndex];

        // Instanzia il prefab del Canvas
        canvasInstance = Instantiate(prefabToSpawn);

        // Assegna la Main Camera come Render Camera
        Canvas canvas = canvasInstance.GetComponent<Canvas>();

        canvas.worldCamera = mainCamera;
    }

    private void Update()
    {
        if (countNumeriUsati == canvasPrefab.Length) 
        {
            gameover = true;
        }
        // Controlla se il prefab attuale è null o distrutto
        if (canvasInstance == null && !inCreazione)
        {
            inCreazione = true;
            // Avvia la coroutine per spawnare un nuovo prefab
            StartCoroutine(SpawnPrefab());
        }
    }

    private IEnumerator SpawnPrefab()
    {

        // Scegli un prefab casuale dalla lista
        int randomIndex = Random.Range(0, canvasPrefab.Length);
        if (IsNumberInArray(numeriUsati, randomIndex))
        {
            Debug.Log("daje");
            // Aspetta un certo intervallo prima di spawnare
            yield return new WaitForSeconds(spawnDelay);

            countNumeriUsati += 1;

            GameObject prefabToSpawn = canvasPrefab[randomIndex];

            // Instanzia il prefab del Canvas
            canvasInstance = Instantiate(prefabToSpawn);

            // Assegna la Main Camera come Render Camera
            Canvas canvas = canvasInstance.GetComponent<Canvas>();

            canvas.worldCamera = mainCamera;

            inCreazione = false;
        }
        else
        {
            Debug.Log("mannaggia");
        }
    }

    // Funzione che verifica se un numero è presente nell'array
    bool IsNumberInArray(int[] array, int number)
    {
        foreach (int element in array)
        {
            if (element == number)
            {
                return true; // Il numero è presente nell'array
            }
        }
        return false; // Il numero non è presente nell'array
    }
}
