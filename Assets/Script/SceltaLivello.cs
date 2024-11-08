using UnityEngine;
using UnityEngine.SceneManagement;


public class SceltaLivello : MonoBehaviour
{
    public Color colore;
    public ControlloMappa controlloMappa;
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer spriterenderer = GetComponent<SpriteRenderer> ();
        colore = spriterenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        // Verifica se � stato cliccato il pulsante sinistro del mouse
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
                    Debug.Log(gameObject.name + Utils.AumentaXY(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1]));
                    if (gameObject.name == Utils.AumentaX(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1]))
                    {
                        // casella verde
                        TempData.difficolta = "Domande difficili";
                        string[] stringa = gameObject.name.Split(' ');
                        controlloMappa.gameData.stringValues.Add("Casella " + stringa[1]);
                        controlloMappa.gameData.SaveData();
                        SceneManager.LoadScene("RispostaDomande");
                    }
                    else if (gameObject.name == Utils.AumentaXY(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1]))
                    {
                        // casella rosso
                        TempData.difficolta = "Domande";
                        string[] stringa = gameObject.name.Split(' ');
                        controlloMappa.gameData.stringValues.Add("Casella " + stringa[1]);
                        controlloMappa.gameData.SaveData();
                        SceneManager.LoadScene("RispostaDomande");
                    }
                    else if (gameObject.name == Utils.AumentaY(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1]))
                    {
                        // casella gialla
                        TempData.difficolta = "Domande medie";
                        string[] stringa = gameObject.name.Split(' ');
                        controlloMappa.gameData.stringValues.Add("Casella " + stringa[1]);
                        controlloMappa.gameData.SaveData();
                        SceneManager.LoadScene("RispostaDomande");
                    }
                }
            }
        }
    }
}
