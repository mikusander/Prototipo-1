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
                    else if(casellaSopra.color != Color.white && casellaSopraGameObject.name != "Casella 5,3")
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
                        if(casellaSinistra.color != Color.white && casellaSopraGameObject.name != "Casella 5,3")
                        {
                            casellaSinistra.color = Color.green;
                            Vector3 spawnPos = casellaSinistraGameObject.transform.position;
                            Instantiate(difficoltaUno, spawnPos, Quaternion.identity);
                        }
                    }
                    else
                    {
                        if(casellaSinistra.color != Color.white && casellaSopraGameObject.name != "Casella 5,3")
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
                        else if(casellaSopra.color != Color.white && casellaSopraGameObject.name != "Casella 5,3")
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
                    // aggiungere condizione di gameover
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

    List<int> CondizioneGameOver(List<string> caselleGiusteSbagliate, string ultimaCasella){
        List<List<string>> percorsi = new List<List<string>>();
        string sopra = Utils.Sopra(ultimaCasella);
        if(sopra != "" && !Utils.ControlloPresenza(caselleGiusteSbagliate, sopra))
        {
            List<string> appo = new List<string>();
            appo.Add(sopra);
            percorsi.Add(appo);
        }
        string diagonaleDestra = Utils.DiagonaleDestra(ultimaCasella);
        if(diagonaleDestra != "" && !Utils.ControlloPresenza(caselleGiusteSbagliate, diagonaleDestra))
        {
            List<string> appo = new List<string>();
            appo.Add(diagonaleDestra);
            percorsi.Add(appo);
        }
        string destra = Utils.Destra(ultimaCasella);
        if(destra != "" && !Utils.ControlloPresenza(caselleGiusteSbagliate, destra))
        {
            List<string> appo = new List<string>();
            appo.Add(destra);
            percorsi.Add(appo);
        }
        string diagonaleSottoDestra = Utils.DiagonaleSottoDestra(ultimaCasella);
        if(diagonaleSottoDestra != "" && !Utils.ControlloPresenza(caselleGiusteSbagliate, diagonaleSottoDestra))
        {
            List<string> appo = new List<string>();
            appo.Add(diagonaleSottoDestra);
            percorsi.Add(appo);
        }
        string sotto = Utils.Sotto(ultimaCasella);
        if(sotto != "" && !Utils.ControlloPresenza(caselleGiusteSbagliate, sotto))
        {
            List<string> appo = new List<string>();
            appo.Add(sotto);
            percorsi.Add(appo);
        }
        string diagonaleSottoSinistra = Utils.DiagonaleSottoSinistra(ultimaCasella);
        if(diagonaleSottoSinistra != "" && !Utils.ControlloPresenza(caselleGiusteSbagliate, diagonaleSottoSinistra))
        {
            List<string> appo = new List<string>();
            appo.Add(diagonaleSottoSinistra);
            percorsi.Add(appo);
        }
        string sinistra = Utils.Sinistra(ultimaCasella);
        if(sinistra != "" && !Utils.ControlloPresenza(caselleGiusteSbagliate, sinistra))
        {
            List<string> appo = new List<string>();
            appo.Add(sinistra);
            percorsi.Add(appo);
        }
        string diagonaleSinistra = Utils.DiagonaleSinistra(ultimaCasella);
        if(diagonaleSinistra != "" && !Utils.ControlloPresenza(caselleGiusteSbagliate, diagonaleSinistra))
        {
            List<string> appo = new List<string>();
            appo.Add(diagonaleSinistra);
            percorsi.Add(appo);
        }
        bool modifica = false;
        int x = 0;
        while(x < percorsi.Count)
        {
            modifica = false;
            List<string> percorsoCorrente = percorsi[x];
            string ultimoNodo = percorsoCorrente[percorsoCorrente.Count - 1];
            if(ultimoNodo == "Casella 5,3")
            {
                x++;
                continue;
            }
            string sopraAttuale = Utils.Sopra(ultimoNodo);
            if(sopraAttuale != "" && !Utils.ControlloPresenza(caselleGiusteSbagliate, sopraAttuale))
            {
                if(percorsoCorrente.Count > 1)
                {
                    if(sopraAttuale != percorsoCorrente[percorsoCorrente.Count - 2])
                    {
                        modifica = true;
                        List<string> appo = new List<string>();
                        appo.AddRange(percorsoCorrente);
                        appo.Add(sopraAttuale);
                        percorsi.Add(appo);
                    }
                }
                else
                {
                    modifica = true;
                    List<string> appo = new List<string>();
                    appo.AddRange(percorsoCorrente);
                    appo.Add(sopraAttuale);
                    percorsi.Add(appo);
                }
            }

            string destraAttuale = Utils.Destra(ultimoNodo);
            if(destraAttuale != "" && !Utils.ControlloPresenza(caselleGiusteSbagliate, destraAttuale))
            {
                if(percorsoCorrente.Count > 1)
                {
                    if(destraAttuale != percorsoCorrente[percorsoCorrente.Count - 2])
                    {
                        modifica = true;
                        List<string> appo = new List<string>();
                        appo.AddRange(percorsoCorrente);
                        appo.Add(destraAttuale);
                        percorsi.Add(appo);
                    }
                }
                else
                {
                    modifica = true;
                    List<string> appo = new List<string>();
                    appo.AddRange(percorsoCorrente);
                    appo.Add(destraAttuale);
                    percorsi.Add(appo);
                }
            }

            string sottoAttuale = Utils.Sotto(ultimoNodo);
            if(sottoAttuale != "" && !Utils.ControlloPresenza(caselleGiusteSbagliate, sottoAttuale))
            {
                if(percorsoCorrente.Count > 1)
                {
                    if(sottoAttuale != percorsoCorrente[percorsoCorrente.Count - 2])
                    {
                        modifica = true;
                        List<string> appo = new List<string>();
                        appo.AddRange(percorsoCorrente);
                        appo.Add(sottoAttuale);
                        percorsi.Add(appo);
                    }
                }
                else
                {
                    modifica = true;
                    List<string> appo = new List<string>();
                    appo.AddRange(percorsoCorrente);
                    appo.Add(sottoAttuale);
                    percorsi.Add(appo);
                }
            }

            string sinistraAttuale = Utils.Destra(ultimoNodo);
            if(sinistraAttuale != "" && !Utils.ControlloPresenza(caselleGiusteSbagliate, sinistraAttuale))
            {
                if(percorsoCorrente.Count > 1)
                {
                    if(sinistraAttuale != percorsoCorrente[percorsoCorrente.Count - 2])
                    {
                        modifica = true;
                        List<string> appo = new List<string>();
                        appo.AddRange(percorsoCorrente);
                        appo.Add(sinistraAttuale);
                        percorsi.Add(appo);
                    }
                }
                else
                {
                    modifica = true;
                    List<string> appo = new List<string>();
                    appo.AddRange(percorsoCorrente);
                    appo.Add(sinistraAttuale);
                    percorsi.Add(appo);
                }
            }

            if(modifica)
            {
                percorsi.RemoveAt(x);
                modifica = false;
            }
            else
            {
                x++;
            }
        }

        int percorsoSopra = 100; // numero di caselle per arrivare al traguardo della casella superiore alla casuella attuale
        int percorsoDiagonaleDestra = 100; // numero di caselle per arrivare al traguardo della casella diagonale destra alla casuella attuale
        int percorsoDestra = 100; // numero di caselle per arrivare al traguardo della casella a destra alla casuella attuale
        int percorsoDiagonaleSottoDestra = 100; // numero di caselle per arrivare al traguardo della casella in diagonale sotto a destra alla casuella attuale
        int percorsoSotto = 100; // numero di caselle per arrivare al traguardo della casella inferiore alla casuella attuale
        int percorsoDiagonaleSOttoSinistra = 100; // numero di caselle per arrivare al traguardo della casella diagonale sotto sinistra alla casuella attuale
        int percorsoSinistra = 100; // numero di caselle per arrivare al traguardo della casella a sinistra alla casuella attuale
        int percorsoDiagonaleSinistra = 100; // numero di caselle per arrivare al traguardo della casella in diagonale sopra a sinistra alla casuella attuale
        for(int y = 0; y < percorsi.Count; y++)
        {
            List<string> percorsoAttuale = percorsi[y];
            if(percorsoAttuale[0] == sopra && percorsoAttuale.Count < percorsoSopra)
            {
                percorsoSopra = percorsoAttuale.Count;
            }
            else if(percorsoAttuale[0] == diagonaleDestra && percorsoAttuale.Count < percorsoDiagonaleDestra)
            {
                percorsoDiagonaleDestra = percorsoAttuale.Count;
            }
            else if(percorsoAttuale[0] == destra && percorsoAttuale.Count < percorsoDestra)
            {
                percorsoDestra = percorsoAttuale.Count;
            }
            else if(percorsoAttuale[0] == diagonaleSottoDestra && percorsoAttuale.Count < percorsoDiagonaleSottoDestra)
            {
                percorsoDiagonaleSottoDestra = percorsoAttuale.Count;
            }
            else if(percorsoAttuale[0] == sotto && percorsoAttuale.Count < percorsoSotto)
            {
                percorsoSotto = percorsoAttuale.Count;
            }
            else if(percorsoAttuale[0] == diagonaleSottoSinistra && percorsoAttuale.Count < percorsoDiagonaleSOttoSinistra)
            {
                percorsoDiagonaleSOttoSinistra = percorsoAttuale.Count;
            }
            else if(percorsoAttuale[0] == sinistra && percorsoAttuale.Count < percorsoSinistra)
            {
                percorsoSinistra = percorsoAttuale.Count;
            }
            else if(percorsoAttuale[0] == diagonaleSinistra && percorsoAttuale.Count < percorsoDiagonaleSinistra)
            {
                percorsoDiagonaleSinistra = percorsoAttuale.Count;
            }
        }
        List<int> numeroPassi = new List<int> 
        { 
            percorsoSopra,
            percorsoDiagonaleDestra,
            percorsoDestra,
            percorsoDiagonaleSottoDestra,
            percorsoSotto,
            percorsoDiagonaleSOttoSinistra,
            percorsoSinistra,
            percorsoDiagonaleSinistra
        };
        return numeroPassi;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
