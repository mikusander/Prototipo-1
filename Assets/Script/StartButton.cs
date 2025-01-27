using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StartButton : MonoBehaviour
{
    public ControlloMappa controlloMappa;
    public void PlayGame()
    {
        controlloMappa.mainWriting.SetActive(true);

        // create a random start box
        System.Random random = new System.Random();
        int randomIndexstart = random.Next(2);
        controlloMappa.gameData.start = randomIndexstart;
        controlloMappa.gameData.SaveData();
        controlloMappa.gameData.correctBoxes.Add("Casella 0");
        UnityEngine.Vector3 spawnPos = controlloMappa.initialPosition[randomIndexstart];
        controlloMappa.player = Instantiate(controlloMappa.player, spawnPos, UnityEngine.Quaternion.identity);

        // create a random flag line
        int randomIndex = randomIndexstart == 0 ? 2 : 3;
        spawnPos = controlloMappa.initialPosition[randomIndex];
        controlloMappa.finishLineFlag = Instantiate(controlloMappa.finishLineFlag, spawnPos, UnityEngine.Quaternion.identity);

        // Adding start and end boxes to the adjacency list
        if (randomIndexstart == 0)
        {
            controlloMappa.adjacencyList["Casella 0"] = new List<string> { "Casella 1", "Casella 2", "Casella 3" };
            controlloMappa.adjacencyList["Casella 24"] = new List<string> { "Casella 19", "Casella 22", "Casella 23" };
        }
        else
        {
            controlloMappa.adjacencyList["Casella 0"] = new List<string> { "Casella 3", "Casella 4", "Casella 5" };
            controlloMappa.adjacencyList["Casella 24"] = new List<string> { "Casella 17", "Casella 21", "Casella 22" };
        }

        // inizialize the first bfs
        controlloMappa.weights = controlloMappa.CalculateDistances(controlloMappa.adjacencyList, new List<string>(), "Casella 24", "Casella 0");
        controlloMappa.gameData.lastLose[2] = Utils.TransformDictionaryToString(controlloMappa.weights);
        controlloMappa.gameData.SaveData();

        foreach (string key in controlloMappa.weights.Keys)
        {
            GameObject box = controlloMappa.chessboardBase.transform.Find(key).gameObject;
            if (box != null)
            {
                SpriteRenderer renderer = box.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    switch (controlloMappa.weights[key])
                    {
                        case 1:
                            renderer.color = Color.red;
                            Instantiate(controlloMappa.difficultyThree, box.transform.position, Quaternion.identity);
                            break;
                        case 2:
                            renderer.color = new Color(255f, 255f, 0f, 255f);
                            Instantiate(controlloMappa.difficultyTwo, box.transform.position, Quaternion.identity);
                            break;
                        case 3:
                            renderer.color = Color.green;
                            Instantiate(controlloMappa.difficultyOne, box.transform.position, Quaternion.identity);
                            break;
                    }
                }
            }
        }

        /*
        // search the start box
        Transform casellaTransform = controlloMappa.chessboardBase.transform.Find(controlloMappa.gameData.start);
        if (casellaTransform != null)
        {
            GameObject casella = casellaTransform.gameObject;
            SpriteRenderer renderer = casella.GetComponent<SpriteRenderer>();

            // change the color of the first box
            if (renderer != null)
            {
                renderer.color = Color.white;
            }

            // assign the color and the number of difficulty at the box above
            GameObject boxAboveGameObject = controlloMappa.chessboardBase.transform.Find(Utils.Above(casella.name)).gameObject;
            if (boxAboveGameObject != null)
            {
                SpriteRenderer boxAbove = boxAboveGameObject.GetComponent<SpriteRenderer>();
                if (controlloMappa.weights[0] == 1)
                {
                    boxAbove.color = Color.red;
                    UnityEngine.Vector3 spawnPos = boxAboveGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if (controlloMappa.weights[0] == 2)
                {
                    boxAbove.color = new Color(255f, 255f, 0f, 255f);
                    UnityEngine.Vector3 spawnPos = boxAboveGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if (controlloMappa.weights[0] == 3)
                {
                    boxAbove.color = Color.green;
                    UnityEngine.Vector3 spawnPos = boxAboveGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                }
            }

            // assign the color and the number of difficulty at the left diagonal box superior
            GameObject leftDiagonalBoxGameObject = controlloMappa.chessboardBase.transform.Find(Utils.LeftDiagonal(casella.name)).gameObject;
            if (leftDiagonalBoxGameObject != null)
            {
                SpriteRenderer diagonalBox = leftDiagonalBoxGameObject.GetComponent<SpriteRenderer>();
                if (controlloMappa.weights[7] == 1)
                {
                    diagonalBox.color = Color.red;
                    UnityEngine.Vector3 spawnPos = leftDiagonalBoxGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if (controlloMappa.weights[7] == 2)
                {
                    diagonalBox.color = new Color(255f, 255f, 0f, 255f);
                    UnityEngine.Vector3 spawnPos = leftDiagonalBoxGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if (controlloMappa.weights[7] == 3)
                {
                    diagonalBox.color = Color.green;
                    UnityEngine.Vector3 spawnPos = leftDiagonalBoxGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                }
            }

            // assign the color and the number at the difficulty 
            GameObject leftBoxGameObject = controlloMappa.chessboardBase.transform.Find(Utils.Sinistra(casella.name)).gameObject;
            if (leftBoxGameObject != null)
            {
                SpriteRenderer leftBox = leftBoxGameObject.GetComponent<SpriteRenderer>();
                if (controlloMappa.weights[6] == 1)
                {
                    leftBox.color = Color.red;
                    UnityEngine.Vector3 spawnPos = leftBoxGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if (controlloMappa.weights[6] == 2)
                {
                    leftBox.color = new Color(255f, 255f, 0f, 255f);
                    UnityEngine.Vector3 spawnPos = leftBoxGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if (controlloMappa.weights[6] == 3)
                {
                    leftBox.color = Color.green;
                    UnityEngine.Vector3 spawnPos = leftBoxGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                }
            }

            // assign the color and the number of the dissiculty at the right diagonal box
            GameObject rightDiagonalBoxGameObject = controlloMappa.chessboardBase.transform.Find(Utils.RightDiagonal(casella.name)).gameObject;
            if (rightDiagonalBoxGameObject != null)
            {
                SpriteRenderer rightDiagonalBox = rightDiagonalBoxGameObject.GetComponent<SpriteRenderer>();
                if (controlloMappa.weights[1] == 1)
                {
                    rightDiagonalBox.color = Color.red;
                    UnityEngine.Vector3 spawnPos = rightDiagonalBoxGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if (controlloMappa.weights[1] == 2)
                {
                    rightDiagonalBox.color = new Color(255f, 255f, 0f, 255f);
                    UnityEngine.Vector3 spawnPos = rightDiagonalBoxGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if (controlloMappa.weights[1] == 3)
                {
                    rightDiagonalBox.color = Color.green;
                    UnityEngine.Vector3 spawnPos = rightDiagonalBoxGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                }
            }

            // assign the color and the number of the difficulty at the the right box
            GameObject rightBoxGameObject = controlloMappa.chessboardBase.transform.Find(Utils.Destra(casella.name)).gameObject;
            if (rightDiagonalBoxGameObject != null)
            {
                SpriteRenderer rightBox = rightBoxGameObject.GetComponent<SpriteRenderer>();
                if (controlloMappa.weights[2] == 1)
                {
                    rightBox.color = Color.red;
                    UnityEngine.Vector3 spawnPos = rightBoxGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if (controlloMappa.weights[2] == 2)
                {
                    rightBox.color = new Color(255f, 255f, 0f, 255f);
                    UnityEngine.Vector3 spawnPos = rightBoxGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if (controlloMappa.weights[2] == 3)
                {
                    rightBox.color = Color.green;
                    UnityEngine.Vector3 spawnPos = rightBoxGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                }
            }
        }
        */
        gameObject.SetActive(false);
        controlloMappa.initialWriting.SetActive(false);
        controlloMappa.chessboardBase.SetActive(true);
        controlloMappa.gameData.SaveData();
    }
}
