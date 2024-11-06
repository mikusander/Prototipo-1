using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomBackground : MonoBehaviour
{
    // Array di Sprite che contiene tutte le immagini di sfondo
    public Sprite[] backgrounds;

    // Riferimento al componente SpriteRenderer
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Ottieni il componente SpriteRenderer del GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Carica un'immagine di sfondo casuale
        SetRandomBackground();

        // Adatta l'immagine selezionata alla dimensione della camera
        FitToCamera();
    }

    void SetRandomBackground()
    {
        // Controlla che l'array contenga almeno un'immagine
        if (backgrounds.Length == 0)
        {
            Debug.LogWarning("Non ci sono immagini di sfondo nel'array 'backgrounds'.");
            return;
        }

        // Seleziona un'immagine casuale dall'array
        int randomIndex = Random.Range(0, backgrounds.Length);
        spriteRenderer.sprite = backgrounds[randomIndex];
    }

    void FitToCamera()
    {
        // Ottieni la larghezza e l'altezza del campo visivo della camera
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        // Calcola la scala necessaria per adattare l'immagine alla camera
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;

        float scaleX = cameraWidth / spriteWidth;
        float scaleY = cameraHeight / spriteHeight;

        // Imposta la scala del GameObject per adattare lo sprite
        transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
}
