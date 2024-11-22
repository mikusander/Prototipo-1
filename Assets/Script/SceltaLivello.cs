using UnityEngine;
using UnityEngine.SceneManagement;


public class SceltaLivello : MonoBehaviour
{
    public Color colore;
    public ControlloMappa controlloMappa;
    // Start is called before the first frame update
    void Start()
    {
        controlloMappa = GameObject.Find("GameController").GetComponent<ControlloMappa>();
        SpriteRenderer spriterenderer = GetComponent<SpriteRenderer> ();
        colore = spriterenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        // Verifica se è stato cliccato il pulsante sinistro del mouse
        if (Input.GetMouseButtonDown(0))  // 0 rappresenta il pulsante sinistro del mouse
        {
            // Ottieni la posizione del mouse in coordinate di mondo
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Lancia un raycast 2D dalla posizione del mouse
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // Verifica se il raycast ha colpito qualcosa
            if (hit.collider != null)
            {
                // Controlla se il GameObject colpito è lo stesso su cui è attaccato questo script
                if (hit.collider.gameObject == gameObject)
                {
                    GameObject casellaSbagliata = GameObject.Find("Errore " + gameObject.name);
                    if (casellaSbagliata == null && gameObject.name == Utils.Sopra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1]))
                    {
                        // casella sopra di colore giallo
                        TempData.difficolta = "Domande difficili";
                        TempData.ultimaCasella = gameObject.name;
                        SceneManager.LoadScene("RispostaDomande");
                    }
                    else if (casellaSbagliata == null && gameObject.name == Utils.DiagonaleSinistra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1]))
                    {
                        // casella diagonale sinistra di colore rosso
                        TempData.difficolta = "Domande";
                        TempData.ultimaCasella = gameObject.name;
                        SceneManager.LoadScene("RispostaDomande");
                    }
                    else if (casellaSbagliata == null && gameObject.name == Utils.Sinistra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1]))
                    {
                        // casella a sinistra di colore verde
                        TempData.difficolta = "Domande medie";
                        TempData.ultimaCasella = gameObject.name;
                        SceneManager.LoadScene("RispostaDomande");
                    }
                    else if (casellaSbagliata == null && gameObject.name == Utils.DiagonaleDestra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1]))
                    {
                        // casella in diagonale a destra
                        TempData.difficolta = "Domande medie";
                        TempData.ultimaCasella = gameObject.name;
                        SceneManager.LoadScene("RispostaDomande");
                    }
                    else if (casellaSbagliata == null && gameObject.name == Utils.Destra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1]))
                    {
                        // casella a destra
                        TempData.difficolta = "Domande medie";
                        TempData.ultimaCasella = gameObject.name;
                        SceneManager.LoadScene("RispostaDomande");
                    }
                }
            }
        }
    }
}
