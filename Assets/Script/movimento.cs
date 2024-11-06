using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class DragGameObject2D : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    public GameObject yesRect;
    public GameObject noRect;
    private Vector3 inizialPosition;
    private Vector3 yesPosition = new Vector3(4.5f, 0f, 0f);
    private Vector3 noPosition = new Vector3(-1f, 0f, 0f);
    public float destroyDelay = 1f;
    public GameObject ditoInSu;
    public GameObject ditoInGiu;
    private Vector3 spawnPosition = new Vector3(0, 1, 0);
    public GameObject canva;
    public Text testo;
    public Controllo controllo;
    public Punti punti;

    void Start()
    {
        controllo = GameObject.Find("GameController").GetComponent<Controllo>();
        testo.text = controllo.currentQuestion.text;

        inizialPosition = transform.position;
        mainCamera = Camera.main;  // Riferimento alla telecamera principale
    }

    void Update()
    {
        if (controllo.gameover)
        {
            Destroy(canva);
            Destroy(gameObject);
        }
        // Gestisce l'input del mouse
        if (Input.GetMouseButtonDown(0))
        {
            // Verifica se abbiamo cliccato sul GameObject tramite un Raycast
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                // Calcola l'offset tra la posizione del GameObject e il clic del mouse
                offset = transform.position - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (transform.position.x < noPosition.x)
            {
                if (controllo.currentQuestion.correctAnswer)
                {
                    controllo.puntiAttuali += 1;
                    controllo.isUltima = false;
                    Destroy(canva);
                    GameObject instance = Instantiate(ditoInSu, spawnPosition, Quaternion.identity);
                    Destroy(instance, destroyDelay);
                    Destroy(gameObject);
                }
                else
                {
                    controllo.isUltima = false;
                    Destroy(canva);
                    GameObject instance = Instantiate(ditoInGiu, spawnPosition, Quaternion.identity);
                    Destroy(instance, destroyDelay);
                    Destroy(gameObject);
                }
            }
            else if (transform.position.x > yesPosition.x)
            {
                if (controllo.currentQuestion.correctAnswer)
                {
                    controllo.isUltima = false;
                    Destroy(canva);
                    GameObject instance = Instantiate(ditoInGiu, spawnPosition, Quaternion.identity);
                    Destroy(instance, destroyDelay);
                    Destroy(gameObject);
                }
                else
                {
                    controllo.isUltima = false;
                    controllo.puntiAttuali += 1;
                    Destroy(canva);
                    GameObject instance = Instantiate(ditoInSu, spawnPosition, Quaternion.identity);
                    Destroy(instance, destroyDelay);
                    Destroy(gameObject);
                }
            }
            else 
            {
                transform.position = inizialPosition;
            }
            isDragging = false;  // Quando rilasciamo il mouse, smettiamo di trascinare
        }

        if(transform.position.x > inizialPosition.x && isDragging)
        {
            yesRect.SetActive(false);
            noRect.SetActive(true);
        }
        else if (transform.position.x < inizialPosition.x)
        {
            noRect.SetActive(false);
            yesRect.SetActive(true);
        }

        if (isDragging)
        {
            // Aggiorna la posizione del GameObject mentre trasciniamo
            Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition) + offset;
            newPosition.z = 0;  // Manteniamo la posizione Z costante in 2D
            newPosition.y = transform.position.y;
            transform.position = newPosition;
        }


    }
}
