using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToccoFrecciaSu : MonoBehaviour
{
    public Controllo controllo;
    private GameObject cartaDomanda;
    public GameObject ditoInSu;
    public GameObject ditoInGiu;
    private Vector3 spawnPosition = new Vector3(0, 1, 0);
    public float destroyDelay = 2f;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;  // Riferimento alla telecamera principale
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Verifica se abbiamo cliccato sul GameObject tramite un Raycast
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject && !controllo.inCreazione)
            {
                if (controllo.currentQuestion.correctAnswer)
                {
                    controllo.puntiAttuali += 1;
                    // Trova il Canvas "Domanda"
                    GameObject canvasDomanda = GameObject.Find("Domanda(Clone)");

                    // Verifica se il Canvas � stato trovato
                    if (canvasDomanda != null)
                    {
                        // Cerca il GameObject "CartaDomanda" all'interno del Canvas "Domanda"
                        Transform cartaTransform = canvasDomanda.transform.Find("CartaDomanda");

                        // Verifica se "CartaDomanda" è stato trovato
                        if (cartaTransform != null)
                        {
                            cartaDomanda = cartaTransform.gameObject;

                            controllo.isUltima = false;
                            Destroy(canvasDomanda);
                            GameObject instance = Instantiate(ditoInSu, spawnPosition, Quaternion.identity);
                            Animator animator = instance.GetComponent<Animator>();
                            StartCoroutine(AnimationMano(animator));
                            Destroy(instance, destroyDelay);
                        }
                        else
                        {
                            Debug.LogWarning("CartaDomanda non trovata nel Canvas Domanda.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Canvas Domanda non trovato nella scena.");
                    }
                }
                else
                {
                    // Trova il Canvas "Domanda"
                    GameObject canvasDomanda = GameObject.Find("Domanda(Clone)");

                    // Verifica se il Canvas � stato trovato
                    if (canvasDomanda != null)
                    {
                        // Cerca il GameObject "CartaDomanda" all'interno del Canvas "Domanda"
                        Transform cartaTransform = canvasDomanda.transform.Find("CartaDomanda");

                        // Verifica se "CartaDomanda" è stato trovato
                        if (cartaTransform != null)
                        {
                            cartaDomanda = cartaTransform.gameObject;

                            controllo.isUltima = false;
                            Destroy(canvasDomanda);
                            GameObject instance = Instantiate(ditoInGiu, spawnPosition, Quaternion.identity);
                            Animator animator = instance.GetComponent<Animator>();
                            StartCoroutine(AnimationMano(animator));
                            Destroy(instance, destroyDelay);
                        }
                        else
                        {
                            Debug.LogWarning("CartaDomanda non trovata nel Canvas Domanda.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Canvas Domanda non trovato nella scena.");
                    }
                }
            }
        }
    }
    IEnumerator AnimationMano(Animator animator)
    {
        // Avvia l'animazione
        TempData.animazione = true;
        animator.SetTrigger("Attivazione");

        // Ottieni la durata dello stato attivo
        AnimatorStateInfo animationInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = animationInfo.length;

        // Aspetta la durata dell'animazione
        yield return new WaitForSeconds(animationDuration / 5 * 2);
        TempData.animazione = false;
        yield return new WaitForSeconds(animationDuration / 5 * 3);

        animator.SetTrigger("Disattivazione");
    }
}
