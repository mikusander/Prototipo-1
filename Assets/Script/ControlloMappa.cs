using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ControlloMappa : MonoBehaviour
{

    public GameObject theEnd;
    public GameObject gameoverLogo;
    public GameObject restart;
    public GameObject player;
    public GameObject errore;
    public GameObject baseScacchiera;
    public GameData gameData;
    public float moveDuration = 2f;
    public GameObject bottoneIniziale;
    public GameObject scrittaIniziale;
    public GameObject scrittaPrincipale;
    public GameObject difficoltaUno;
    public GameObject difficoltaDue;
    public GameObject difficoltaTre;
    public GameObject testoDifficoltaUno;
    public GameObject testoDifficoltaDue;
    public GameObject testoDifficoltaTre;
    public GameObject testoDifficoltaFinale;
    public GameObject bandieraTraguardo;
    public GameObject logoTraguardo;
    public bool valoreCasuale;

    // Start is called before the first frame update
    void Start()
    {
        // Carica i dati salvati (se esistono)
        gameData = GetComponent<GameData>();
        if (gameData != null)
        {
            gameData.LoadData();
        }
        if (gameData.stringValues.Count > 0)
        {
            baseScacchiera.SetActive(true);
        }
        bool nuovoErrore = false;
        if(gameData.caselleSbagliate.Count > 0 && TempData.ultimoErrore != gameData.caselleSbagliate[gameData.caselleSbagliate.Count - 1])
        {
            nuovoErrore = true;
        }
        foreach (string i in gameData.caselleSbagliate)
        {
            Vector3 casellaSbagliataPosition = GameObject.Find(i).gameObject.transform.position;
            GameObject casella = Instantiate(errore, casellaSbagliataPosition, Quaternion.identity);
            if (TempData.gioco && !TempData.vittoria && nuovoErrore && gameData.caselleSbagliate[gameData.caselleSbagliate.Count - 1] == i)
            {
                Animator animatorErr = casella.GetComponent<Animator>();
                StartCoroutine(ErrorAnimation(animatorErr));

                nuovoErrore = false;
            }
            casella.name = "Errore " + i;
        }
        if(gameData.caselleSbagliate.Count > 1)
        {
            TempData.ultimoErrore = "Errore " + gameData.caselleSbagliate[gameData.caselleSbagliate.Count - 1];
        }
        for (int i = 0; i < gameData.stringValues.Count; i++)
        {
            SpriteRenderer colore = GameObject.Find(gameData.stringValues[i]).GetComponent<SpriteRenderer>();
            colore.color = Color.white;
        }

        if (gameData.stringValues.Count == 0)
        {
            bottoneIniziale.SetActive(true);
            scrittaIniziale.SetActive(true);
        }
        else if (gameData.stringValues.Count == 1)
        {
            scrittaPrincipale.SetActive(true);
            Transform casellaTransform = baseScacchiera.transform.Find("Casella 0,0");
            if ( casellaTransform != null )
            {
                GameObject casella = casellaTransform.gameObject;
                // Cambia colore usando il componente Renderer
                SpriteRenderer renderer = casella.GetComponent<SpriteRenderer>();

                // Verifica che il GameObject abbia un Renderer
                if (renderer != null)
                {
                    // Modifica il colore del materiale
                    renderer.color = Color.white;
                }
                GameObject casellaSopraGameObject = GameObject.Find(Utils.Sopra(casella.name));
                GameObject casellaSbagliataSopra = GameObject.Find("Errore " + Utils.Sopra(casella.name));
                if (casellaSopraGameObject != null && casellaSbagliataSopra == null)
                {
                    SpriteRenderer casellaSopra = casellaSopraGameObject.GetComponent<SpriteRenderer>();
                    casellaSopra.color = new Color(255f, 255f, 0f, 255f);
                    Vector3 spawnPos = casellaSopraGameObject.transform.position;
                    Instantiate(difficoltaDue, spawnPos, Quaternion.identity);
                }
                GameObject casellaDiagonaleGameObject = GameObject.Find(Utils.DiagonaleSinistra(casella.name));
                GameObject casellaSbagliataDiagonale = GameObject.Find("Errore " + Utils.DiagonaleSinistra(casella.name));
                if (casellaDiagonaleGameObject != null && casellaSbagliataDiagonale == null)
                {
                    SpriteRenderer casellaDiagonale = casellaDiagonaleGameObject.GetComponent<SpriteRenderer>();
                    casellaDiagonale.color = Color.red;
                    Vector3 spawnPos = casellaDiagonaleGameObject.transform.position;
                    Instantiate(difficoltaTre, spawnPos, Quaternion.identity);
                }
                GameObject casellaSinistraGameObject = GameObject.Find(Utils.Sinistra(casella.name));
                GameObject casellaSbagliataSinistra = GameObject.Find("Errore " + Utils.Sinistra(casella.name));
                if (casellaSinistraGameObject != null && casellaSbagliataSinistra == null)
                {
                    SpriteRenderer casellaSinistra = casellaSinistraGameObject.GetComponent<SpriteRenderer>();
                    casellaSinistra.color = Color.green;
                    Vector3 spawnPos = casellaSinistraGameObject.transform.position;
                    Instantiate(difficoltaUno, spawnPos, Quaternion.identity);
                }
                Vector3 spawnPosition = casella.transform.position;
                player = Instantiate(player, spawnPosition, Quaternion.identity);
                if (casellaSbagliataSopra != null && casellaSbagliataDiagonale != null && casellaSbagliataSinistra != null)
                {
                    gameoverLogo.SetActive(true);
                    restart.SetActive(true);
                }
            }
        }
        else if (gameData.stringValues[gameData.stringValues.Count - 1] == "Casella 5,3")
        {
            theEnd.SetActive(true);
            restart.SetActive(true);
        }
        else
        {
            scrittaPrincipale.SetActive(true);
            if(TempData.gioco && TempData.vittoria)
            {
                string ultimaCasellaString = gameData.stringValues[gameData.stringValues.Count - 1];
                string penultimaCasellaString = gameData.stringValues[gameData.stringValues.Count - 2];
                Transform ultimaCasellaTransform = baseScacchiera.transform.Find(ultimaCasellaString);
                Transform penultimaCasellaTransform = baseScacchiera.transform.Find(penultimaCasellaString);
                if (ultimaCasellaTransform != null && penultimaCasellaTransform != null)
                {
                    GameObject ultimaCasella = ultimaCasellaTransform.gameObject;
                    GameObject penultimaCasella = penultimaCasellaTransform.gameObject;
                    Vector3 spawnPosition = penultimaCasella.transform.position;
                    player = Instantiate(player, spawnPosition, Quaternion.identity);
                    StartCoroutine(MoveToTarget(spawnPosition, ultimaCasella.transform.position, moveDuration));
                }
                GameObject casellaSopraGameObject = GameObject.Find(Utils.Sopra(ultimaCasellaString));
                GameObject casellaSbagliataSopra = GameObject.Find("Errore " + Utils.Sopra(ultimaCasellaString));
                if (casellaSopraGameObject != null && casellaSbagliataSopra == null)
                {
                    SpriteRenderer casellaSopra = casellaSopraGameObject.GetComponent<SpriteRenderer>();
                    if(casellaSopraGameObject.name == "Casella 5,3")
                    {
                        bandieraTraguardo.SetActive(false);
                        logoTraguardo.SetActive(true);
                        casellaSopra.color = Color.white;
                    }
                    else if(casellaSopra.color != Color.white)
                    {
                        casellaSopra.color = new Color(255f, 255f, 0f, 255f);
                        Vector3 spawnPos = casellaSopraGameObject.transform.position;
                        Instantiate(difficoltaDue, spawnPos, Quaternion.identity);
                    }
                }
                GameObject casellaDiagonaleGameObject = GameObject.Find(Utils.DiagonaleSinistra(ultimaCasellaString));
                GameObject casellaSbagliataDiagonale = GameObject.Find("Errore " + Utils.DiagonaleSinistra(ultimaCasellaString));
                if (casellaDiagonaleGameObject != null && casellaSbagliataDiagonale == null)
                {
                    SpriteRenderer casellaDiagonale = casellaDiagonaleGameObject.GetComponent<SpriteRenderer>();
                    if(casellaDiagonaleGameObject.name == "Casella 5,3")
                    {
                        bandieraTraguardo.SetActive(false);
                        logoTraguardo.SetActive(true);
                        casellaDiagonale.color = Color.white;
                    }
                    else if(casellaDiagonale.color != Color.white && casellaSopraGameObject.name != "Casella 5,3")
                    {
                        casellaDiagonale.color = Color.red;
                        Vector3 spawnPos = casellaDiagonaleGameObject.transform.position;
                        Instantiate(difficoltaTre, spawnPos, Quaternion.identity);
                    }
                }
                GameObject casellaSinistraGameObject = GameObject.Find(Utils.Sinistra(ultimaCasellaString));
                GameObject casellaSbagliataSinistra = GameObject.Find("Errore " + Utils.Sinistra(ultimaCasellaString));
                valoreCasuale = Random.Range(0, 2) == 0; 
                if (casellaSinistraGameObject != null && casellaSbagliataSinistra == null)
                {
                    SpriteRenderer casellaSinistra = casellaSinistraGameObject.GetComponent<SpriteRenderer>();
                    if(casellaSinistraGameObject.name == "Casella 5,3")
                    {
                        bandieraTraguardo.SetActive(false);
                        logoTraguardo.SetActive(true);
                        casellaSinistra.color = Color.white;
                    }
                    else if(valoreCasuale)
                    {
                        if(casellaSinistra.color != Color.white && casellaSinistraGameObject.name != "Casella 5,3")
                        {
                            casellaSinistra.color = Color.green;
                            Vector3 spawnPos = casellaSinistraGameObject.transform.position;
                            Instantiate(difficoltaUno, spawnPos, Quaternion.identity);
                        }
                    }
                    else
                    {
                        if(casellaSinistra.color != Color.white && casellaSinistraGameObject.name != "Casella 5,3")
                        {
                            casellaSinistra.color = new Color(255f, 255f, 0f, 255f);
                            Vector3 spawnPos = casellaSinistraGameObject.transform.position;
                            Instantiate(difficoltaDue, spawnPos, Quaternion.identity);
                        }
                    }
                }
                GameObject casellaDiagonaleDestraGameObject = GameObject.Find(Utils.DiagonaleDestra(ultimaCasellaString));
                GameObject casellaSbagliataDiagonaleDestra = GameObject.Find("Errore " + Utils.DiagonaleDestra(ultimaCasellaString));
                if(casellaDiagonaleDestraGameObject != null && casellaSbagliataDiagonaleDestra == null)
                {
                    SpriteRenderer casellaDiagonaleDestra = casellaDiagonaleDestraGameObject.GetComponent<SpriteRenderer>();
                    if(casellaDiagonaleDestra.color != Color.white)
                    {
                        casellaDiagonaleDestra.color = Color.red;
                        Vector3 spawnPos = casellaDiagonaleDestraGameObject.transform.position;
                        Instantiate(difficoltaTre, spawnPos, Quaternion.identity);
                    }
                }
                GameObject casellaDestraGameObject = GameObject.Find(Utils.Destra(ultimaCasellaString));
                GameObject casellaSbagliataDestra = GameObject.Find("Errore " + Utils.Destra(ultimaCasellaString));
                if(casellaDestraGameObject != null && casellaSbagliataDestra == null)
                {
                    SpriteRenderer casellaDestra = casellaDestraGameObject.GetComponent<SpriteRenderer>();
                    if(valoreCasuale)
                    {
                        if(casellaDestra.color != Color.white)
                        {
                            casellaDestra.color = new Color(255f, 255f, 0f, 255f);
                            Vector3 spawnPos = casellaDestraGameObject.transform.position;
                            Instantiate(difficoltaDue, spawnPos, Quaternion.identity);
                        }
                    }
                    else
                    {
                        if(casellaDestra.color != Color.white)
                        {
                            casellaDestra.color = Color.green;
                            Vector3 spawnPos = casellaDestraGameObject.transform.position;
                            Instantiate(difficoltaUno, spawnPos, Quaternion.identity);
                        }
                    }
                }
                GameObject casellaDiagonaleSottoDestraGameObject = GameObject.Find(Utils.DiagonaleSottoDestra(ultimaCasellaString));
                GameObject casellaSbagliataDiagonaleSottoDestra = GameObject.Find("Errore " + Utils.DiagonaleSottoDestra(ultimaCasellaString));
                if(casellaDiagonaleSottoDestraGameObject != null && casellaSbagliataDiagonaleSottoDestra == null)
                {
                    SpriteRenderer casellaDiagonaleSottoDestra = casellaDiagonaleSottoDestraGameObject.GetComponent<SpriteRenderer>();
                    if(casellaDiagonaleSottoDestra.color != Color.white)
                    {
                        casellaDiagonaleSottoDestra.color = new Color(255f, 255f, 0f, 255f);
                        Vector3 spawnPos = casellaDiagonaleSottoDestraGameObject.transform.position;
                        Instantiate(difficoltaDue, spawnPos, Quaternion.identity);
                    }
                }
                GameObject casellaDiagonaleSottoSinistraGameObject = GameObject.Find(Utils.DiagonaleSottoSinistra(ultimaCasellaString));
                GameObject casellaSbagliataDiagonaleSottoSinistra = GameObject.Find("Errore " + Utils.DiagonaleSottoSinistra(ultimaCasellaString));
                if(casellaDiagonaleSottoSinistraGameObject != null && casellaSbagliataDiagonaleSottoSinistra == null)
                {
                    SpriteRenderer casellaDiagonaleSottoSinistra = casellaDiagonaleSottoSinistraGameObject.GetComponent<SpriteRenderer>();
                    if(casellaDiagonaleSottoSinistra.color != Color.white)
                    {
                        casellaDiagonaleSottoSinistra.color = new Color(255f, 255f, 0f, 255f);
                        Vector3 spawnPos = casellaDiagonaleSottoSinistraGameObject.transform.position;
                        Instantiate(difficoltaDue, spawnPos, Quaternion.identity);
                    }
                }
                GameObject casellaSottoGameObject = GameObject.Find(Utils.Sotto(ultimaCasellaString));
                GameObject casellaSbagliataSotto = GameObject.Find("Errore " + Utils.Sotto(ultimaCasellaString));
                if(casellaSottoGameObject != null && casellaSbagliataSotto == null)
                {
                    SpriteRenderer casellaSotto = casellaSottoGameObject.GetComponent<SpriteRenderer>();
                    if(casellaSotto.color != Color.white)
                    {
                        casellaSotto.color = new Color(255f, 255f, 0f, 255f);
                        Vector3 spawnPos = casellaSottoGameObject.transform.position;
                        Instantiate(difficoltaUno, spawnPos, Quaternion.identity);
                    }
                }
            }
            else
            {
                string ultimaCasellaString = gameData.stringValues[gameData.stringValues.Count - 1];
                Transform ultimaCasellaTransform = baseScacchiera.transform.Find(ultimaCasellaString);
                List<string> caselleGiusteSbagliate = new List<string>();
                caselleGiusteSbagliate.AddRange(gameData.stringValues);
                caselleGiusteSbagliate.AddRange(gameData.caselleSbagliate);
                if(Utils.RigaSuperiore(ultimaCasellaString, caselleGiusteSbagliate))
                {
                    gameoverLogo.SetActive(true);
                    restart.SetActive(true);
                }
                else
                {
                    if (ultimaCasellaTransform != null)
                    {
                        GameObject ultimaCasella = ultimaCasellaTransform.gameObject;
                        Vector3 spawnPosition = ultimaCasella.transform.position;
                        player = Instantiate(player, spawnPosition, Quaternion.identity);
                    }
                    GameObject casellaSopraGameObject = GameObject.Find(Utils.Sopra(ultimaCasellaString));
                    GameObject casellaSbagliataSopra = GameObject.Find("Errore " + Utils.Sopra(ultimaCasellaString));
                    if (casellaSopraGameObject != null && casellaSbagliataSopra == null)
                    {
                        SpriteRenderer casellaSopra = casellaSopraGameObject.GetComponent<SpriteRenderer>();
                        if(casellaSopraGameObject.name == "Casella 5,3")
                        {
                            casellaSopra.color = Color.white;
                            Vector3 spawanPos = casellaSopraGameObject.transform.position;
                            bandieraTraguardo.SetActive(false);
                            logoTraguardo.SetActive(true);
                        }
                        else if(casellaSopra.color != Color.white)
                        {
                            casellaSopra.color = new Color(255f, 255f, 0f, 255f);
                            Vector3 spawnPos = casellaSopraGameObject.transform.position;
                            Instantiate(difficoltaDue, spawnPos, Quaternion.identity);
                        }
                    }
                    GameObject casellaDiagonaleGameObject = GameObject.Find(Utils.DiagonaleSinistra(ultimaCasellaString));
                    GameObject casellaSbagliataDiagonale = GameObject.Find("Errore " + Utils.DiagonaleSinistra(ultimaCasellaString));
                    if (casellaDiagonaleGameObject != null && casellaSbagliataDiagonale == null)
                    {
                        SpriteRenderer casellaDiagonale = casellaDiagonaleGameObject.GetComponent<SpriteRenderer>();
                        if(casellaDiagonaleGameObject.name == "Casella 5,3")
                        {
                            casellaDiagonale.color = Color.white;
                            bandieraTraguardo.SetActive(false);
                            logoTraguardo.SetActive(true);
                        }
                        else if(casellaDiagonale.color != Color.white)
                        {
                            casellaDiagonale.color = Color.red;
                            Vector3 spawnPos = casellaDiagonaleGameObject.transform.position;
                            Instantiate(difficoltaTre, spawnPos, Quaternion.identity);
                        }
                    }
                    GameObject casellaSinistraGameObject = GameObject.Find(Utils.Sinistra(ultimaCasellaString));
                    GameObject casellaSbagliataSinistra = GameObject.Find("Errore " + Utils.Sinistra(ultimaCasellaString));
                    bool valoreCasuale = Random.Range(0, 2) == 0; 
                    if (casellaSinistraGameObject != null && casellaSbagliataSinistra == null)
                    {
                        SpriteRenderer casellaSinistra = casellaSinistraGameObject.GetComponent<SpriteRenderer>();
                        if(casellaSinistraGameObject.name == "Casella 5,3")
                        {
                            casellaSinistra.color = Color.white;
                            bandieraTraguardo.SetActive(false);
                            logoTraguardo.SetActive(true);
                        }
                        else
                        {
                            if(valoreCasuale)
                            {
                                if(casellaSinistra.color != Color.white)
                                {
                                    casellaSinistra.color = Color.green;
                                    Vector3 spawnPos = casellaSinistraGameObject.transform.position;
                                    Instantiate(difficoltaUno, spawnPos, Quaternion.identity);
                                }
                            }
                            else
                            {
                                if(casellaSinistra.color != Color.white)
                                {
                                    casellaSinistra.color = new Color(255f, 255f, 0f, 255f);
                                    Vector3 spawnPos = casellaSinistraGameObject.transform.position;
                                    Instantiate(difficoltaDue, spawnPos, Quaternion.identity);
                                }
                            }
                        }
                    }
                    GameObject casellaDiagonaleDestraGameObject = GameObject.Find(Utils.DiagonaleDestra(ultimaCasellaString));
                    GameObject casellaSbagliataDiagonaleDestra = GameObject.Find("Errore " + Utils.DiagonaleDestra(ultimaCasellaString));
                    if(casellaDiagonaleDestraGameObject != null && casellaSbagliataDiagonaleDestra == null)
                    {
                        SpriteRenderer casellaDiagonaleDestra = casellaDiagonaleDestraGameObject.GetComponent<SpriteRenderer>();
                        if(casellaDiagonaleDestra.color != Color.white)
                        {
                            casellaDiagonaleDestra.color = Color.red;
                            Vector3 spawnPos = casellaDiagonaleDestraGameObject.transform.position;
                            Instantiate(difficoltaTre, spawnPos, Quaternion.identity);
                        }
                    }
                    GameObject casellaDestraGameObject = GameObject.Find(Utils.Destra(ultimaCasellaString));
                    GameObject casellaSbagliataDestra = GameObject.Find("Errore " + Utils.Destra(ultimaCasellaString));
                    if(casellaDestraGameObject != null && casellaSbagliataDestra == null)
                    {
                        SpriteRenderer casellaDestra = casellaDestraGameObject.GetComponent<SpriteRenderer>();
                        if(valoreCasuale)
                        {
                            if(casellaDestra.color != Color.white)
                            {
                                casellaDestra.color = new Color(255f, 255f, 0f, 255f);
                                Vector3 spawnPos = casellaDestraGameObject.transform.position;
                                Instantiate(difficoltaDue, spawnPos, Quaternion.identity);
                            }
                        }
                        else
                        {
                            if(casellaDestra.color != Color.white)
                            {
                                casellaDestra.color = Color.green;
                                Vector3 spawnPos = casellaDestraGameObject.transform.position;
                                Instantiate(difficoltaUno, spawnPos, Quaternion.identity);
                            }
                        }
                    }
                    GameObject casellaDiagonaleSottoDestraGameObject = GameObject.Find(Utils.DiagonaleSottoDestra(ultimaCasellaString));
                    GameObject casellaSbagliataDiagonaleSottoDestra = GameObject.Find("Errore " + Utils.DiagonaleSottoDestra(ultimaCasellaString));
                    if(casellaDiagonaleSottoDestraGameObject != null && casellaSbagliataDiagonaleSottoDestra == null)
                    {
                        SpriteRenderer casellaDiagonaleSottoDestra = casellaDiagonaleSottoDestraGameObject.GetComponent<SpriteRenderer>();
                        if(casellaDiagonaleSottoDestra.color != Color.white)
                        {
                            casellaDiagonaleSottoDestra.color = new Color(255f, 255f, 0f, 255f);
                            Vector3 spawnPos = casellaDiagonaleSottoDestraGameObject.transform.position;
                            Instantiate(difficoltaDue, spawnPos, Quaternion.identity);
                        }
                    }
                    GameObject casellaDiagonaleSottoSinistraGameObject = GameObject.Find(Utils.DiagonaleSottoSinistra(ultimaCasellaString));
                    GameObject casellaSbagliataDiagonaleSottoSinistra = GameObject.Find("Errore " + Utils.DiagonaleSottoSinistra(ultimaCasellaString));
                    if(casellaDiagonaleSottoSinistraGameObject != null && casellaSbagliataDiagonaleSottoSinistra == null)
                    {
                        SpriteRenderer casellaDiagonaleSottoSinistra = casellaDiagonaleSottoSinistraGameObject.GetComponent<SpriteRenderer>();
                        if(casellaDiagonaleSottoSinistra.color != Color.white)
                        {
                            casellaDiagonaleSottoSinistra.color = new Color(255f, 255f, 0f, 255f);
                            Vector3 spawnPos = casellaDiagonaleSottoSinistraGameObject.transform.position;
                            Instantiate(difficoltaDue, spawnPos, Quaternion.identity);
                        }
                    }
                    GameObject casellaSottoGameObject = GameObject.Find(Utils.Sotto(ultimaCasellaString));
                    GameObject casellaSbagliataSotto = GameObject.Find("Errore " + Utils.Sotto(ultimaCasellaString));
                    if(casellaSottoGameObject != null && casellaSbagliataSotto == null)
                    {
                        SpriteRenderer casellaSotto = casellaSottoGameObject.GetComponent<SpriteRenderer>();
                        if(casellaSotto.color != Color.white)
                        {
                            casellaSotto.color = new Color(255f, 255f, 0f, 255f);
                            Vector3 spawnPos = casellaSottoGameObject.transform.position;
                            Instantiate(difficoltaUno, spawnPos, Quaternion.identity);
                        }
                    }
                    // verifica che non ci sono più strade da percorrere
                    // (-casellaSopra V casellaSbagliataSopra)^(-casellaDiagonale V casellaSbagliataDiagonale)^(-casellaSinistra V casellaSbagliataSinistra)^(-casellaDiagonaleDestra V casellaSbagliataDiagonaleDestra)^(-casellaDestra V casellaSbagliataDestra)
                    if(
                        (
                            (!(casellaSopraGameObject != null) || (casellaSbagliataSopra != null))
                            && 
                            (!(casellaDiagonaleGameObject != null) || (casellaSbagliataDiagonale != null)) 
                            && 
                            (!(casellaSinistraGameObject != null) || (casellaSbagliataSinistra != null)) 
                            &&
                            (!(casellaDiagonaleDestraGameObject != null) || (casellaSbagliataDiagonaleDestra != null))
                            &&
                            (!(casellaDestraGameObject != null) || (casellaSbagliataDestra != null))
                            &&
                            (!(casellaDiagonaleSottoDestraGameObject != null) || (casellaSbagliataDiagonaleSottoDestra != null))
                            &&
                            (!(casellaDiagonaleSottoSinistraGameObject != null) || (casellaSbagliataDiagonaleSottoSinistra != null))
                            &&
                            (!(casellaSottoGameObject != null) || (casellaSbagliataSotto != null))
                        )
                        ||
                        (
                            ((casellaSopraGameObject != null) && (casellaSbagliataSopra != null))
                            && 
                            ((casellaDiagonaleGameObject != null) && (casellaSbagliataDiagonale != null)) 
                            && 
                            ((casellaSinistraGameObject != null) && (casellaSbagliataSinistra != null)) 
                            &&
                            ((casellaDiagonaleDestraGameObject != null) && (casellaSbagliataDiagonaleDestra != null))
                            &&
                            ((casellaDestraGameObject != null) && (casellaSbagliataDestra == null))
                            &&
                            (casellaDestraGameObject.name.EndsWith("0"))
                        )
                        ||
                        (
                            ((casellaSopraGameObject != null) && (casellaSbagliataSopra != null))
                            && 
                            ((casellaDiagonaleGameObject != null) && (casellaSbagliataDiagonale != null)) 
                            && 
                            ((casellaSinistraGameObject != null) && (casellaSbagliataSinistra == null)) 
                            &&
                            ((casellaDiagonaleDestraGameObject != null) && (casellaSbagliataDiagonaleDestra != null))
                            &&
                            ((casellaDestraGameObject != null) && (casellaSbagliataDestra != null))
                            &&
                            (casellaSinistraGameObject.name.EndsWith("3"))
                        )
                    )
                    {
                        gameoverLogo.SetActive(true);
                        restart.SetActive(true);
                    }
                }
            }
        }
    }

    IEnumerator ErrorAnimation(Animator animator)
    {
        // Avvia l'animazione
        animator.SetTrigger("Attivazione");

        // Ottieni la durata dello stato attivo
        AnimatorStateInfo animationInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = animationInfo.length;

        // Aspetta la durata dell'animazione
        yield return new WaitForSeconds(animationDuration);
    }

    // Coroutine che muove l'oggetto
    IEnumerator MoveToTarget(Vector3 playerPosition, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = playerPosition;
        float timeElapsed = 0;
        Animator animator = player.GetComponent<Animator>();
        // Attiva l'animazione di movimento
        if (animator != null)
        {
            animator.SetTrigger("Attivazione");
        }

        // Movimento lineare (dal punto A al punto B)
        while (timeElapsed < duration)
        {
            player.transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null; // Aspetta il prossimo frame
        }

        // Assicurati che il gameObject arrivi esattamente alla destinazione
        player.transform.position = targetPosition;
        // Attiva l'animazione di movimento
        if (animator != null)
        {
            animator.SetTrigger("Disattivazione");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}