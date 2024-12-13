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
            List<string> caselleGiusteSbagliate = new List<string>();
            caselleGiusteSbagliate.AddRange(gameData.stringValues);
            caselleGiusteSbagliate.AddRange(gameData.caselleSbagliate);
            string ultimaCasellaString = gameData.stringValues[gameData.stringValues.Count - 1];
            Transform ultimaCasellaTransform = baseScacchiera.transform.Find(ultimaCasellaString);
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
            if(TempData.gioco && TempData.vittoria)
            {
                string penultimaCasellaString = gameData.stringValues[gameData.stringValues.Count - 2];
                Transform penultimaCasellaTransform = baseScacchiera.transform.Find(penultimaCasellaString);
                if (ultimaCasellaTransform != null && penultimaCasellaTransform != null)
                {
                    GameObject ultimaCasella = ultimaCasellaTransform.gameObject;
                    GameObject penultimaCasella = penultimaCasellaTransform.gameObject;
                    Vector3 spawnPosition = penultimaCasella.transform.position;
                    player = Instantiate(player, spawnPosition, Quaternion.identity);
                    StartCoroutine(MoveToTarget(spawnPosition, ultimaCasella.transform.position, moveDuration));
                }
            }
            else
            {
                if (ultimaCasellaTransform != null)
                {
                    GameObject ultimaCasella = ultimaCasellaTransform.gameObject;
                    Vector3 spawnPosition = ultimaCasella.transform.position;
                    player = Instantiate(player, spawnPosition, Quaternion.identity);
                }

                if(Utils.RigaSuperiore(ultimaCasellaString, caselleGiusteSbagliate))
                {
                    gameoverLogo.SetActive(true);
                    restart.SetActive(true);
                }
                else
                {
                    List<int> pesi = CondizioneGameOver(caselleGiusteSbagliate, ultimaCasellaString);
                    Debug.Log(
                       "sopra: " + pesi[0]
                       + "\n" +
                       "diagonaleDestra: " + pesi[1]
                       + "\n" +
                       "destra: " + pesi[2]
                       + "\n" +
                       "diagonaleSottoDestra: " + pesi[3]
                       + "\n" +
                       "sotto: " + pesi[4]
                       + "\n" +
                       "diagonaleSottoSinistra: " + pesi[5]
                       + "\n" +
                       "sinistra: " + pesi[6]
                       + "\n" +
                       "diagonaleSinistra: " + pesi[7]
                     );
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
        int[,] matrice = new int[6, 4];
        int [,] matricePercorsi = CalcoloDistanze(matrice, caselleGiusteSbagliate, 3, 5);
        int[] ultima = Utils.PrendiNumeri(ultimaCasella);
        int[] sopra = new int[2];
        int[] diagonaleDestra = new int[2];
        int[] destra = new int[2];
        int[] diagonaleSottoDestra = new int[2];
        int[] sotto = new int[2];
        int[] diagonaleSottoSinistra = new int[2];
        int[] sinistra = new int[2];
        int[] diagonaleSinistra = new int[2];
        if(ultima[0] <= 5 && Utils.Sopra(ultimaCasella) != "")
        {
            sopra = Utils.PrendiNumeri(Utils.Sopra(ultimaCasella));
        }
        if(ultima[0] <= 5 && ultima[1] >= 0 && Utils.DiagonaleDestra(ultimaCasella) != "")
        {
            diagonaleDestra = Utils.PrendiNumeri(Utils.DiagonaleDestra(ultimaCasella));
        }
        if(ultima[1] >= 0 && Utils.Destra(ultimaCasella) != "")
        {
            destra = Utils.PrendiNumeri(Utils.Destra(ultimaCasella));
        }
        if(ultima[0] >= 0 && ultima[1] >= 0 && Utils.DiagonaleSottoDestra(ultimaCasella) != "")
        {
            diagonaleSottoDestra = Utils.PrendiNumeri(Utils.DiagonaleSottoDestra(ultimaCasella));
        }
        if(ultima[0] >= 0 && Utils.Sotto(ultimaCasella) != "")
        {
            sotto = Utils.PrendiNumeri(Utils.Sotto(ultimaCasella));
        }
        if(ultima[0] >= 0 && ultima[1] <= 3 && Utils.DiagonaleSottoSinistra(ultimaCasella) != "")
        {
            diagonaleSottoSinistra = Utils.PrendiNumeri(Utils.DiagonaleSottoSinistra(ultimaCasella));
        }
        if(ultima[1] <= 3 && Utils.Sinistra(ultimaCasella)!= "")
        {
            sinistra = Utils.PrendiNumeri(Utils.Sinistra(ultimaCasella));
        }
        if(ultima[0] <= 5 && ultima[1] <= 3 && Utils.DiagonaleSinistra(ultimaCasella) != "")
        {
            diagonaleSinistra = Utils.PrendiNumeri(Utils.DiagonaleSinistra(ultimaCasella));
        }
        List<int> pesi = new List<int> 
        {
            matrice[sopra[0], sopra[1]],
            matrice[diagonaleDestra[0], diagonaleDestra[1]],
            matrice[destra[0], destra[1]],
            matrice[diagonaleSottoDestra[0], diagonaleSottoSinistra[1]],
            matrice[sotto[0], sotto[1]],
            matrice[diagonaleSottoSinistra[0], diagonaleSottoSinistra[1]],
            matrice[sinistra[0], sinistra[1]],
            matrice[diagonaleSinistra[0], diagonaleSinistra[1]]
        };
        return pesi;
    }

    int [,] CalcoloDistanze(int[,] matrice, List<string> caselleGiusteSbagliate, int valoreAttualeX, int valoreAttualeY)
    {
        if(valoreAttualeX == 3 && valoreAttualeY == 5)
        {
            for (int i = 0; i < matrice.GetLength(0); i++) // Scorre le righe
            {
                string riga = ""; // Accumula i valori della riga come stringa
                for (int j = 0; j < matrice.GetLength(1); j++) // Scorre le colonne
                {
                    riga += matrice[i, j].ToString("D2") + " "; // Formatta il valore con spaziatura
                }
                Debug.Log(riga); // Stampa la riga nella Console di Unity
            }
            matrice[5, 3] = 1;
            int[,] matriceUno = new int[6, 4];
            int[,] matriceDue = new int[6, 4];
            int[,] matriceTre = new int[6, 4];
            string sotto = Utils.CreaCasella(valoreAttualeX, valoreAttualeY - 1);
            if(!Utils.ControlloPresenza(caselleGiusteSbagliate, sotto))
            {
                matriceUno = CalcoloDistanze(matrice, caselleGiusteSbagliate, valoreAttualeX, valoreAttualeY - 1);
            }
            string destra = Utils.CreaCasella(valoreAttualeX - 1, valoreAttualeY);
            if(!Utils.ControlloPresenza(caselleGiusteSbagliate, destra))
            {
                matriceTre = CalcoloDistanze(matrice, caselleGiusteSbagliate, valoreAttualeX - 1, valoreAttualeY);
            }
            string diagonaleSotto = Utils.CreaCasella(valoreAttualeX - 1, valoreAttualeY - 1);
            if(!Utils.ControlloPresenza(caselleGiusteSbagliate, diagonaleSotto))
            {
                matriceDue = CalcoloDistanze(matrice, caselleGiusteSbagliate, valoreAttualeX - 1, valoreAttualeY - 1);
            }
            matrice[4, 3] = matriceUno[4, 3];
            matrice[4, 2] = matriceDue[4, 2];
            matrice[5, 2] = matriceTre[5, 2];
            return matrice;
        }
        else if(valoreAttualeX >= 0 && valoreAttualeY >= 0)
        {
            // Crea una stringa che includa tutto il contenuto
            string output = $"X: {valoreAttualeX}, Y: {valoreAttualeY}\nMatrice:\n";

            for (int i = 0; i < matrice.GetLength(0); i++) // Scorre le righe
            {
                for (int j = 0; j < matrice.GetLength(1); j++) // Scorre le colonne
                {
                    output += matrice[i, j].ToString("D2") + " "; // Aggiunge il valore formattato con spaziatura
                }
                output += "\n"; // Va a capo dopo ogni riga
            }

            // Stampa tutto in una singola chiamata a Debug.Log
            Debug.Log(output);

            if(valoreAttualeX < 3)
            {
                matrice[valoreAttualeY, valoreAttualeX] += matrice[valoreAttualeY, valoreAttualeX + 1];
            }
            if(valoreAttualeY < 5)
            {
                matrice[valoreAttualeY, valoreAttualeX] += matrice[valoreAttualeY + 1, valoreAttualeX];
            }
            if(valoreAttualeX < 3 && valoreAttualeY < 5)
            {
                matrice[valoreAttualeY, valoreAttualeX] += matrice[valoreAttualeY + 1, valoreAttualeX + 1];
            }
            int[,] matriceUno = new int[6, 4];
            int[,] matriceDue = new int[6, 4];
            int[,] matriceTre = new int[6, 4];
            string sotto = Utils.CreaCasella(valoreAttualeX, valoreAttualeY - 1);
            if(!Utils.ControlloPresenza(caselleGiusteSbagliate, sotto) && valoreAttualeY != 0)
            {
                matriceUno = CalcoloDistanze(matrice, caselleGiusteSbagliate, valoreAttualeX, valoreAttualeY - 1);
                matrice[valoreAttualeY - 1, valoreAttualeX] = matriceUno[valoreAttualeY - 1, valoreAttualeX];
            }
            string destra = Utils.CreaCasella(valoreAttualeX - 1, valoreAttualeY);
            if(!Utils.ControlloPresenza(caselleGiusteSbagliate, destra) && valoreAttualeX != 0)
            {
                matriceTre = CalcoloDistanze(matrice, caselleGiusteSbagliate, valoreAttualeX - 1, valoreAttualeY);
                matrice[valoreAttualeY, valoreAttualeX - 1] = matriceTre[valoreAttualeY, valoreAttualeX - 1];
            }
            string diagonaleSotto = Utils.CreaCasella(valoreAttualeX - 1, valoreAttualeY - 1);
            if(!Utils.ControlloPresenza(caselleGiusteSbagliate, diagonaleSotto) && valoreAttualeX != 0 && valoreAttualeY != 0)
            {
                matriceDue = CalcoloDistanze(matrice, caselleGiusteSbagliate, valoreAttualeX - 1, valoreAttualeY - 1);
                matrice[valoreAttualeY - 1, valoreAttualeX - 1] = matriceDue[valoreAttualeY - 1, valoreAttualeX - 1];
            }
            return matrice;
        }
        else
        {
            return matrice;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
