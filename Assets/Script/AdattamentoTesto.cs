using UnityEngine;
using UnityEngine.UI;

public class TextBackground : MonoBehaviour
{
    public Text text; // Riferimento all'oggetto di testo
    public Image background; // Riferimento all'oggetto di sfondo
    public float padding = 10f; // Spazio extra tra il testo e lo sfondo

    void Start()
    {
        UpdateBackgroundSize();
    }

    void Update()
    {
        // Se il testo cambia nel tempo, aggiorna le dimensioni
        UpdateBackgroundSize();
    }

    void UpdateBackgroundSize()
    {
        // Ottieni i RectTransform del testo e dello sfondo
        RectTransform textRect = text.GetComponent<RectTransform>();
        RectTransform backgroundRect = background.GetComponent<RectTransform>();

        // Imposta le dimensioni dello sfondo con padding
        backgroundRect.sizeDelta = new Vector2(textRect.sizeDelta.x + padding * 4, textRect.sizeDelta.y + padding * 2);

        // Allinea lo sfondo con il testo
        backgroundRect.position = textRect.position; // Allinea la posizione del background
    }
}
