using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;



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
                    if (
                        controlloMappa.weights.ContainsKey(gameObject.name)
                        &&
                        !controlloMappa.gameData.wrongBoxes.Contains(gameObject.name)
                        &&
                        !controlloMappa.gameData.correctBoxes.Contains(gameObject.name)
                      )
                    {
                        string boxName = gameObject.name;
                        if (TempData.casellaCliccata == boxName)
                        {
                            switch (controlloMappa.weights[boxName])
                            {
                                case 1:
                                    TempData.difficolta = "Domande difficili";
                                    break;
                                case 2:
                                    TempData.difficolta = "Domande medie";
                                    break;
                                case 3:
                                    TempData.difficolta = "Domande";
                                    break;
                            }
                            TempData.lastBox = boxName;
                            SceneManager.LoadScene("RispostaDomande");
                        }
                        else
                        {
                            Deactivate();
                            controlloMappa.textDifficultyOne.SetActive(controlloMappa.weights[boxName] == 3);
                            controlloMappa.textDifficultyTwo.SetActive(controlloMappa.weights[boxName] == 2);
                            controlloMappa.textDifficultyThree.SetActive(controlloMappa.weights[boxName] == 1);
                            TempData.casellaCliccata = boxName;
                        }
                    }
                    /*
                    var utilsActions = new Dictionary<Func<string, string>, int>
                    {
                        { Utils.Above, 0 },
                        { Utils.RightDiagonal, 1 },
                        { Utils.Destra, 2 },
                        { Utils.DiagonalRightBelow, 3 },
                        { Utils.Below, 4 },
                        { Utils.LeftDiagonalBelow, 5 },
                        { Utils.Sinistra, 6 },
                        { Utils.LeftDiagonal, 7 }
                    };
                    SpriteRenderer spriterenderer = GetComponent<SpriteRenderer> ();
                    colore = spriterenderer.color;
                    GameObject casellaSbagliata = GameObject.Find("Errore " + gameObject.name);
                    // check if it is an adjacent box and load the descriptions or start the clicked difficulty level
                    if(
                        casellaSbagliata == null
                        &&
                        (gameObject.name == controlloMappa.gameData.finishLine || colore != Color.white)
                      )
                    {
                        foreach(var utilsAction in utilsActions)
                        {
                            string lastBoxString = controlloMappa.gameData.correctBoxes[controlloMappa.gameData.correctBoxes.Count - 1];
                            if(gameObject.name == utilsAction.Key(lastBoxString))
                            {
                                if(gameObject.name == controlloMappa.gameData.finishLine)
                                {
                                    if(TempData.casellaCliccata == gameObject.name)
                                    {
                                        TempData.difficolta = "Domande finali";
                                        TempData.lastBox = gameObject.name;
                                        SceneManager.LoadScene("RispostaDomande");
                                    }
                                    else
                                    {
                                        Deactivate();
                                        controlloMappa.textDifficultyFinal.SetActive(true);
                                        TempData.casellaCliccata = gameObject.name;
                                    }
                                }
                                else
                                {
                                    if(TempData.casellaCliccata == gameObject.name)
                                    {
                                        switch (controlloMappa.weights[utilsAction.Value])
                                        {
                                            case 1:
                                                TempData.difficolta = "Domande difficili";
                                                break;
                                            case 2:
                                                TempData.difficolta = "Domande medie";
                                                break;
                                            case 3:
                                                TempData.difficolta = "Domande";
                                                break;
                                        }
                                        TempData.lastBox = gameObject.name;
                                        SceneManager.LoadScene("RispostaDomande");
                                    }
                                    else
                                    {
                                        Deactivate();
                                        controlloMappa.textDifficultyOne.SetActive(controlloMappa.weights[utilsAction.Value] == 3);
                                        controlloMappa.textDifficultyTwo.SetActive(controlloMappa.weights[utilsAction.Value] == 2);
                                        controlloMappa.textDifficultyThree.SetActive(controlloMappa.weights[utilsAction.Value] == 1);
                                        TempData.casellaCliccata = gameObject.name;
                                    }
                                }
                            }
                        }
                    }*/
                }
            }
        }
    }

    // disables all writing
    private void Deactivate()
    {
        controlloMappa.mainWriting.SetActive(false);
        controlloMappa.textDifficultyOne.SetActive(false);
        controlloMappa.textDifficultyTwo.SetActive(false);
        controlloMappa.textDifficultyThree.SetActive(false);
        controlloMappa.textDifficultyFinal.SetActive(false);
    }
}
