using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LastLevel : MonoBehaviour
{

    private ControlloMappa controlloMappa;

    // Start is called before the first frame update
    void Start()
    {
        controlloMappa = GameObject.Find("GameController").GetComponent<ControlloMappa>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button was clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Get mouse position in world coordinates
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Launch a 2D raycast from the mouse position
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // Check if the raycast hit anything
            if (hit.collider != null)
            {
                // Check if the hit GameObject is the same one this script is attached to
                if (hit.collider.gameObject == gameObject)
                {
                    if (TempData.casellaCliccata == gameObject.name)
                    {
                        TempData.lastBox = gameObject.name;
                        TempData.difficolta = "Final";
                        SceneManager.LoadScene("NuovoGameplay");
                    }
                    else
                    {
                        controlloMappa.Deactivate();
                        controlloMappa.textDifficultyFinal.SetActive(true);
                        TempData.casellaCliccata = gameObject.name;
                    }
                }
            }
        }
    }
}
