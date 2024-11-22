using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
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

    // Start is called before the first frame update
    void Start()
    {
        // Carica i dati salvati (se esistono)
        gameData = GetComponent<GameData>();
        if (gameData != null)
        {
            gameData.LoadData();
        }
        bool nuovoErrore = false;
        if(TempData.ultimoErrore != gameData.caselleSbagliate[gameData.caselleSbagliate.Count - 1])
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
                ErrorAnimation(animatorErr);
                nuovoErrore = false;
            }
            casella.name = "Errore " + i;
        }
        TempData.ultimoErrore = "Errore " + gameData.caselleSbagliate[gameData.caselleSbagliate.Count - 1];
        for (int i = 0; i < gameData.stringValues.Count; i++)
        {
            SpriteRenderer colore = GameObject.Find(gameData.stringValues[i]).GetComponent<SpriteRenderer>();
            colore.color = Color.white;
        }
        if (gameData.stringValues.Count == 0)
        {
            Transform casellaTransform = baseScacchiera.transform.Find("Casella 0,0");
            if ( casellaTransform != null )
            {
                gameData.stringValues.Add("Casella 0,0");
                gameData.SaveData();
                GameObject casella = casellaTransform.gameObject;
                // Cambia colore usando il componente Renderer
                SpriteRenderer renderer = casella.GetComponent<SpriteRenderer>();

                // Verifica che il GameObject abbia un Renderer
                if (renderer != null)
                {
                    // Modifica il colore del materiale
                    renderer.color = Color.white;
                }
                Transform casellaSopraTransform = baseScacchiera.transform.Find("Casella 1,0");
                if (casellaSopraTransform != null )
                {
                    GameObject casellaSopra = casellaSopraTransform.gameObject;
                    SpriteRenderer rendererSopra = casellaSopra.GetComponent<SpriteRenderer>();
                    if (rendererSopra != null)
                    {
                        rendererSopra.color = new Color (255f, 255f, 0f, 255f);
                    }
                }
                Transform casellaDiagonaleTransform = baseScacchiera.transform.Find("Casella 1,1");
                if (casellaDiagonaleTransform != null )
                {
                    GameObject casellaDiagonale = casellaDiagonaleTransform.gameObject;
                    SpriteRenderer rendererDiagonale = casellaDiagonale.GetComponent<SpriteRenderer>();
                    if (rendererDiagonale != null)
                    {
                        rendererDiagonale.color = Color.red;
                    }
                }
                Transform casellaSinistraTransform = baseScacchiera.transform.Find("Casella 0,1");
                if ( casellaSinistraTransform != null )
                {
                    GameObject casellaSinistra = casellaSinistraTransform.gameObject;
                    SpriteRenderer rendererSinistra = casellaSinistra.GetComponent<SpriteRenderer>();
                    if (rendererSinistra != null)
                    {
                        rendererSinistra.color = Color.green;
                    }
                }
                Vector3 spawnPosition = casella.transform.position;
                player = Instantiate(player, spawnPosition, Quaternion.identity);
            }
        }
        else if (gameData.stringValues.Count == 1)
        {
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
                Transform casellaSopraTransform = baseScacchiera.transform.Find("Casella 1,0");
                GameObject casellaSopraErrataGameObject = GameObject.Find("Errore Casella 1,0");
                if (casellaSopraTransform != null && casellaSopraErrataGameObject == null)
                {
                    GameObject casellaSopra = casellaSopraTransform.gameObject;
                    SpriteRenderer rendererSopra = casellaSopra.GetComponent<SpriteRenderer>();
                    if (rendererSopra != null)
                    {
                        rendererSopra.color = new Color (255f, 255f, 0f, 255f);
                    }
                }
                Transform casellaDiagonaleTransform = baseScacchiera.transform.Find("Casella 1,1");
                GameObject casellaDiagonaleErrataGameObject = GameObject.Find("Errore Casella 1,1");
                if (casellaDiagonaleTransform != null && casellaDiagonaleErrataGameObject == null)
                {
                    GameObject casellaDiagonale = casellaDiagonaleTransform.gameObject;
                    SpriteRenderer rendererDiagonale = casellaDiagonale.GetComponent<SpriteRenderer>();
                    if (rendererDiagonale != null)
                    {
                        rendererDiagonale.color = Color.red;
                    }
                }
                Transform casellaSinistraTransform = baseScacchiera.transform.Find("Casella 0,1");
                GameObject casellaSinistraErrataGameObject = GameObject.Find("Errore Casella 0,1");
                if ( casellaSinistraTransform != null && casellaSinistraErrataGameObject == null )
                {
                    GameObject casellaSinistra = casellaSinistraTransform.gameObject;
                    SpriteRenderer rendererSinistra = casellaSinistra.GetComponent<SpriteRenderer>();
                    if (rendererSinistra != null)
                    {
                        rendererSinistra.color = Color.green;
                    }
                }
                Vector3 spawnPosition = casella.transform.position;
                player = Instantiate(player, spawnPosition, Quaternion.identity);
                if (casellaSopraErrataGameObject != null && casellaDiagonaleErrataGameObject != null && casellaSinistraErrataGameObject != null)
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
                    if(casellaSopra.color != Color.white)
                        casellaSopra.color = new Color(255f, 255f, 0f, 255f);
                }
                GameObject casellaDiagonaleGameObject = GameObject.Find(Utils.DiagonaleSinistra(ultimaCasellaString));
                GameObject casellaSbagliataDiagonale = GameObject.Find("Errore " + Utils.DiagonaleSinistra(ultimaCasellaString));
                if (casellaDiagonaleGameObject != null && casellaSbagliataDiagonale == null)
                {
                    SpriteRenderer casellaDiagonale = casellaDiagonaleGameObject.GetComponent<SpriteRenderer>();
                    if(casellaDiagonale.color != Color.white)
                        casellaDiagonale.color = Color.red;
                }
                GameObject casellaSinistraGameObject = GameObject.Find(Utils.Sinistra(ultimaCasellaString));
                GameObject casellaSbagliataSinistra = GameObject.Find("Errore " + Utils.Sinistra(ultimaCasellaString));
                if (casellaSinistraGameObject != null && casellaSbagliataSinistra == null)
                {
                    SpriteRenderer casellaSinistra = casellaSinistraGameObject.GetComponent<SpriteRenderer>();
                    if(casellaSinistra.color != Color.white)
                        casellaSinistra.color = Color.green;
                }
                GameObject casellaDiagonaleDestraGameObject = GameObject.Find(Utils.DiagonaleDestra(ultimaCasellaString));
                GameObject casellaSbagliataDiagonaleDestra = GameObject.Find("Errore " + Utils.DiagonaleDestra(ultimaCasellaString));
                if(casellaDiagonaleDestraGameObject != null && casellaSbagliataDiagonaleDestra == null)
                {
                    SpriteRenderer casellaDiagonaleDestra = casellaDiagonaleDestraGameObject.GetComponent<SpriteRenderer>();
                    if(casellaDiagonaleDestra.color != Color.white)
                        casellaDiagonaleDestra.color = Color.green;
                }
                GameObject casellaDestraGameObject = GameObject.Find(Utils.Destra(ultimaCasellaString));
                GameObject casellaSbagliataDestra = GameObject.Find("Errore " + Utils.Destra(ultimaCasellaString));
                if(casellaDestraGameObject != null && casellaSbagliataDestra == null)
                {
                    SpriteRenderer casellaDestra = casellaDestraGameObject.GetComponent<SpriteRenderer>();
                    if(casellaDestra.color != Color.white)
                        casellaDestra.color = Color.green;
                }
            }
            else
            {
                string ultimaCasellaString = gameData.stringValues[gameData.stringValues.Count - 1];
                Transform ultimaCasellaTransform = baseScacchiera.transform.Find(ultimaCasellaString);
                if(Utils.RigaSuperiore(ultimaCasellaString, gameData.caselleSbagliate))
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
                        if(casellaSopra.color != Color.white)
                            casellaSopra.color = new Color(255f, 255f, 0f, 255f);
                    }
                    GameObject casellaDiagonaleGameObject = GameObject.Find(Utils.DiagonaleSinistra(ultimaCasellaString));
                    GameObject casellaSbagliataDiagonale = GameObject.Find("Errore " + Utils.DiagonaleSinistra(ultimaCasellaString));
                    if (casellaDiagonaleGameObject != null && casellaSbagliataDiagonale == null)
                    {
                        SpriteRenderer casellaDiagonale = casellaDiagonaleGameObject.GetComponent<SpriteRenderer>();
                        if(casellaDiagonale.color != Color.white)
                            casellaDiagonale.color = Color.red;
                    }
                    GameObject casellaSinistraGameObject = GameObject.Find(Utils.Sinistra(ultimaCasellaString));
                    GameObject casellaSbagliataSinistra = GameObject.Find("Errore " + Utils.Sinistra(ultimaCasellaString));
                    if (casellaSinistraGameObject != null && casellaSbagliataSinistra == null)
                    {
                        SpriteRenderer casellaSinistra = casellaSinistraGameObject.GetComponent<SpriteRenderer>();
                        if(casellaSinistra.color != Color.white)
                            casellaSinistra.color = Color.green;
                    }
                    GameObject casellaDiagonaleDestraGameObject = GameObject.Find(Utils.DiagonaleDestra(ultimaCasellaString));
                    GameObject casellaSbagliataDiagonaleDestra = GameObject.Find("Errore " + Utils.DiagonaleDestra(ultimaCasellaString));
                    if(casellaDiagonaleDestraGameObject != null && casellaSbagliataDiagonaleDestra == null)
                    {
                        SpriteRenderer casellaDiagonaleDestra = casellaDiagonaleDestraGameObject.GetComponent<SpriteRenderer>();
                        if(casellaDiagonaleDestra.color != Color.white)
                            casellaDiagonaleDestra.color = Color.green;
                    }
                    GameObject casellaDestraGameObject = GameObject.Find(Utils.Destra(ultimaCasellaString));
                    GameObject casellaSbagliataDestra = GameObject.Find("Errore " + Utils.Destra(ultimaCasellaString));
                    if(casellaDiagonaleDestraGameObject != null && casellaSbagliataDestra == null)
                    {
                        SpriteRenderer casellaDestra = casellaDestraGameObject.GetComponent<SpriteRenderer>();
                        if(casellaDestra.color != Color.white)
                            casellaDestra.color = Color.green;
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

    private IEnumerator ErrorAnimation(Animator animator)
    {
        Debug.Log("ciao");
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
