using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpawnDomanda : MonoBehaviour
{
    [SerializeField] private Canvas targetCanvas;
    public RawImage rawImage;

    void Start()
    {
        // SpawnText("Testo dinamico sopra l'oggetto!");
        SpawnImage("fellini1.jpg");
    }

    void SpawnImage(string name)
    {
        // Carica l'immagine dalla cartella Resources/Easy
        Texture2D texture = Resources.Load<Texture2D>("Easy/Fellini1");

        if (texture != null)
        {
            // Imposta la texture sul RawImage
            rawImage.texture = texture;
        }
        else
        {
            Debug.LogError("Immagine non trovata!");
        }
    }

    void SpawnText(string message)
    {
        // Crea un nuovo GameObject per il testo
        GameObject textObject = new GameObject("DynamicTextTMP");

        // Aggiungi il componente TextMeshProUGUI
        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
        textComponent.text = message;
        textComponent.fontSize = 36;
        textComponent.alignment = TextAlignmentOptions.Center;
        textComponent.color = Color.black;


        // Imposta il font base di TextMeshPro
        textComponent.font = TMP_Settings.defaultFontAsset;

        // Imposta il testo come figlio del Canvas
        textObject.transform.SetParent(targetCanvas.transform, false);

        // Configura il RectTransform del testo
        RectTransform textRect = textObject.GetComponent<RectTransform>();

        // Imposta la posizione del testo
        textRect.anchoredPosition = new Vector2(0, 90);
        textRect.sizeDelta = new Vector2(550, 400);
    }
}
