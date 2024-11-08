using System.Collections;
using System.Collections.Generic;
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
                    if (colore.r == 0f && colore.g == 1f && colore.b == 0f && colore.a == 1f)
                    {
                        // casella verde
                        TempData.difficolta = "Domande difficili";
                        string[] stringa = gameObject.name.Split(' ');
                        controlloMappa.gameData.stringValues.Add("Casella " + stringa[1]);
                        controlloMappa.gameData.SaveData();
                        SceneManager.LoadScene("RispostaDomande");
                    }
                    else if (colore.r == 0f && colore.g == 0f && colore.b == 0f && colore.a == 1f)
                    {
                        // casella rosso
                        TempData.difficolta = "Domande";
                        string[] stringa = gameObject.name.Split(' ');
                        controlloMappa.gameData.stringValues.Add("Casella " + stringa[1]);
                        controlloMappa.gameData.SaveData();
                        SceneManager.LoadScene("RispostaDomande");
                    }
                    else if (colore.r == 255f && colore.g == 255f && colore.b == 0f && colore.a == 255f)
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
