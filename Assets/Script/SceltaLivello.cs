using System.Linq;
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
                    if (
                        (gameObject.name == controlloMappa.gameData.traguardo || colore != Color.white)
                        && 
                        casellaSbagliata == null 
                        && 
                        gameObject.name == Utils.Sopra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1])
                    )
                    {
                        // casella sopra di colore giallo, difficoltà due
                        if(gameObject.name == controlloMappa.gameData.traguardo)
                        {
                            if(TempData.casellaCliccata == gameObject.name)
                            {
                                TempData.difficolta = "Domande finali";
                                TempData.ultimaCasella = gameObject.name;
                                SceneManager.LoadScene("RispostaDomande");
                            }
                            else
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(false);
                                controlloMappa.testoDifficoltaDue.SetActive(false);
                                controlloMappa.testoDifficoltaTre.SetActive(false);
                                controlloMappa.testoDifficoltaFinale.SetActive(true);
                                TempData.casellaCliccata = gameObject.name;
                            }
                        }
                        else
                        {
                            if(TempData.casellaCliccata == gameObject.name)
                            {
                                if(controlloMappa.pesi[0] == 3)
                                {
                                    TempData.difficolta = "Domande";
                                }
                                else if(controlloMappa.pesi[0] == 2)
                                {
                                    TempData.difficolta = "Domande medie";
                                }
                                else if(controlloMappa.pesi[0] == 1)
                                {
                                    TempData.difficolta = "Domande difficili";
                                }
                                TempData.ultimaCasella = gameObject.name;
                                SceneManager.LoadScene("RispostaDomande");
                            }
                            else
                            {
                                if(controlloMappa.pesi[0] == 3)
                                {
                                    controlloMappa.scrittaPrincipale.SetActive(false);
                                    controlloMappa.testoDifficoltaUno.SetActive(true);
                                    controlloMappa.testoDifficoltaDue.SetActive(false);
                                    controlloMappa.testoDifficoltaTre.SetActive(false);
                                }
                                else if(controlloMappa.pesi[0] == 2)
                                {
                                    controlloMappa.scrittaPrincipale.SetActive(false);
                                    controlloMappa.testoDifficoltaUno.SetActive(false);
                                    controlloMappa.testoDifficoltaDue.SetActive(true);
                                    controlloMappa.testoDifficoltaTre.SetActive(false);
                                }
                                else if(controlloMappa.pesi[0] == 1)
                                {
                                    controlloMappa.scrittaPrincipale.SetActive(false);
                                    controlloMappa.testoDifficoltaUno.SetActive(false);
                                    controlloMappa.testoDifficoltaDue.SetActive(false);
                                    controlloMappa.testoDifficoltaTre.SetActive(true);
                                }
                                TempData.casellaCliccata = gameObject.name;
                            }
                        }
                    }
                    else if (
                        (gameObject.name == controlloMappa.gameData.traguardo || colore != Color.white) 
                        && 
                        casellaSbagliata == null 
                        && 
                        gameObject.name == Utils.DiagonaleSinistra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1])
                        )
                    {
                        // casella diagonale sinistra di colore rosso
                        if(gameObject.name == controlloMappa.gameData.traguardo)
                        {
                            if(TempData.casellaCliccata == gameObject.name)
                            {
                                TempData.difficolta = "Domande finali";
                                TempData.ultimaCasella = gameObject.name;
                                SceneManager.LoadScene("RispostaDomande");
                            }
                            else
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(false);
                                controlloMappa.testoDifficoltaDue.SetActive(false);
                                controlloMappa.testoDifficoltaTre.SetActive(false);
                                controlloMappa.testoDifficoltaFinale.SetActive(true);
                                TempData.casellaCliccata = gameObject.name;
                            }
                        }
                        else
                        {
                            if (TempData.casellaCliccata == gameObject.name)
                            {
                                if(controlloMappa.pesi[7] == 3)
                                {
                                    TempData.difficolta = "Domande";
                                }
                                else if(controlloMappa.pesi[7] == 2)
                                {
                                    TempData.difficolta = "Domande medie";
                                }
                                else if(controlloMappa.pesi[7] == 1)
                                {
                                    TempData.difficolta = "Domande difficili";
                                }
                                TempData.ultimaCasella = gameObject.name;
                                SceneManager.LoadScene("RispostaDomande");
                            }
                            else
                            {
                                Debug.Log(controlloMappa.pesi.Count());
                                if(controlloMappa.pesi[7] == 3)
                                {
                                    controlloMappa.scrittaPrincipale.SetActive(false);
                                    controlloMappa.testoDifficoltaUno.SetActive(true);
                                    controlloMappa.testoDifficoltaDue.SetActive(false);
                                    controlloMappa.testoDifficoltaTre.SetActive(false);
                                }
                                else if(controlloMappa.pesi[7] == 2)
                                {
                                    controlloMappa.scrittaPrincipale.SetActive(false);
                                    controlloMappa.testoDifficoltaUno.SetActive(false);
                                    controlloMappa.testoDifficoltaDue.SetActive(true);
                                    controlloMappa.testoDifficoltaTre.SetActive(false);
                                }
                                else if(controlloMappa.pesi[7] == 1)
                                {
                                    controlloMappa.scrittaPrincipale.SetActive(false);
                                    controlloMappa.testoDifficoltaUno.SetActive(false);
                                    controlloMappa.testoDifficoltaDue.SetActive(false);
                                    controlloMappa.testoDifficoltaTre.SetActive(true);
                                }
                                TempData.casellaCliccata = gameObject.name;
                            }
                        }
                    }
                    else if (
                        (gameObject.name == controlloMappa.gameData.traguardo || colore != Color.white) 
                        && 
                        casellaSbagliata == null 
                        && 
                        gameObject.name == Utils.Sinistra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1])
                        )
                    {
                        // casella a sinistra di colore verde o giallo, difficoltà 1 o 2
                        if(gameObject.name == controlloMappa.gameData.traguardo)
                        {
                            if(TempData.casellaCliccata == gameObject.name)
                            {
                                TempData.difficolta = "Domande finali";
                                TempData.ultimaCasella = gameObject.name;
                                SceneManager.LoadScene("RispostaDomande");
                            }
                            else
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(false);
                                controlloMappa.testoDifficoltaDue.SetActive(false);
                                controlloMappa.testoDifficoltaTre.SetActive(false);
                                controlloMappa.testoDifficoltaFinale.SetActive(true);
                                TempData.casellaCliccata = gameObject.name;
                            }
                        }
                        else
                        {
                            if (TempData.casellaCliccata == gameObject.name)
                            {
                                if(controlloMappa.pesi[6] == 3)
                                {
                                    TempData.difficolta = "Domande";
                                }
                                else if(controlloMappa.pesi[6] == 2)
                                {
                                    TempData.difficolta = "Domande medie";
                                }
                                else if(controlloMappa.pesi[6] == 1)
                                {
                                    TempData.difficolta = "Domande difficili";
                                }
                                TempData.ultimaCasella = gameObject.name;
                                SceneManager.LoadScene("RispostaDomande");
                            }
                            else
                            {
                                if(controlloMappa.pesi[6] == 3)
                                {
                                    controlloMappa.scrittaPrincipale.SetActive(false);
                                    controlloMappa.testoDifficoltaUno.SetActive(true);
                                    controlloMappa.testoDifficoltaDue.SetActive(false);
                                    controlloMappa.testoDifficoltaTre.SetActive(false);
                                }
                                else if(controlloMappa.pesi[6] == 2)
                                {
                                    controlloMappa.scrittaPrincipale.SetActive(false);
                                    controlloMappa.testoDifficoltaUno.SetActive(false);
                                    controlloMappa.testoDifficoltaDue.SetActive(true);
                                    controlloMappa.testoDifficoltaTre.SetActive(false);
                                }
                                else if(controlloMappa.pesi[6] == 1)
                                {
                                    controlloMappa.scrittaPrincipale.SetActive(false);
                                    controlloMappa.testoDifficoltaUno.SetActive(false);
                                    controlloMappa.testoDifficoltaDue.SetActive(false);
                                    controlloMappa.testoDifficoltaTre.SetActive(true);
                                }
                                TempData.casellaCliccata = gameObject.name;
                            }       
                        }
                    }
                    else if (
                        colore != Color.white 
                        && 
                        casellaSbagliata == null 
                        && 
                        gameObject.name == Utils.DiagonaleDestra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1])
                        )
                    {
                        // casella in diagonale a destra
                        if (TempData.casellaCliccata == gameObject.name)
                        {
                            if(controlloMappa.pesi[1] == 3)
                            {
                                TempData.difficolta = "Domande";
                            }
                            else if(controlloMappa.pesi[1] == 2)
                            {
                                TempData.difficolta = "Domande medie";
                            }
                            else if(controlloMappa.pesi[1] == 1)
                            {
                                TempData.difficolta = "Domande difficili";
                            }
                            TempData.ultimaCasella = gameObject.name;
                            SceneManager.LoadScene("RispostaDomande");
                        }
                        else
                        {
                            if(controlloMappa.pesi[1] == 3)
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(true);
                                controlloMappa.testoDifficoltaDue.SetActive(false);
                                controlloMappa.testoDifficoltaTre.SetActive(false);
                            }
                            else if(controlloMappa.pesi[1] == 2)
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(false);
                                controlloMappa.testoDifficoltaDue.SetActive(true);
                                controlloMappa.testoDifficoltaTre.SetActive(false);
                            }
                            else if(controlloMappa.pesi[1] == 1)
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(false);
                                controlloMappa.testoDifficoltaDue.SetActive(false);
                                controlloMappa.testoDifficoltaTre.SetActive(true);
                            }
                            TempData.casellaCliccata = gameObject.name;
                        }
                    }
                    else if (colore != Color.white && casellaSbagliata == null && gameObject.name == Utils.Destra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1]))
                    {
                        // casella a destra di colore o verde o giallo, di difficoltà 1 o 2
                        if (TempData.casellaCliccata == gameObject.name)
                        {
                            if(controlloMappa.pesi[2] == 3)
                            {
                                TempData.difficolta = "Domande";
                            }
                            else if(controlloMappa.pesi[2] == 2)
                            {
                                TempData.difficolta = "Domande medie";
                            }
                            else if(controlloMappa.pesi[2] == 1)
                            {
                                TempData.difficolta = "Domande difficili";
                            }
                        }
                        else
                        {
                            if(controlloMappa.pesi[2] == 3)
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(true);
                                controlloMappa.testoDifficoltaDue.SetActive(false);
                                controlloMappa.testoDifficoltaTre.SetActive(false);
                            }
                            else if(controlloMappa.pesi[2] == 2)
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(false);
                                controlloMappa.testoDifficoltaDue.SetActive(true);
                                controlloMappa.testoDifficoltaTre.SetActive(false);
                            }
                            else if(controlloMappa.pesi[2] == 1)
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(false);
                                controlloMappa.testoDifficoltaDue.SetActive(false);
                                controlloMappa.testoDifficoltaTre.SetActive(true);
                            }
                            TempData.casellaCliccata = gameObject.name;
                        }
                    }
                    else if (
                        colore != Color.white 
                        && 
                        casellaSbagliata == null 
                        && 
                        gameObject.name == Utils.Sotto(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1])
                        )
                    {
                        // casella sotto di colore verde, difficoltà 1
                        if (TempData.casellaCliccata == gameObject.name)
                        {
                            if(controlloMappa.pesi[4] == 3)
                            {
                                TempData.difficolta = "Domande";
                            }
                            else if(controlloMappa.pesi[4] == 2)
                            {
                                TempData.difficolta = "Domande medie";
                            }
                            else if(controlloMappa.pesi[4] == 1)
                            {
                                TempData.difficolta = "Domande difficili";
                            }
                            TempData.ultimaCasella = gameObject.name;
                            SceneManager.LoadScene("RispostaDomande");
                        }
                        else
                        {
                            if(controlloMappa.pesi[4] == 3)
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(true);
                                controlloMappa.testoDifficoltaDue.SetActive(false);
                                controlloMappa.testoDifficoltaTre.SetActive(false);
                            }
                            else if(controlloMappa.pesi[4] == 2)
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(false);
                                controlloMappa.testoDifficoltaDue.SetActive(true);
                                controlloMappa.testoDifficoltaTre.SetActive(false);
                            }
                            else if(controlloMappa.pesi[4] == 1)
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(false);
                                controlloMappa.testoDifficoltaDue.SetActive(false);
                                controlloMappa.testoDifficoltaTre.SetActive(true);
                            }
                            TempData.casellaCliccata = gameObject.name;
                        }
                    }
                    else if (
                        colore != Color.white 
                        && 
                        casellaSbagliata == null 
                        && 
                        gameObject.name == Utils.DiagonaleSottoDestra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1])
                        )
                    {
                        // casella in diagonale inferiore a destra di colore giallo, difficoltà di livello 2
                        if (TempData.casellaCliccata == gameObject.name)
                        {
                            if(controlloMappa.pesi[3] == 3)
                            {
                                TempData.difficolta = "Domande";
                            }
                            else if(controlloMappa.pesi[3] == 2)
                            {
                                TempData.difficolta = "Domande medie";
                            }
                            else if(controlloMappa.pesi[3] == 1)
                            {
                                TempData.difficolta = "Domande difficili";
                            }
                            TempData.ultimaCasella = gameObject.name;
                            SceneManager.LoadScene("RispostaDomande");
                        }
                        else
                        {
                            if(controlloMappa.pesi[3] == 3)
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(true);
                                controlloMappa.testoDifficoltaDue.SetActive(false);
                                controlloMappa.testoDifficoltaTre.SetActive(false);
                            }
                            else if(controlloMappa.pesi[3] == 2)
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(false);
                                controlloMappa.testoDifficoltaDue.SetActive(true);
                                controlloMappa.testoDifficoltaTre.SetActive(false);
                            }
                            else if(controlloMappa.pesi[3] == 1)
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(false);
                                controlloMappa.testoDifficoltaDue.SetActive(false);
                                controlloMappa.testoDifficoltaTre.SetActive(true);
                            }
                            TempData.casellaCliccata = gameObject.name;
                        }
                    }
                    else if (
                        colore != Color.white 
                        && 
                        casellaSbagliata == null 
                        && 
                        gameObject.name == Utils.DiagonaleSottoSinistra(controlloMappa.gameData.stringValues[controlloMappa.gameData.stringValues.Count - 1])
                        )
                    {
                        // casella in diagonale inferiore a sinistra di colore giallo, difficolta 2
                        if (TempData.casellaCliccata == gameObject.name)
                        {
                            if(controlloMappa.pesi[5] == 3)
                            {
                                TempData.difficolta = "Domande";
                            }
                            else if(controlloMappa.pesi[5] == 2)
                            {
                                TempData.difficolta = "Domande medie";
                            }
                            else if(controlloMappa.pesi[5] == 1)
                            {
                                TempData.difficolta = "Domande difficili";
                            }
                            TempData.ultimaCasella = gameObject.name;
                            SceneManager.LoadScene("RispostaDomande");
                        }
                        else
                        {
                            if(controlloMappa.pesi[5] == 3)
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(true);
                                controlloMappa.testoDifficoltaDue.SetActive(false);
                                controlloMappa.testoDifficoltaTre.SetActive(false);
                            }
                            else if(controlloMappa.pesi[5] == 2)
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(false);
                                controlloMappa.testoDifficoltaDue.SetActive(true);
                                controlloMappa.testoDifficoltaTre.SetActive(false);
                            }
                            else if(controlloMappa.pesi[5] == 1)
                            {
                                controlloMappa.scrittaPrincipale.SetActive(false);
                                controlloMappa.testoDifficoltaUno.SetActive(false);
                                controlloMappa.testoDifficoltaDue.SetActive(false);
                                controlloMappa.testoDifficoltaTre.SetActive(true);
                            }
                            TempData.casellaCliccata = gameObject.name;
                        }
                    }
                }
            }
        }
    }
}
