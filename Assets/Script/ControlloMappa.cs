using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

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
    public int[] pesi;

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
        if(gameData.traguardo == "")
        {
            string[] possibleChars = { "1", "2", "3", "0" };
            int randomIndex = UnityEngine.Random.Range(0, possibleChars.Length);
            gameData.traguardo = "Casella " + "5," + possibleChars[randomIndex];
            gameData.SaveData();
            GameObject casellaFinale = GameObject.Find(gameData.traguardo);
            if(casellaFinale != null)
            {
                UnityEngine.Vector3 spawnPos = casellaFinale.transform.position;
                bandieraTraguardo = Instantiate(bandieraTraguardo, spawnPos, UnityEngine.Quaternion.identity);
            }
        }
        else
        {
            GameObject casellaFinale = GameObject.Find(gameData.traguardo);
            if(casellaFinale != null)
            {
                UnityEngine.Vector3 spawnPos = casellaFinale.transform.position;
                bandieraTraguardo = Instantiate(bandieraTraguardo, spawnPos, UnityEngine.Quaternion.identity);
            }
        }
        if(gameData.inizio == "")
        {
            string[] possibleChars = { "1", "2", "3", "0" };
            int randomIndex = UnityEngine.Random.Range(0, possibleChars.Length);
            gameData.inizio = "Casella " + "0," + possibleChars[randomIndex];
            gameData.SaveData();
            GameObject casellaInizio = GameObject.Find(gameData.inizio);
            if(casellaInizio != null)
            {
                UnityEngine.Vector3 spawnPos = casellaInizio.transform.position;
                player = Instantiate(player, spawnPos, UnityEngine.Quaternion.identity);
            }
        }
        else
        {
            GameObject casellaInizio = GameObject.Find(gameData.inizio);
            if(casellaInizio != null)
            {
                UnityEngine.Vector3 spawnPos = casellaInizio.transform.position;
                player = Instantiate(player, spawnPos, UnityEngine.Quaternion.identity);
            }
        }
        bool nuovoErrore = false;
        if(gameData.caselleSbagliate.Count > 0 && TempData.ultimoErrore != gameData.caselleSbagliate[gameData.caselleSbagliate.Count - 1])
        {
            nuovoErrore = true;
        }
        foreach (string i in gameData.caselleSbagliate)
        {
            UnityEngine.Vector3 casellaSbagliataPosition = GameObject.Find(i).gameObject.transform.position;
            GameObject casella = Instantiate(errore, casellaSbagliataPosition, UnityEngine.Quaternion.identity);
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
            List<string> caselleGiusteSbagliate = new List<string>();
            caselleGiusteSbagliate.AddRange(gameData.stringValues);
            caselleGiusteSbagliate.AddRange(gameData.caselleSbagliate);
            string ultimaCasellaString = gameData.stringValues[gameData.stringValues.Count - 1];
            Transform ultimaCasellaTransform = baseScacchiera.transform.Find(ultimaCasellaString);
            pesi = CondizioneGameOver(caselleGiusteSbagliate, ultimaCasellaString);
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
                    if(pesi[0] == 1)
                    {
                        casellaSopra.color = Color.red;
                        UnityEngine.Vector3 spawnPos = casellaSopraGameObject.transform.position;
                        Instantiate(difficoltaTre, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[0] == 2)
                    {
                        casellaSopra.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = casellaSopraGameObject.transform.position;
                        Instantiate(difficoltaDue, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[0] == 3)
                    {
                        casellaSopra.color = Color.green;
                        UnityEngine.Vector3 spawnPos = casellaSopraGameObject.transform.position;
                        Instantiate(difficoltaUno, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
                GameObject casellaDiagonaleGameObject = GameObject.Find(Utils.DiagonaleSinistra(casella.name));
                GameObject casellaSbagliataDiagonale = GameObject.Find("Errore " + Utils.DiagonaleSinistra(casella.name));
                if (casellaDiagonaleGameObject != null && casellaSbagliataDiagonale == null)
                {
                    SpriteRenderer casellaDiagonale = casellaDiagonaleGameObject.GetComponent<SpriteRenderer>();
                    if(pesi[7] == 1)
                    {
                        casellaDiagonale.color = Color.red;
                        UnityEngine.Vector3 spawnPos = casellaDiagonaleGameObject.transform.position;
                        Instantiate(difficoltaTre, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[7] == 2)
                    {
                        casellaDiagonale.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = casellaDiagonaleGameObject.transform.position;
                        Instantiate(difficoltaDue, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[7] == 3)
                    {
                        casellaDiagonale.color = Color.green;
                        UnityEngine.Vector3 spawnPos = casellaDiagonaleGameObject.transform.position;
                        Instantiate(difficoltaUno, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
                GameObject casellaSinistraGameObject = GameObject.Find(Utils.Sinistra(casella.name));
                GameObject casellaSbagliataSinistra = GameObject.Find("Errore " + Utils.Sinistra(casella.name));
                if (casellaSinistraGameObject != null && casellaSbagliataSinistra == null)
                {
                    SpriteRenderer casellaSinistra = casellaSinistraGameObject.GetComponent<SpriteRenderer>();
                    if(pesi[6] == 1)
                    {
                        casellaSinistra.color = Color.red;
                        UnityEngine.Vector3 spawnPos = casellaSinistraGameObject.transform.position;
                        Instantiate(difficoltaTre, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[6] == 2)
                    {
                        casellaSinistra.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = casellaSinistraGameObject.transform.position;
                        Instantiate(difficoltaDue, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[6] == 3)
                    {
                        casellaSinistra.color = Color.green;
                        UnityEngine.Vector3 spawnPos = casellaSinistraGameObject.transform.position;
                        Instantiate(difficoltaUno, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
                UnityEngine.Vector3 spawnPosition = casella.transform.position;
                player = Instantiate(player, spawnPosition, UnityEngine.Quaternion.identity);
                if (casellaSbagliataSopra != null && casellaSbagliataDiagonale != null && casellaSbagliataSinistra != null)
                {
                    gameoverLogo.SetActive(true);
                    restart.SetActive(true);
                }
            }
        }
        else if (gameData.stringValues[gameData.stringValues.Count - 1] == gameData.traguardo)
        {
            bandieraTraguardo.SetActive(false);
            theEnd.SetActive(true);
            restart.SetActive(true);
        }
        else
        {
            scrittaPrincipale.SetActive(true);
            List<string> caselleGiusteSbagliate = new List<string>();
            caselleGiusteSbagliate.AddRange(gameData.stringValues);
            caselleGiusteSbagliate.AddRange(gameData.caselleSbagliate);
            string ultimaCasellaString = gameData.stringValues[gameData.stringValues.Count - 1];
            Transform ultimaCasellaTransform = baseScacchiera.transform.Find(ultimaCasellaString);
            pesi = CondizioneGameOver(caselleGiusteSbagliate, ultimaCasellaString);
            GameObject casellaSopraGameObject = GameObject.Find(Utils.Sopra(ultimaCasellaString));
            GameObject casellaSbagliataSopra = GameObject.Find("Errore " + Utils.Sopra(ultimaCasellaString));
            if (casellaSopraGameObject != null && casellaSbagliataSopra == null)
            {
                SpriteRenderer casellaSopra = casellaSopraGameObject.GetComponent<SpriteRenderer>();
                if(casellaSopraGameObject.name == gameData.traguardo)
                {
                    bandieraTraguardo.SetActive(false);
                    UnityEngine.Vector3 spawnPos = GameObject.Find(gameData.traguardo).transform.position;
                    Instantiate(logoTraguardo, spawnPos, UnityEngine.Quaternion.identity);
                    casellaSopra.color = Color.white;
                }
                else if(casellaSopra.color != Color.white)
                {
                    if(pesi[0] == 1)
                    {
                        casellaSopra.color = Color.red;
                        UnityEngine.Vector3 spawnPos = casellaSopraGameObject.transform.position;
                        Instantiate(difficoltaTre, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[0] == 2)
                    {
                        casellaSopra.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = casellaSopraGameObject.transform.position;
                        Instantiate(difficoltaDue, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[0] == 3)
                    {
                        casellaSopra.color = Color.green;
                        UnityEngine.Vector3 spawnPos = casellaSopraGameObject.transform.position;
                        Instantiate(difficoltaUno, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
            }
            GameObject casellaDiagonaleGameObject = GameObject.Find(Utils.DiagonaleSinistra(ultimaCasellaString));
            GameObject casellaSbagliataDiagonale = GameObject.Find("Errore " + Utils.DiagonaleSinistra(ultimaCasellaString));
            if (casellaDiagonaleGameObject != null && casellaSbagliataDiagonale == null)
            {
                SpriteRenderer casellaDiagonale = casellaDiagonaleGameObject.GetComponent<SpriteRenderer>();
                if(casellaDiagonaleGameObject.name == gameData.traguardo)
                {
                    bandieraTraguardo.SetActive(false);
                    UnityEngine.Vector3 spawnPos = GameObject.Find(gameData.traguardo).transform.position;
                    Instantiate(logoTraguardo, spawnPos, UnityEngine.Quaternion.identity);
                    casellaDiagonale.color = Color.white;
                }
                else if(casellaDiagonale.color != Color.white)
                {
                    if(pesi[7] == 1)
                    {
                        casellaDiagonale.color = Color.red;
                        UnityEngine.Vector3 spawnPos = casellaDiagonaleGameObject.transform.position;
                        Instantiate(difficoltaTre, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[7] == 2)
                    {
                        casellaDiagonale.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = casellaDiagonaleGameObject.transform.position;
                        Instantiate(difficoltaDue, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[7] == 3)
                    {
                        casellaDiagonale.color = Color.green;
                        UnityEngine.Vector3 spawnPos = casellaDiagonaleGameObject.transform.position;
                        Instantiate(difficoltaUno, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
            }
            GameObject casellaSinistraGameObject = GameObject.Find(Utils.Sinistra(ultimaCasellaString));
            GameObject casellaSbagliataSinistra = GameObject.Find("Errore " + Utils.Sinistra(ultimaCasellaString));
            valoreCasuale = UnityEngine.Random.Range(0, 2) == 0; 
            if (casellaSinistraGameObject != null && casellaSbagliataSinistra == null)
            {
                SpriteRenderer casellaSinistra = casellaSinistraGameObject.GetComponent<SpriteRenderer>();
                if(casellaSinistraGameObject.name == gameData.traguardo)
                {
                    bandieraTraguardo.SetActive(false);
                    UnityEngine.Vector3 spawnPos = GameObject.Find(gameData.traguardo).transform.position;
                    Instantiate(logoTraguardo, spawnPos, UnityEngine.Quaternion.identity);
                    casellaSinistra.color = Color.white;
                }
                else if(casellaSinistra.color != Color.white)
                {
                    if(pesi[6] == 1)
                    {
                        casellaSinistra.color = Color.red;
                        UnityEngine.Vector3 spawnPos = casellaSinistraGameObject.transform.position;
                        Instantiate(difficoltaTre, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[6] == 2)
                    {
                        casellaSinistra.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = casellaSinistraGameObject.transform.position;
                        Instantiate(difficoltaDue, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[6] == 3)
                    {
                        casellaSinistra.color = Color.green;
                        UnityEngine.Vector3 spawnPos = casellaSinistraGameObject.transform.position;
                        Instantiate(difficoltaUno, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
            }
            GameObject casellaDiagonaleDestraGameObject = GameObject.Find(Utils.DiagonaleDestra(ultimaCasellaString));
            GameObject casellaSbagliataDiagonaleDestra = GameObject.Find("Errore " + Utils.DiagonaleDestra(ultimaCasellaString));
            if(casellaDiagonaleDestraGameObject != null && casellaSbagliataDiagonaleDestra == null)
            {
                SpriteRenderer casellaDiagonaleDestra = casellaDiagonaleDestraGameObject.GetComponent<SpriteRenderer>();
                if(casellaDiagonaleDestraGameObject.name == gameData.traguardo)
                {
                    bandieraTraguardo.SetActive(false);
                    UnityEngine.Vector3 spawnPos = GameObject.Find(gameData.traguardo).transform.position;
                    Instantiate(logoTraguardo, spawnPos, UnityEngine.Quaternion.identity);
                    casellaDiagonaleDestra.color = Color.white;
                }
                else if(casellaDiagonaleDestra.color != Color.white)
                {
                    if(pesi[1] == 1)
                    {
                        casellaDiagonaleDestra.color = Color.red;
                        UnityEngine.Vector3 spawnPos = casellaDiagonaleDestraGameObject.transform.position;
                        Instantiate(difficoltaTre, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[1] == 2)
                    {
                        casellaDiagonaleDestra.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = casellaDiagonaleDestraGameObject.transform.position;
                        Instantiate(difficoltaDue, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[1] == 3)
                    {
                        casellaDiagonaleDestra.color = Color.green;
                        UnityEngine.Vector3 spawnPos = casellaDiagonaleDestraGameObject.transform.position;
                        Instantiate(difficoltaUno, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
            }
            GameObject casellaDestraGameObject = GameObject.Find(Utils.Destra(ultimaCasellaString));
            GameObject casellaSbagliataDestra = GameObject.Find("Errore " + Utils.Destra(ultimaCasellaString));
            if(casellaDestraGameObject != null && casellaSbagliataDestra == null)
            {
                SpriteRenderer casellaDestra = casellaDestraGameObject.GetComponent<SpriteRenderer>();
                if(casellaDestraGameObject.name == gameData.traguardo)
                {
                    bandieraTraguardo.SetActive(false);
                    UnityEngine.Vector3 spawnPos = GameObject.Find(gameData.traguardo).transform.position;
                    Instantiate(logoTraguardo, spawnPos, UnityEngine.Quaternion.identity);
                    casellaDestra.color = Color.white;
                }
                else if(casellaDestra.color != Color.white)
                {
                    if(pesi[2] == 1)
                    {
                        casellaDestra.color = Color.red;
                        UnityEngine.Vector3 spawnPos = casellaDestraGameObject.transform.position;
                        Instantiate(difficoltaTre, spawnPos, UnityEngine.Quaternion.identity); 
                    }
                    else if(pesi[2] == 2)
                    {
                        casellaDestra.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = casellaDestraGameObject.transform.position;
                        Instantiate(difficoltaDue, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[2] == 3)
                    {
                        casellaDestra.color = Color.green;
                        UnityEngine.Vector3 spawnPos = casellaDestraGameObject.transform.position;
                        Instantiate(difficoltaUno, spawnPos, UnityEngine.Quaternion.identity);
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
                    if(pesi[3] == 1)
                    {
                        casellaDiagonaleSottoDestra.color = Color.red;
                        UnityEngine.Vector3 spawnPos = casellaDiagonaleSottoDestraGameObject.transform.position;
                        Instantiate(difficoltaTre, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[3] == 2)
                    {
                        casellaDiagonaleSottoDestra.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = casellaDiagonaleSottoDestraGameObject.transform.position;
                        Instantiate(difficoltaDue, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[3] == 3)
                    {
                        casellaDiagonaleSottoDestra.color = Color.green;
                        UnityEngine.Vector3 spawnPos = casellaDiagonaleSottoDestraGameObject.transform.position;
                        Instantiate(difficoltaUno, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
            }
            GameObject casellaDiagonaleSottoSinistraGameObject = GameObject.Find(Utils.DiagonaleSottoSinistra(ultimaCasellaString));
            GameObject casellaSbagliataDiagonaleSottoSinistra = GameObject.Find("Errore " + Utils.DiagonaleSottoSinistra(ultimaCasellaString));
            if(casellaDiagonaleSottoSinistraGameObject != null && casellaSbagliataDiagonaleSottoSinistra == null)
            {
                SpriteRenderer casellaDiagonaleSottoSinistra = casellaDiagonaleSottoSinistraGameObject.GetComponent<SpriteRenderer>();
                if(casellaDiagonaleSottoSinistra.color != Color.white)
                {
                    if(pesi[5] == 1)
                    {
                        casellaDiagonaleSottoSinistra.color = Color.red;
                        UnityEngine.Vector3 spawnPos = casellaDiagonaleSottoSinistraGameObject.transform.position;
                        Instantiate(difficoltaTre, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[5] == 2)
                    {
                        casellaDiagonaleSottoSinistra.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = casellaDiagonaleSottoSinistraGameObject.transform.position;
                        Instantiate(difficoltaDue, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[5] == 3)
                    {
                        casellaDiagonaleSottoSinistra.color = Color.green;
                        UnityEngine.Vector3 spawnPos = casellaDiagonaleSottoSinistraGameObject.transform.position;
                        Instantiate(difficoltaUno, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
            }
            GameObject casellaSottoGameObject = GameObject.Find(Utils.Sotto(ultimaCasellaString));
            GameObject casellaSbagliataSotto = GameObject.Find("Errore " + Utils.Sotto(ultimaCasellaString));
            if(casellaSottoGameObject != null && casellaSbagliataSotto == null)
            {
                SpriteRenderer casellaSotto = casellaSottoGameObject.GetComponent<SpriteRenderer>();
                if(casellaSotto.color != Color.white)
                {
                    if(pesi[4] == 1)
                    {
                        casellaSotto.color = Color.red;
                        UnityEngine.Vector3 spawnPos = casellaSottoGameObject.transform.position;
                        Instantiate(difficoltaTre, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[4] == 2)
                    {
                        casellaSotto.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = casellaSottoGameObject.transform.position;
                        Instantiate(difficoltaDue, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(pesi[4] == 3)
                    {
                        casellaSotto.color = Color.green;
                        UnityEngine.Vector3 spawnPos = casellaSottoGameObject.transform.position;
                        Instantiate(difficoltaUno, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
            }
            if(TempData.gioco && TempData.vittoria)
            {
                string penultimaCasellaString = gameData.stringValues[gameData.stringValues.Count - 2];
                Transform penultimaCasellaTransform = baseScacchiera.transform.Find(penultimaCasellaString);
                if (ultimaCasellaTransform != null && penultimaCasellaTransform != null)
                {
                    GameObject ultimaCasella = ultimaCasellaTransform.gameObject;
                    GameObject penultimaCasella = penultimaCasellaTransform.gameObject;
                    UnityEngine.Vector3 spawnPosition = penultimaCasella.transform.position;
                    player = Instantiate(player, spawnPosition, UnityEngine.Quaternion.identity);
                    StartCoroutine(MoveToTarget(spawnPosition, ultimaCasella.transform.position, moveDuration));
                }
            }
            else
            {
                if (ultimaCasellaTransform != null)
                {
                    GameObject ultimaCasella = ultimaCasellaTransform.gameObject;
                    UnityEngine.Vector3 spawnPosition = ultimaCasella.transform.position;
                    player = Instantiate(player, spawnPosition, UnityEngine.Quaternion.identity);
                }

                if(Utils.RigaSuperiore(ultimaCasellaString, caselleGiusteSbagliate))
                {
                    gameoverLogo.SetActive(true);
                    restart.SetActive(true);
                }
                else
                {
                    
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
    IEnumerator MoveToTarget(UnityEngine.Vector3 playerPosition, UnityEngine.Vector3 targetPosition, float duration)
    {
        UnityEngine.Vector3 startPosition = playerPosition;
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
            player.transform.position = UnityEngine.Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
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

    int[] CondizioneGameOver(List<string> caselleGiusteSbagliate, string ultimaCasella){
        
        int[,] matrice = new int[6, 4];

        for( int x = 0; x < caselleGiusteSbagliate.Count; x++ )
        {
            int[] numeri = Utils.PrendiNumeri(caselleGiusteSbagliate[x]);
            matrice[numeri[0], numeri[1]] = -1;
        }

        // Stampa il risultato
        PrintMatrix(matrice);

        // Partenza del BFS
        int startX = 5, startY = 3;

        // Chiama l'algoritmo BFS
        BFS(matrice, startX, startY);

        matrice[5, 3] = 1;
        
        // Stampa il risultato
        PrintMatrix(matrice);

        int[] appo = Utils.PrendiNumeri(ultimaCasella);

        return minMax(matrice, appo[0], appo[1]);
    }

    private int[] minMax(int[,] matrice, int varX, int varY)
    {
        int[] miMa = new int[2];
        miMa[0] = 100;
        miMa[1] = 0;
        var directions = new List<(int, int)>
        {
            (-1,  0), // Su
            ( 1,  0), // Giù
            ( 0, -1), // Sinistra
            ( 0,  1), // Destra
            (-1, -1), // Alto-sinistra
            ( 1,  1), // Basso-destra
            ( 1, -1), // Basso-sinistra
            (-1,  1)  // Alto-destra
        };

        foreach (var (dx, dy) in directions)
        {
            int newX = varX + dx;
            int newY = varY + dy;

            // Controlla che la nuova posizione sia valida
            if (newX >= 0 && newX < 6 && newY >= 0 && newY < 6 && matrice[newX, newY] != -1)
            {
                // Aggiorna il minimo
                if (miMa[0] > matrice[newX, newY])
                {
                    miMa[0] = matrice[newX, newY];
                }
                // Aggiorna il massimo
                if (miMa[1] < matrice[newX, newY])
                {
                    miMa[1] = matrice[newX, newY];
                }
            }
        }

        // Mappa di direzioni e indici corrispondenti per 'pesi'
        var directionIndices = new Dictionary<(int, int), int>
        {
            { (1,  0), 0 }, // Su
            { (1,  -1), 1 }, // Alto-destra
            { ( 0,  -1), 2 }, // Destra
            { ( -1,  -1), 3 }, // Basso-destra
            { ( -1,  0), 4 }, // Giù
            { ( -1, 1), 5 }, // Basso-sinistra
            { ( 0, 1), 6 }, // Sinistra
            { (1, 1), 7 }  // Alto-sinistra
        };

        int[] pesi = new int[8];

        foreach (var (dx, dy) in directions)
        {
            int newX = varX + dx;
            int newY = varY + dy;

            // Controlla che la nuova posizione sia valida
            if (newX >= 0 && newX < 6 && newY >= 0 && newY < 6 && matrice[newX, newY] != -1)
            {
                int indice = directionIndices[(dx, dy)]; // Ottieni l'indice associato alla direzione
                if (matrice[newX, newY] == miMa[0])
                {
                    pesi[indice] = 1;
                }
                else if (matrice[newX, newY] == miMa[1])
                {
                    pesi[indice] = 3;
                }
                else
                {
                    pesi[indice] = 2;
                }
            }
        }

        string stampa = "";
        foreach(int i in pesi)
        {
            stampa += i + " ";
        }
        Debug.Log(stampa);

        return pesi;
    }


    public static void BFS(int[,] matrice, int startX, int startY)
    {
        int rows = matrice.GetLength(0);
        int cols = matrice.GetLength(1);

        // Direzioni per muoversi (su, giù, sinistra, destra + diagonali)
        int[] dirX = { -1,  1,  0,  0, -1, -1,  1,  1 };
        int[] dirY = {  0,  0, -1,  1, -1,  1, -1,  1 };

        // Inizializza la coda per il BFS
        Queue<(int, int)> queue = new Queue<(int, int)>();

        // Aggiungi il punto di partenza alla coda e imposta la distanza iniziale a 0
        queue.Enqueue((startX, startY));
        matrice[startX, startY] = 0;

        while (queue.Count > 0)
        {
            // Estrai l'elemento in testa alla coda
            var (x, y) = queue.Dequeue();

            // Esplora le direzioni adiacenti e diagonali
            for (int i = 0; i < dirX.Length; i++)
            {
                int newX = x + dirX[i];
                int newY = y + dirY[i];

                // Controlla se la nuova posizione è valida
                if (newX >= 0 && newX < rows && newY >= 0 && newY < cols)
                {
                    // Continua solo se la casella è raggiungibile e non ancora visitata
                    if (matrice[newX, newY] == 0)
                    {
                        matrice[newX, newY] = matrice[x, y] + 1; // Aggiorna il numero di passi
                        queue.Enqueue((newX, newY)); // Aggiungi la nuova posizione alla coda
                    }
                }
            }
        }

        // Imposta a -1 le caselle inaccessibili che non sono state raggiunte
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (matrice[i, j] == 0 && (i != startX || j != startY))
                {
                    matrice[i, j] = -1; // Non raggiunto
                }
            }
        }
    }

    public static void PrintMatrix(int[,] matrice)
    {
        int rows = matrice.GetLength(0);
        int cols = matrice.GetLength(1);
        string matrix = "";

        for (int i = 0; i < rows; i++)
        {
            string row = "";
            for (int j = 0; j < cols; j++)
            {
                row += matrice[i, j].ToString("D2") + " ";
            }
            matrix += row + "\n";
        }
        Debug.Log(matrix);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
