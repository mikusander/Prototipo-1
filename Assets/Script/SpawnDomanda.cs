using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;

public class SpawnDomanda : MonoBehaviour
{
    [SerializeField] private Canvas targetCanvas;
    [SerializeField] private GameObject textArea;
    [SerializeField] private GameObject rawImageGameObject;
    [SerializeField] private GameObject rawImageVideoGameObject;
    [SerializeField] private ControlNewGameplay control;


    void Start()
    {
        if (control.currentQuestion.Tipo == "Testo")
        {
            SpawnText(control.currentQuestion.Contenuto);
        }
        else if (control.currentQuestion.Tipo == "Foto")
        {
            SpawnImage(Path.Combine(TempData.difficolta, control.currentQuestion.Contenuto));
        }
        else if (control.currentQuestion.Tipo == "Video")
        {
            SpawnVideo(Path.Combine(TempData.difficolta, control.currentQuestion.Contenuto));
        }
    }

    void SpawnVideo(string videoPath)
    {
        rawImageVideoGameObject.SetActive(true);
        RawImage rawImageVideo = rawImageVideoGameObject.GetComponent<RawImage>();
        VideoPlayer videoPlayer = rawImageVideoGameObject.GetComponent<VideoPlayer>();

        // Aggiungi un VideoPlayer dinamicamente al GameObject
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        // Configura il VideoPlayer
        videoPlayer.playOnAwake = false; // Non avviare automaticamente
        videoPlayer.isLooping = true; // Opzionale: attiva loop
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;

        // Carica il video dalla cartella Resources
        VideoClip videoClip = Resources.Load<VideoClip>(videoPath);
        if (videoClip == null)
        {
            Debug.LogError("Video non trovato in Resources: " + videoPath);
            return;
        }
        videoPlayer.clip = videoClip;

        // Crea una RenderTexture e assegnala al VideoPlayer
        RenderTexture renderTexture = new RenderTexture(1920, 1080, 0); // Regola le dimensioni come necessario
        videoPlayer.targetTexture = renderTexture;

        // Assegna la RenderTexture alla componente RawImage
        rawImageVideo.texture = renderTexture;

        // Avvia la riproduzione
        videoPlayer.Play();
    }

    void SpawnImage(string name)
    {
        rawImageGameObject.SetActive(true);
        RawImage rawImage = rawImageGameObject.GetComponent<RawImage>();

        // Carica l'immagine dalla cartella Resources/Easy
        Texture2D texture = Resources.Load<Texture2D>("Easy/Fellini2");

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
        textArea.SetActive(true);

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
