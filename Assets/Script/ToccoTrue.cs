using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToccoFrecciaSu : MonoBehaviour
{
    [SerializeField] private Controllo controllo;
    private GameObject cartaDomanda;
    [SerializeField] private GameObject ditoInSu;
    [SerializeField] private GameObject ditoInGiu;
    private Vector3 spawnPosition = new Vector3(0, 1, 0);
    private float destroyDelay = 2f;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check if we clicked on the GameObject via a Raycast
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject && !controllo.inCreazione)
            {
                if (controllo.currentQuestion.correctAnswer)
                {
                    controllo.puntiAttuali += 1;
                    // Find the "Domanda" Canvas
                    GameObject canvasDomanda = GameObject.Find("Domanda(Clone)");

                    // Check if the Canvas was found
                    if (canvasDomanda != null)
                    {
                        // Search for the "Carta Domanda" GameObject inside the "Domanda" Canvas
                        Transform cartaTransform = canvasDomanda.transform.Find("CartaDomanda");

                        // Check if the "CartaDomanda" was found
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
                    // Find the canvas "Domanda"
                    GameObject canvasDomanda = GameObject.Find("Domanda(Clone)");

                    // Check if the canvas was found
                    if (canvasDomanda != null)
                    {
                        // Search for the "CartaDomanda" GameObject inside the "Domanda"
                        Transform cartaTransform = canvasDomanda.transform.Find("CartaDomanda");

                        // Check if the "CartaDomanda" was found
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
        // Start the animation
        TempData.animazione = true;
        animator.SetTrigger("Attivazione");

        // Get the active state duration
        AnimatorStateInfo animationInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = animationInfo.length;

        // Wait for the duration of the animation
        float dura = 0.6f;
        yield return new WaitForSeconds(dura);
        TempData.animazione = false;
    }
}
