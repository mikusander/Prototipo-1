using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewMovimento : MonoBehaviour
{
    [SerializeField] private GameObject questionCanvas;
    private bool inDestruction = false;
    private ControlNewGameplay control;
    private Camera mainCamera;
    private Vector3 initialPosition;
    private bool isDragging = false;
    private Vector3 offset;
    private Vector3 yesPosition = new Vector3(1.7f, -3.9f, 0f);
    private Vector3 noPosition = new Vector3(-1.7f, -3.9f, 0f);
    [SerializeField] private TextMeshProUGUI yesText;
    [SerializeField] private TextMeshProUGUI noText;

    // Start is called before the first frame update
    void Start()
    {
        control = GameObject.Find("GameController").GetComponent<ControlNewGameplay>();

        initialPosition = transform.position;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                offset = transform.position - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        if (isDragging)
        {
            if (transform.position.x < noPosition.x && !inDestruction && questionCanvas != null)
            {
                isDragging = false;
                inDestruction = true;
                // Rimuovi i componenti dipendenti
                Destroy(questionCanvas.GetComponent<CanvasScaler>());
                Destroy(questionCanvas.GetComponent<GraphicRaycaster>());
                // introdurre la logica prima della distruzione
                Destroy(questionCanvas);
            }
            else if (transform.position.x > yesPosition.x && !inDestruction && questionCanvas != null)
            {
                isDragging = false;
                inDestruction = true;
                // Rimuovi i componenti dipendenti
                Destroy(questionCanvas.GetComponent<CanvasScaler>());
                Destroy(questionCanvas.GetComponent<GraphicRaycaster>());
                // introdurre la logica prima della distruzione
                Destroy(questionCanvas);
            }

            // Update the position of the GameObject as we drag
            Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition) + offset;
            newPosition.z = 0;

            // Calcola la differenza di posizione sull'asse X
            float deltaX = newPosition.x - initialPosition.x;

            if (initialPosition.x < newPosition.x)
            {
                newPosition.y = initialPosition.y - deltaX / 2;
                float distance = yesPosition.x - transform.position.x;
                float opacity = MapValue(distance);

                // Imposta il colore del testo di yesText con l'opacità calcolata
                Color yesTextColor = yesText.color;
                yesTextColor.a = opacity;
                yesText.color = yesTextColor;
            }
            else
            {
                newPosition.y = initialPosition.y + deltaX / 2;
                float distance = transform.position.x - noPosition.x;
                float opacity = MapValue(distance);

                // Imposta il colore del testo di yesText con l'opacità calcolata
                Color noTextColor = noText.color;
                noTextColor.a = opacity;
                noText.color = noTextColor;
            }

            // Imposta una sensibilità per l'inclinazione (più alta = inclinazione più evidente)
            float tiltSensitivity = 10f;

            // Calcola l'angolo di inclinazione basato sullo spostamento sull'asse X
            float tiltAngle = deltaX * tiltSensitivity;

            // Imposta la rotazione del GameObject (inclinazione sull'asse Z per un effetto 2D)
            transform.rotation = Quaternion.Euler(0, 0, -tiltAngle);

            transform.position = newPosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (transform.position.x < noPosition.x)
            {
                inDestruction = true;
                // Rimuovi i componenti dipendenti
                Destroy(questionCanvas.GetComponent<CanvasScaler>());
                Destroy(questionCanvas.GetComponent<GraphicRaycaster>());
                // introdurre la logica prima della distruzione
                Destroy(questionCanvas);
            }
            else if (transform.position.x > yesPosition.x)
            {
                inDestruction = true;
                // Rimuovi i componenti dipendenti
                Destroy(questionCanvas.GetComponent<CanvasScaler>());
                Destroy(questionCanvas.GetComponent<GraphicRaycaster>());
                // introdurre la logica prima della distruzione
                Destroy(questionCanvas);
            }
            else
            {
                // reset the initial position of the gameobject
                transform.position = initialPosition;
                transform.rotation = Quaternion.Euler(0, 0, 0);

                // reset the opacity of the yes text
                Color yesTextColor = yesText.color;
                yesTextColor.a = 0f;
                yesText.color = yesTextColor;

                // rest the opacity of the no text
                Color noTextColor = noText.color;
                noTextColor.a = 0f;
                noText.color = noTextColor;
            }
            isDragging = false;
        }
    }

    float MapValue(float input)
    {
        if (input < 0)
        {
            // Se il valore è negativo, restituisci sempre 1
            return 1f;
        }

        // Mappa l'intervallo [1.7, 0] su [0, 1]
        float result = Mathf.InverseLerp(1.7f, 0f, input);

        return result;
    }


}
