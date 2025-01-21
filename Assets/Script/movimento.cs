using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class DragGameObject2D : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    [SerializeField] private GameObject yesRect;
    [SerializeField] private GameObject noRect;
    private Vector3 inizialPosition;
    private Vector3 yesPosition = new Vector3(4.5f, 0f, 0f);
    private Vector3 noPosition = new Vector3(-1f, 0f, 0f);
    private float destroyDelay = 2f;
    [SerializeField] private GameObject ditoInSu;
    [SerializeField] private GameObject ditoInGiu;
    private Vector3 spawnPosition = new Vector3(0, 1, 0);
    [SerializeField] private GameObject canva;
    [SerializeField] private Text testo;
    private Controllo controllo;

    void Start()
    {
        controllo = GameObject.Find("GameController").GetComponent<Controllo>();
        testo.text = controllo.currentQuestion.text;

        inizialPosition = transform.position;
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (controllo.gameover)
        {
            Destroy(canva);
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                offset = transform.position - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (transform.position.x < noPosition.x)
            {
                if (controllo.currentQuestion.correctAnswer)
                {
                    controllo.puntiAttuali += 1;
                    controllo.isUltima = false;
                    Destroy(canva);
                    GameObject instance = Instantiate(ditoInSu, spawnPosition, Quaternion.identity);
                    Animator animator = instance.GetComponent<Animator>();
                    controllo.StartCoroutine(AnimationMano(animator));
                    Destroy(instance, destroyDelay);
                }
                else
                {
                    controllo.isUltima = false;
                    Destroy(canva);
                    GameObject instance = Instantiate(ditoInGiu, spawnPosition, Quaternion.identity);
                    Animator animator = instance.GetComponent<Animator>();
                    controllo.StartCoroutine(AnimationMano(animator));
                    Destroy(instance, destroyDelay);
                }
            }
            else if (transform.position.x > yesPosition.x)
            {
                if (controllo.currentQuestion.correctAnswer)
                {
                    controllo.isUltima = false;
                    Destroy(canva);
                    GameObject instance = Instantiate(ditoInGiu, spawnPosition, Quaternion.identity);
                    Animator animator = instance.GetComponent<Animator>();
                    controllo.StartCoroutine(AnimationMano(animator));
                    Destroy(instance, destroyDelay);
                }
                else
                {
                    controllo.isUltima = false;
                    controllo.puntiAttuali += 1;
                    Destroy(canva);
                    GameObject instance = Instantiate(ditoInSu, spawnPosition, Quaternion.identity);
                    Animator animator = instance.GetComponent<Animator>();
                    controllo.StartCoroutine(AnimationMano(animator));
                    Destroy(instance, destroyDelay);
                }
            }
            else
            {
                transform.position = inizialPosition;
            }
            isDragging = false;
        }

        if (transform.position.x > inizialPosition.x && isDragging)
        {
            yesRect.SetActive(false);
            noRect.SetActive(true);
        }
        else if (transform.position.x < inizialPosition.x)
        {
            noRect.SetActive(false);
            yesRect.SetActive(true);
        }

        if (isDragging)
        {
            // Update the position of the GameObject as we drag
            Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition) + offset;
            newPosition.z = 0;  // Manteniamo la posizione Z costante in 2D
            newPosition.y = transform.position.y;
            transform.position = newPosition;
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
