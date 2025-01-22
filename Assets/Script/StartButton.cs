using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public ControlloMappa controlloMappa;
    public void PlayGame()
    {
        controlloMappa.mainWriting.SetActive(true);

        // create a random start box
        string[] possibleChars = { "3", "0" };
        int randomIndexstart = UnityEngine.Random.Range(0, possibleChars.Length);
        controlloMappa.gameData.start = "Casella " + "0," + possibleChars[randomIndexstart];
        controlloMappa.gameData.correctBoxes.Add(controlloMappa.gameData.start);
        Transform startBox = controlloMappa.chessboardBase.transform.Find(controlloMappa.gameData.start);
        if (startBox != null)
        {
            UnityEngine.Vector3 spawnPos = startBox.transform.position;
            controlloMappa.player = Instantiate(controlloMappa.player, spawnPos, UnityEngine.Quaternion.identity);
        }

        // create a random flag line
        string randomIndex = possibleChars[randomIndexstart] == "3" ? "0" : possibleChars[randomIndexstart] == "0" ? "3" : possibleChars[randomIndexstart];
        controlloMappa.gameData.finishLine = "Casella " + "5," + randomIndex;
        Transform finishLine = controlloMappa.chessboardBase.transform.Find(controlloMappa.gameData.finishLine);
        if (finishLine != null)
        {
            UnityEngine.Vector3 spawnPos = finishLine.transform.position;
            controlloMappa.finishLineFlag = Instantiate(controlloMappa.finishLineFlag, spawnPos, UnityEngine.Quaternion.identity);
        }

        // inizialize the first bfs
        List<string> singleElementList = new List<string> { controlloMappa.gameData.start };
        controlloMappa.weights = controlloMappa.ConditionGameOver(singleElementList, controlloMappa.gameData.start, controlloMappa.gameData.finishLine);
        controlloMappa.gameData.lastLose[2] = Utils.TransformListIntToString(controlloMappa.weights);
        controlloMappa.gameData.SaveData();

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
        gameObject.SetActive(false);
        controlloMappa.initialWriting.SetActive(false);
        controlloMappa.chessboardBase.SetActive(true);
        controlloMappa.gameData.SaveData();
    }
}
