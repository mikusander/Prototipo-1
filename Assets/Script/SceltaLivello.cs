using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using Unity.Mathematics;



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
        if (Input.GetMouseButtonDown(0) && !controlloMappa.gameOver)
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
                        controlloMappa.adjacencyList[controlloMappa.gameData.correctBoxes[controlloMappa.gameData.correctBoxes.Count - 1]].Contains(gameObject.name)
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
                                    TempData.difficolta = "Hard";
                                    break;
                                case 2:
                                    TempData.difficolta = "Medium";
                                    break;
                                case 3:
                                    TempData.difficolta = "Easy";
                                    break;
                            }
                            TempData.casellaCliccata = "";
                            TempData.lastBox = boxName;
                            SceneManager.LoadScene("NuovoGameplay");
                        }
                        else
                        {
                            DestroyChild(gameObject.name);
                            GameObject play = Instantiate(controlloMappa.playButton, gameObject.transform.position, Quaternion.identity);
                            Vector3 worldPosition = play.transform.position;
                            play.transform.SetParent(gameObject.transform);
                            play.transform.position = worldPosition;

                            if (TempData.casellaCliccata != "")
                            {
                                GameObject previousBox = GameObject.Find(TempData.casellaCliccata);
                                DestroyChild(TempData.casellaCliccata);
                                switch (controlloMappa.weights[TempData.casellaCliccata])
                                {
                                    case 1:
                                        GameObject appoDifficultyThree = Instantiate(controlloMappa.difficultyThree, previousBox.transform.position, Quaternion.identity);
                                        worldPosition = appoDifficultyThree.transform.position;
                                        appoDifficultyThree.transform.SetParent(previousBox.transform);
                                        appoDifficultyThree.transform.position = worldPosition;
                                        break;
                                    case 2:
                                        GameObject appoDifficultyTwo = Instantiate(controlloMappa.difficultyTwo, previousBox.transform.position, quaternion.identity);
                                        worldPosition = appoDifficultyTwo.transform.position;
                                        appoDifficultyTwo.transform.SetParent(previousBox.transform);
                                        appoDifficultyTwo.transform.position = worldPosition;
                                        break;
                                    case 3:
                                        GameObject appoDifficultyOne = Instantiate(controlloMappa.difficultyOne, previousBox.transform.position, quaternion.identity);
                                        worldPosition = appoDifficultyOne.transform.position;
                                        appoDifficultyOne.transform.SetParent(previousBox.transform);
                                        appoDifficultyOne.transform.position = worldPosition;
                                        break;
                                }
                            }

                            controlloMappa.Deactivate();
                            controlloMappa.textDifficultyOne.SetActive(controlloMappa.weights[boxName] == 3);
                            controlloMappa.textDifficultyTwo.SetActive(controlloMappa.weights[boxName] == 2);
                            controlloMappa.textDifficultyThree.SetActive(controlloMappa.weights[boxName] == 1);
                            TempData.casellaCliccata = boxName;
                        }
                    }
                }
            }
        }
    }

    private void DestroyChild(string nameGameObject)
    {
        Transform parentObject = GameObject.Find(nameGameObject).transform;
        foreach (Transform child in parentObject)
        {
            Destroy(child.gameObject);
        }
    }
}
