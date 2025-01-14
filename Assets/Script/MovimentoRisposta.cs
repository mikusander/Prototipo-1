using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoRisposta : MonoBehaviour
{

    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    private Vector3 inizialPosition;
    private Vector3 yesPosition = new Vector3(4.5f, 0f, 0f);
    private Vector3 noPosition = new Vector3(-1f, 0f, 0f);
    public GameObject yesText;
    public GameObject noText;

    // Start is called before the first frame update
    void Start()
    {
        inizialPosition = transform.position;
        mainCamera = Camera.main;  // Riferimento alla telecamera principale
    }

    // Update is called once per frame
    void Update()
    {
        // Gestisce l'input del mouse
        if (Input.GetMouseButtonDown(0))
        {
            // Verifica se abbiamo cliccato sul GameObject tramite un Raycast
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log("daje");
                isDragging = true;
                // Calcola l'offset tra la posizione del GameObject e il clic del mouse
                offset = transform.position - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (transform.position.x < noPosition.x)
            {
                
            }
            else if (transform.position.x > yesPosition.x)
            {
                
            }
            else 
            {
                transform.position = inizialPosition;
            }
            isDragging = false;  // Quando rilasciamo il mouse, smettiamo di trascinare
        }

        if(transform.position.x > inizialPosition.x && isDragging)
        {
            
        }
        else if (transform.position.x < inizialPosition.x)
        {

        }

        if (isDragging)
        {
            // Aggiorna la posizione del GameObject mentre trasciniamo
            Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition) + offset;
            newPosition.z = 0;  // Manteniamo la posizione Z costante in 2D
            transform.position = newPosition;
        }
    }
}
