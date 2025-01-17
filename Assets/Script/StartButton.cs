using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public ControlloMappa controlloMappa;
    public void PlayGame()
    {
        controlloMappa.mainWriting.SetActive(true);
        string[] possibleChars = { "1", "2", "3", "0" };
        int randomIndex = UnityEngine.Random.Range(0, possibleChars.Length);
        controlloMappa.gameData.finishLine = "Casella " + "5," + possibleChars[randomIndex];
        Transform casellaFinale = controlloMappa.chessboardBase.transform.Find(controlloMappa.gameData.finishLine);
        if (casellaFinale != null)
        {
            UnityEngine.Vector3 spawnPos = casellaFinale.transform.position;
            controlloMappa.finishLineFlag = Instantiate(controlloMappa.finishLineFlag, spawnPos, UnityEngine.Quaternion.identity);
        }
        int randomIndexstart = UnityEngine.Random.Range(0, possibleChars.Length);
        controlloMappa.gameData.start = "Casella " + "0," + possibleChars[randomIndexstart];
        controlloMappa.gameData.correctBoxes.Add(controlloMappa.gameData.start);
        Transform startBox = controlloMappa.chessboardBase.transform.Find(controlloMappa.gameData.start);
        if (startBox != null)
        {
            UnityEngine.Vector3 spawnPos = startBox.transform.position;
            controlloMappa.player = Instantiate(controlloMappa.player, spawnPos, UnityEngine.Quaternion.identity);
        }
        List<string> singleElementList = new List<string> { controlloMappa.gameData.start };

        controlloMappa.weights = controlloMappa.ConditionGameOver(singleElementList, controlloMappa.gameData.start, controlloMappa.gameData.finishLine);
        controlloMappa.gameData.lastLose[2] = Utils.TransformListIntToString(controlloMappa.weights);
        controlloMappa.gameData.SaveData();

        Transform casellaTransform = controlloMappa.chessboardBase.transform.Find(controlloMappa.gameData.start);
        if (casellaTransform != null)
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
            GameObject boxAboveGameObject = controlloMappa.chessboardBase.transform.Find(Utils.Above(casella.name)).gameObject;
            if (boxAboveGameObject != null)
            {
                SpriteRenderer boxAbove = boxAboveGameObject.GetComponent<SpriteRenderer>();
                if(controlloMappa.weights[0] == 1)
                {
                    boxAbove.color = Color.red;
                    UnityEngine.Vector3 spawnPos = boxAboveGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if(controlloMappa.weights[0] == 2)
                {
                    boxAbove.color = new Color(255f, 255f, 0f, 255f);
                    UnityEngine.Vector3 spawnPos = boxAboveGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if(controlloMappa.weights[0] == 3)
                {
                    boxAbove.color = Color.green;
                    UnityEngine.Vector3 spawnPos = boxAboveGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                }
            }
            GameObject casellaDiagonaleGameObject = controlloMappa.chessboardBase.transform.Find(Utils.LeftDiagonal(casella.name)).gameObject;
            if (casellaDiagonaleGameObject != null)
            {
                SpriteRenderer casellaDiagonale = casellaDiagonaleGameObject.GetComponent<SpriteRenderer>();
                if(controlloMappa.weights[7] == 1)
                {
                    casellaDiagonale.color = Color.red;
                    UnityEngine.Vector3 spawnPos = casellaDiagonaleGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if(controlloMappa.weights[7] == 2)
                {
                    casellaDiagonale.color = new Color(255f, 255f, 0f, 255f);
                    UnityEngine.Vector3 spawnPos = casellaDiagonaleGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if(controlloMappa.weights[7] == 3)
                {
                    casellaDiagonale.color = Color.green;
                    UnityEngine.Vector3 spawnPos = casellaDiagonaleGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                }
            }
            GameObject casellaSinistraGameObject = controlloMappa.chessboardBase.transform.Find(Utils.Sinistra(casella.name)).gameObject;
            if (casellaSinistraGameObject != null)
            {
                SpriteRenderer casellaSinistra = casellaSinistraGameObject.GetComponent<SpriteRenderer>();
                if(controlloMappa.weights[6] == 1)
                {
                    casellaSinistra.color = Color.red;
                    UnityEngine.Vector3 spawnPos = casellaSinistraGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if(controlloMappa.weights[6] == 2)
                {
                    casellaSinistra.color = new Color(255f, 255f, 0f, 255f);
                    UnityEngine.Vector3 spawnPos = casellaSinistraGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if(controlloMappa.weights[6] == 3)
                {
                    casellaSinistra.color = Color.green;
                    UnityEngine.Vector3 spawnPos = casellaSinistraGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                }
            }
            GameObject rightDiagonalBoxGameObject = controlloMappa.chessboardBase.transform.Find(Utils.rightDiagonal(casella.name)).gameObject;
            if(rightDiagonalBoxGameObject != null)
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
            GameObject casellaDestraGameObject = controlloMappa.chessboardBase.transform.Find(Utils.Destra(casella.name)).gameObject;
            if (rightDiagonalBoxGameObject != null)
            {
                SpriteRenderer casellaDestra = casellaDestraGameObject.GetComponent<SpriteRenderer>();
                if (controlloMappa.weights[2] == 1)
                {
                    casellaDestra.color = Color.red;
                    UnityEngine.Vector3 spawnPos = casellaDestraGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if (controlloMappa.weights[2] == 2)
                {
                    casellaDestra.color = new Color(255f, 255f, 0f, 255f);
                    UnityEngine.Vector3 spawnPos = casellaDestraGameObject.transform.position;
                    Instantiate(controlloMappa.difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if (controlloMappa.weights[2] == 3)
                {
                    casellaDestra.color = Color.green;
                    UnityEngine.Vector3 spawnPos = casellaDestraGameObject.transform.position;
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
