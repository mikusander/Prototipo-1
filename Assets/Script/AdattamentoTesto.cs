using UnityEngine;
using UnityEngine.UI;

public class TextBackground : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Image background;
    private float padding = 10f;

    void Start()
    {
        UpdateBackgroundSize();
    }

    void Update()
    {
        // If the text changes over time, update the size
        UpdateBackgroundSize();
    }

    void UpdateBackgroundSize()
    {
        // Get the RectTransforms of the text and background
        RectTransform textRect = text.GetComponent<RectTransform>();
        RectTransform backgroundRect = background.GetComponent<RectTransform>();

        // Set the size of the padding background
        backgroundRect.sizeDelta = new Vector2(textRect.sizeDelta.x + padding * 4, textRect.sizeDelta.y + padding * 2);

        // Align the background with the text
        backgroundRect.position = textRect.position;
    }
}
