using UnityEngine;
using UnityEngine.SceneManagement;


public class SceltaLivello : MonoBehaviour
{
    private Color colore;
    public ControlloMappa controlloMappa;
    // Start is called before the first frame update
    void Start()
    {
        controlloMappa = GameObject.Find("GameController").GetComponent<ControlloMappa>();
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
                    SpriteRenderer spriterenderer = GetComponent<SpriteRenderer> ();
                    colore = spriterenderer.color;
                    GameObject casellaSbagliata = GameObject.Find("Errore " + gameObject.name);
                    if (colore != Color.white && casellaSbagliata == null && gameObject.name == Utils.Sopra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1]))
                    {
                        // casella sopra di colore giallo, difficoltà due
                        if(TempData.casellaCliccata == gameObject.name)
                        {
                            TempData.difficolta = "Domande medie";
                            TempData.ultimaCasella = gameObject.name;
                            SceneManager.LoadScene("RispostaDomande");
                        }
                        else
                        {
                            controlloMappa.scrittaPrincipale.SetActive(false);
                            controlloMappa.testoDifficoltaUno.SetActive(false);
                            controlloMappa.testoDifficoltaDue.SetActive(true);
                            controlloMappa.testoDifficoltaTre.SetActive(false);
                            TempData.casellaCliccata = gameObject.name;
                        }
                    }
                    else if (colore != Color.white && casellaSbagliata == null && gameObject.name == Utils.DiagonaleSinistra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1]))
                    {
                        // casella diagonale sinistra di colore rosso
                        if (TempData.casellaCliccata == gameObject.name)
                        {
                            TempData.difficolta = "Domande difficili";
                            TempData.ultimaCasella = gameObject.name;
                            SceneManager.LoadScene("RispostaDomande");
                        }
                        else
                        {
                            controlloMappa.scrittaPrincipale.SetActive(false);
                            controlloMappa.testoDifficoltaUno.SetActive(false);
                            controlloMappa.testoDifficoltaDue.SetActive(false);
                            controlloMappa.testoDifficoltaTre.SetActive(true);
                            TempData.casellaCliccata = gameObject.name;
                        }
                    }
                    else if (colore != Color.white && casellaSbagliata == null && gameObject.name == Utils.Sinistra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1]))
                    {
                        // casella a sinistra di colore verde o giallo, difficoltà 1 o 2
                        if (TempData.casellaCliccata == gameObject.name)
                        {
                            if (controlloMappa.valoreCasuale)
                            {
                                TempData.difficolta = "Domande medie";
                                TempData.ultimaCasella = gameObject.name;
                                SceneManager.LoadScene("RispostaDomande");
                            }
                            else
                            {
                                TempData.difficolta = "Domande";
                                TempData.ultimaCasella = gameObject.name;
                                SceneManager.LoadScene("RispostaDomande");
                            }
                        }
                        else
                        {
                            if (controlloMappa.valoreCasuale)
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(false);
                                controlloMappa.testoDifficoltaDue.SetActive(true);
                                controlloMappa.testoDifficoltaTre.SetActive(false);
                                TempData.casellaCliccata = gameObject.name;
                            }
                            else
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(true);
                                controlloMappa.testoDifficoltaDue.SetActive(false);
                                controlloMappa.testoDifficoltaTre.SetActive(false);
                                TempData.casellaCliccata = gameObject.name;
                            }
                        }
                    }
                    else if (colore != Color.white && casellaSbagliata == null && gameObject.name == Utils.DiagonaleDestra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1]))
                    {
                        // casella in diagonale a destra
                        if (TempData.casellaCliccata == gameObject.name)
                        {
                            TempData.difficolta = "Domande difficili";
                            TempData.ultimaCasella = gameObject.name;
                            SceneManager.LoadScene("RispostaDomande");
                        }
                        else
                        {
                            controlloMappa.scrittaPrincipale.SetActive(false);
                            controlloMappa.testoDifficoltaUno.SetActive(false);
                            controlloMappa.testoDifficoltaDue.SetActive(false);
                            controlloMappa.testoDifficoltaTre.SetActive(true);
                            TempData.casellaCliccata = gameObject.name;
                        }
                    }
                    else if (colore != Color.white && casellaSbagliata == null && gameObject.name == Utils.Destra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1]))
                    {
                        // casella a destra di colore o verde o giallo, di difficoltà 1 o 2
                        if (TempData.casellaCliccata == gameObject.name)
                        {
                            if (controlloMappa.valoreCasuale)
                            {
                                TempData.difficolta = "Domande";
                                TempData.ultimaCasella = gameObject.name;
                                SceneManager.LoadScene("RispostaDomande");
                            }
                            else
                            {
                                TempData.difficolta = "Domande medie";
                                TempData.ultimaCasella = gameObject.name;
                                SceneManager.LoadScene("RispostaDomande");
                            }
                        }
                        else
                        {
                            if (controlloMappa.valoreCasuale)
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(true);
                                controlloMappa.testoDifficoltaDue.SetActive(false);
                                controlloMappa.testoDifficoltaTre.SetActive(false);
                                TempData.casellaCliccata = gameObject.name;
                            }
                            else
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(false);
                                controlloMappa.testoDifficoltaDue.SetActive(true);
                                controlloMappa.testoDifficoltaTre.SetActive(false);
                                TempData.casellaCliccata = gameObject.name;
                            }
                        }
                    }
                    else if (colore != Color.white && casellaSbagliata == null && gameObject.name == Utils.Sotto(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1]))
                    {
                        // casella sotto di colore verde, difficoltà 1
                        if (TempData.casellaCliccata == gameObject.name)
                        {
                            TempData.difficolta = "Domande";
                            TempData.ultimaCasella = gameObject.name;
                            SceneManager.LoadScene("RispostaDomande");
                        }
                        else
                        {
                            controlloMappa.scrittaPrincipale.SetActive(false);
                            controlloMappa.testoDifficoltaUno.SetActive(true);
                            controlloMappa.testoDifficoltaDue.SetActive(false);
                            controlloMappa.testoDifficoltaTre.SetActive(false);
                            TempData.casellaCliccata = gameObject.name;
                        }
                    }
                    else if (colore != Color.white && casellaSbagliata == null && gameObject.name == Utils.DiagonaleSottoDestra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1]))
                    {
                        // casella in diagonale inferiore a destra di colore giallo, difficoltà di livello 2
                        if (TempData.casellaCliccata == gameObject.name)
                        {
                            TempData.difficolta = "Domande medie";
                            TempData.ultimaCasella = gameObject.name;
                            SceneManager.LoadScene("RispostaDomande");
                        }
                        else
                        {
                            controlloMappa.scrittaPrincipale.SetActive(false);
                            controlloMappa.testoDifficoltaUno.SetActive(false);
                            controlloMappa.testoDifficoltaDue.SetActive(true);
                            controlloMappa.testoDifficoltaTre.SetActive(false);
                            TempData.casellaCliccata = gameObject.name;
                        }
                    }
                    else if (colore != Color.white && casellaSbagliata == null && gameObject.name == Utils.DiagonaleSottoSinistra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1]))
                    {
                        // casella in diagonale inferiore a sinistra di colore giallo, difficolta 2
                        if (TempData.casellaCliccata == gameObject.name)
                        {
                            TempData.difficolta = "Domande medie";
                            TempData.ultimaCasella = gameObject.name;
                            SceneManager.LoadScene("RispostaDomande");
                        }
                        else
                        {
                            controlloMappa.scrittaPrincipale.SetActive(false);
                            controlloMappa.testoDifficoltaUno.SetActive(false);
                            controlloMappa.testoDifficoltaDue.SetActive(true);
                            controlloMappa.testoDifficoltaTre.SetActive(false);
                            TempData.casellaCliccata = gameObject.name;
                        }
                    }
                }
            }
        }
    }
}
