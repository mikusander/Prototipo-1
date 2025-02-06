using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;

public class StartButton : MonoBehaviour
{
    public ControlloMappa controlloMappa;
    private System.Random random = new System.Random();
    public void PlayGame()
    {
        controlloMappa.mainWriting.SetActive(true);

        // create a random start box
        System.Random random = new System.Random();
        int randomIndexstart = random.Next(2);
        controlloMappa.gameData.start = randomIndexstart;
        controlloMappa.gameData.SaveData();
        controlloMappa.gameData.correctBoxes.Add("Casella 0");

        // Adding start and end boxes to the adjacency list
        if (randomIndexstart == 0)
        {
            // assign the adjiacent node to a start and finish node
            controlloMappa.adjacencyList["Casella 0"] = new List<string> { "Casella 1", "Casella 26" };
            controlloMappa.adjacencyList["Casella 24"] = new List<string> { "Casella 19", "Casella 22", "Casella 23" };

            // spawn the player and the finishLine logo
            controlloMappa.player = Instantiate(controlloMappa.player, new Vector3(2f, -1.8f, 0f), Quaternion.identity);
            Instantiate(controlloMappa.finishLineFlag, new Vector3(-1f, 4f, 0f), Quaternion.identity);
            controlloMappa.twentySevenBox = Instantiate(controlloMappa.twentySevenBox, new Vector3(1f, 4.1f, 0f), Quaternion.identity);
            controlloMappa.twentySevenBox.transform.SetParent(controlloMappa.chessboardBase.transform);
            controlloMappa.twentySevenBox.name = "Casella 27";
            controlloMappa.adjacencyList[controlloMappa.twentySevenBox.name] = new List<string> { "Casella 17", "Casella 21", "Casella 22" };
            controlloMappa.adjacencyList["Casella 17"].Add("Casella 27");
            controlloMappa.adjacencyList["Casella 21"].Add("Casella 27");
            controlloMappa.adjacencyList["Casella 22"].Add("Casella 27");
        }
        else
        {
            // assign the adjiacent node to a start and finish node
            controlloMappa.adjacencyList["Casella 0"] = new List<string> { "Casella 5", "Casella 25" };
            controlloMappa.adjacencyList["Casella 24"] = new List<string> { "Casella 17", "Casella 21", "Casella 22" };

            // spawn the player and the finishLine logo
            controlloMappa.player = Instantiate(controlloMappa.player, new Vector3(-2f, -1.8f, 0f), Quaternion.identity);
            Instantiate(controlloMappa.finishLineFlag, new Vector3(1f, 4f, 0f), Quaternion.identity);
            controlloMappa.twentySevenBox = Instantiate(controlloMappa.twentySevenBox, new Vector3(-1f, 4.1f, 0f), Quaternion.identity);
            controlloMappa.twentySevenBox.transform.SetParent(controlloMappa.chessboardBase.transform);
            controlloMappa.twentySevenBox.name = "Casella 27";
            controlloMappa.adjacencyList[controlloMappa.twentySevenBox.name] = new List<string> { "Casella 19", "Casella 22", "Casella 23" };
            controlloMappa.adjacencyList["Casella 19"].Add("Casella 27");
            controlloMappa.adjacencyList["Casella 22"].Add("Casella 27");
            controlloMappa.adjacencyList["Casella 23"].Add("Casella 27");
        }

        // inizialize the first bfs
        controlloMappa.weights = new Dictionary<string, int>();
        InitialWeights();
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
                    GameObject boxNumber;
                    switch (controlloMappa.weights[key])
                    {
                        case 1:
                            renderer.color = Color.red;
                            boxNumber = Instantiate(controlloMappa.difficultyThree, box.transform.position, Quaternion.identity);
                            boxNumber.transform.SetParent(box.transform);
                            break;
                        case 2:
                            renderer.color = new Color(255f, 255f, 0f, 255f);
                            boxNumber = Instantiate(controlloMappa.difficultyTwo, box.transform.position, Quaternion.identity);
                            boxNumber.transform.SetParent(box.transform);
                            break;
                        case 3:
                            renderer.color = Color.green;
                            boxNumber = Instantiate(controlloMappa.difficultyOne, box.transform.position, Quaternion.identity);
                            boxNumber.transform.SetParent(box.transform);
                            break;
                    }
                }
            }
        }
        gameObject.SetActive(false);
        controlloMappa.initialWriting.SetActive(false);
        controlloMappa.chessboardBase.SetActive(true);
        controlloMappa.gameData.SaveData();
    }

    private void InitialWeights()
    {
        Dictionary<string, int> totalWeight = new Dictionary<string, int>();
        totalWeight["Casella 7"] = 1;
        totalWeight["Casella 8"] = 1;
        totalWeight["Casella 9"] = 1;
        totalWeight["Casella 12"] = 1;
        totalWeight["Casella 13"] = 1;
        totalWeight["Casella 14"] = 1;
        totalWeight["Casella 18"] = 1;
        totalWeight["Casella 1"] = random.Next(2, 4);
        bool loop = false;
        while (!loop)
        {
            foreach (string x in controlloMappa.adjacencyList.Keys)
            {
                if (totalWeight.ContainsKey(x))
                    continue;
                foreach (string y in controlloMappa.adjacencyList[x])
                {
                    if (totalWeight.ContainsKey(y))
                    {
                        if (totalWeight[y] == 2)
                            totalWeight[x] = 3;
                        if (totalWeight[y] == 3)
                            totalWeight[x] = 2;
                    }
                }
            }
            if (totalWeight.Count >= controlloMappa.adjacencyList.Count)
            {
                Debug.Log(controlloMappa.adjacencyList.Count);
                loop = !loop;
            }
        }

        controlloMappa.gameData.totalWeights = totalWeight; controlloMappa.gameData.SaveData();
        foreach (string x in controlloMappa.adjacencyList["Casella 0"])
        {
            controlloMappa.weights[x] = totalWeight[x];
        }
    }
}
