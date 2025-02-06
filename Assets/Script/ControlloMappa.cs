using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ControlloMappa : MonoBehaviour
{

    public GameObject playButton;
    [SerializeField] private GameObject theEnd;
    [SerializeField] private GameObject gameoverLogo;
    public bool gameOver = false;
    [SerializeField] private GameObject restart;
    public GameObject player;
    [SerializeField] private GameObject error;
    public GameObject chessboardBase;
    public GameData gameData;
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private GameObject initialButton;
    public GameObject initialWriting;
    public GameObject mainWriting;
    public GameObject difficultyOne;
    public GameObject difficultyTwo;
    public GameObject difficultyThree;
    public GameObject textDifficultyOne;
    public GameObject textDifficultyTwo;
    public GameObject textDifficultyThree;
    public GameObject textDifficultyFinal;
    public GameObject finishLineFlag;
    public GameObject finalLogo;
    public Dictionary<string, int> weights = new Dictionary<string, int>();
    public Dictionary<string, int> actualWeights;
    private List<string> rightWrongBoxes = new List<string>();
    public GameObject twentySevenBox;
    public Dictionary<string, List<string>> adjacencyList = new Dictionary<string, List<string>>
    {
        {"Casella 1", new List<string> { "Casella 2", "Casella 6", "Casella 26" } },
        {"Casella 2", new List<string> { "Casella 1", "Casella 3", "Casella 6", "Casella 7", "Casella 8", "Casella 26" } },
        {"Casella 3", new List<string> { "Casella 2", "Casella 4", "Casella 8", "Casella 25", "Casella 26" } },
        {"Casella 4", new List<string> { "Casella 3", "Casella 8", "Casella 9", "Casella 10", "Casella 5", "Casella 25" } },
        {"Casella 5", new List<string> { "Casella 4", "Casella 10", "Casella 25" } },
        {"Casella 6", new List<string> { "Casella 1", "Casella 2", "Casella 7", "Casella 11" } },
        {"Casella 7", new List<string> { "Casella 2", "Casella 6", "Casella 8", "Casella 11", "Casella 12", "Casella 13" } },
        {"Casella 8", new List<string> { "Casella 2", "Casella 3", "Casella 4", "Casella 7", "Casella 9", "Casella 13" } },
        {"Casella 9", new List<string> { "Casella 4", "Casella 8", "Casella 10", "Casella 13", "Casella 14", "Casella 15" } },
        {"Casella 10", new List<string> { "Casella 4", "Casella 5", "Casella 9", "Casella 15" } },
        {"Casella 11", new List<string> { "Casella 6", "Casella 7", "Casella 12", "Casella 16" } },
        {"Casella 12", new List<string> { "Casella 7", "Casella 11", "Casella 13", "Casella 16", "Casella 17", "Casella 18" } },
        {"Casella 13", new List<string> { "Casella 7", "Casella 8", "Casella 9", "Casella 12", "Casella 14", "Casella 18" } },
        {"Casella 14", new List<string> { "Casella 9", "Casella 13", "Casella 15", "Casella 18", "Casella 19", "Casella 20" } },
        {"Casella 15", new List<string> { "Casella 9", "Casella 10", "Casella 14", "Casella 20" } },
        {"Casella 16", new List<string> { "Casella 11", "Casella 12", "Casella 17", "Casella 21" } },
        {"Casella 17", new List<string> { "Casella 12", "Casella 16", "Casella 18", "Casella 21", "Casella 22" } },
        {"Casella 18", new List<string> { "Casella 12", "Casella 13", "Casella 14", "Casella 17", "Casella 19", "Casella 22" } },
        {"Casella 19", new List<string> { "Casella 14", "Casella 18", "Casella 20", "Casella 22", "Casella 23" } },
        {"Casella 20", new List<string> { "Casella 14", "Casella 15", "Casella 19", "Casella 23" } },
        {"Casella 21", new List<string> { "Casella 16", "Casella 17" } },
        {"Casella 22", new List<string> { "Casella 17", "Casella 18", "Casella 19" } },
        {"Casella 23", new List<string> { "Casella 19", "Casella 20" } },
        {"Casella 25", new List<string> { "Casella 3", "Casella 4", "Casella 5" } },
        {"Casella 26", new List<string> { "Casella 1", "Casella 2", "Casella 3" } }
    };

    // Start is called before the first frame update
    void Start()
    {
        // Load saves
        gameData = GetComponent<GameData>();
        if (gameData != null)
        {
            gameData.LoadData();
        }

        // activation of the game map
        if (gameData.correctBoxes.Count > 0)
        {
            chessboardBase.SetActive(true);
        }



        // if the player have lost the last game, activate the wrong box animation
        bool newError = false;
        if (gameData.wrongBoxes.Count > 0 && TempData.lastError != gameData.wrongBoxes[gameData.wrongBoxes.Count - 1])
        {
            newError = true;
        }

        // assign the white color to a correct boxes
        foreach (string i in gameData.correctBoxes)
        {
            if (i == "Casella 0" || i == "Casella 24" || i == "the-end(Clone)")
                continue;
            SpriteRenderer box = GameObject.Find(i).GetComponent<SpriteRenderer>();
            if (box != null)
                box.color = Color.white;
        }

        // assign the X at the wrong boxes
        foreach (string i in gameData.wrongBoxes)
        {
            UnityEngine.Vector3 casellaSbagliataPosition = GameObject.Find(i).gameObject.transform.position;
            GameObject casella = Instantiate(error, casellaSbagliataPosition, UnityEngine.Quaternion.identity);
            if (TempData.game && !TempData.vittoria && newError && gameData.wrongBoxes[gameData.wrongBoxes.Count - 1] == i)
            {
                Animator animatorErr = casella.GetComponent<Animator>();
                StartCoroutine(ErrorAnimation(animatorErr));

                newError = false;
            }
            casella.name = "Errore " + i;
        }

        // assign at the memory the last error for the next wrong box animation
        if (gameData.wrongBoxes.Count > 1)
        {
            TempData.lastError = "Errore " + gameData.wrongBoxes[gameData.wrongBoxes.Count - 1];
        }

        if (gameData.correctBoxes.Count > 0 && gameData.correctBoxes[gameData.correctBoxes.Count - 1] == "the-end(Clone)")
        {
            theEnd.SetActive(true);
            restart.SetActive(true);
            gameOver = true;
        }
        // if there are no checked boxes it loads the home screen
        else if (gameData.correctBoxes.Count == 0)
        {
            initialButton.SetActive(true);
            initialWriting.SetActive(true);
        }
        else if (gameData.correctBoxes.Count > 0)
        {
            mainWriting.SetActive(true);
            rightWrongBoxes.AddRange(gameData.correctBoxes);
            rightWrongBoxes.AddRange(gameData.wrongBoxes);

            string lastBoxString = gameData.correctBoxes[gameData.correctBoxes.Count - 1];

            GameObject lastBox = null;
            if (lastBoxString != "Casella 0" && lastBoxString != "Casella 24")
            {
                lastBox = chessboardBase.transform.Find(lastBoxString).gameObject;
            }

            // Adding start and end boxes to the adjacency list
            if (gameData.start == 0)
            {
                adjacencyList["Casella 0"] = new List<string> { "Casella 1", "Casella 26" };
                adjacencyList["Casella 24"] = new List<string> { "Casella 19", "Casella 22", "Casella 23" };
                if (lastBox != null)
                {
                    player = Instantiate(player, lastBox.transform.position, Quaternion.identity);
                }
                else
                {
                    player = Instantiate(player, new Vector3(2f, -1.8f, 0f), Quaternion.identity);
                }
                if (adjacencyList["Casella 24"].Contains(lastBoxString))
                    Instantiate(finalLogo, new Vector3(-1f, 4f, 0f), Quaternion.identity);
                else
                    Instantiate(finishLineFlag, new Vector3(-1f, 4f, 0f), Quaternion.identity);
                twentySevenBox = Instantiate(twentySevenBox, new Vector3(1f, 4.1f, 0f), Quaternion.identity);
                twentySevenBox.transform.SetParent(chessboardBase.transform);
                twentySevenBox.name = "Casella 27";
                adjacencyList[twentySevenBox.name] = new List<string> { "Casella 17", "Casella 21", "Casella 22" };
                adjacencyList["Casella 17"].Add("Casella 27");
                adjacencyList["Casella 21"].Add("Casella 27");
                adjacencyList["Casella 22"].Add("Casella 27");
            }
            else
            {
                adjacencyList["Casella 0"] = new List<string> { "Casella 5", "Casella 25" };
                adjacencyList["Casella 24"] = new List<string> { "Casella 17", "Casella 21", "Casella 22" };
                if (lastBox != null)
                {
                    player = Instantiate(player, lastBox.transform.position, Quaternion.identity);
                }
                else
                {
                    player = Instantiate(player, new Vector3(-2f, -1.8f, 0f), Quaternion.identity);
                }
                if (adjacencyList["Casella 24"].Contains(lastBoxString))
                    Instantiate(finalLogo, new Vector3(1f, 4f, 0f), Quaternion.identity);
                else
                    Instantiate(finishLineFlag, new Vector3(1f, 4f, 0f), Quaternion.identity);
                twentySevenBox = Instantiate(twentySevenBox, new Vector3(-1f, 4.1f, 0f), Quaternion.identity);
                twentySevenBox.transform.SetParent(chessboardBase.transform);
                twentySevenBox.name = "Casella 27";
                adjacencyList[twentySevenBox.name] = new List<string> { "Casella 19", "Casella 22", "Casella 23" };
                adjacencyList["Casella 19"].Add("Casella 27");
                adjacencyList["Casella 22"].Add("Casella 27");
                adjacencyList["Casella 23"].Add("Casella 27");
            }

            if (TempData.game && TempData.vittoria)
            {
                if (gameData.correctBoxes.Count == 2)
                {
                    if (gameData.start == 0)
                    {
                        StartCoroutine(MoveToTarget(new Vector3(2f, -1.8f, 0f), lastBox.transform.position, moveDuration));
                    }
                    else
                    {
                        StartCoroutine(MoveToTarget(new Vector3(-2f, -1.8f, 0f), lastBox.transform.position, moveDuration));
                    }
                }
                else
                {
                    GameObject penultimateBox = GameObject.Find(gameData.correctBoxes[gameData.correctBoxes.Count - 2]);
                    Vector3 spawnPos = penultimateBox.transform.position;
                    StartCoroutine(MoveToTarget(spawnPos, lastBox.transform.position, moveDuration));
                }
            }

            foreach (string x in adjacencyList[lastBoxString])
            {
                weights[x] = gameData.totalWeights[x];
            }

            if (lastBox != null && lastBoxString != "Casella 0" && lastBoxString != "Casella 24")
            {
                SpriteRenderer renderer = lastBox.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.color = Color.white;
                }
            }

            Dictionary<string, int> actualWeights = CalculateDistances(lastBoxString);
            if (actualWeights.Count == 0)
            {
                gameoverLogo.SetActive(true);
                restart.SetActive(true);
                gameOver = true;
            }

            foreach (string key in weights.Keys)
            {
                if (rightWrongBoxes.Contains(key))
                    continue;
                GameObject box = chessboardBase.transform.Find(key).gameObject;
                if (box != null)
                {
                    SpriteRenderer rendererBox = box.GetComponent<SpriteRenderer>();
                    if (rendererBox != null)
                    {
                        Vector3 worldPosition;
                        switch (weights[key])
                        {
                            case 1:
                                rendererBox.color = Color.red;
                                GameObject appoDifficultyThree = Instantiate(difficultyThree, box.transform.position, Quaternion.identity);
                                worldPosition = appoDifficultyThree.transform.position;
                                appoDifficultyThree.transform.SetParent(box.transform);
                                appoDifficultyThree.transform.position = worldPosition;
                                break;
                            case 2:
                                rendererBox.color = new Color(255f, 255f, 0f, 255f);
                                GameObject appoDifficultyTwo = Instantiate(difficultyTwo, box.transform.position, Quaternion.identity);
                                worldPosition = appoDifficultyTwo.transform.position;
                                appoDifficultyTwo.transform.SetParent(box.transform);
                                appoDifficultyTwo.transform.position = worldPosition;
                                break;
                            case 3:
                                rendererBox.color = Color.green;
                                GameObject appoDifficultyOne = Instantiate(difficultyOne, box.transform.position, Quaternion.identity);
                                worldPosition = appoDifficultyOne.transform.position;
                                appoDifficultyOne.transform.SetParent(box.transform);
                                appoDifficultyOne.transform.position = worldPosition;
                                break;
                        }
                    }
                }
            }
        }
    }

    // disables all writing
    public void Deactivate()
    {
        mainWriting.SetActive(false);
        textDifficultyOne.SetActive(false);
        textDifficultyTwo.SetActive(false);
        textDifficultyThree.SetActive(false);
        textDifficultyFinal.SetActive(false);
    }

    // animation for the error game object
    IEnumerator ErrorAnimation(Animator animator)
    {
        animator.SetTrigger("Attivazione");

        AnimatorStateInfo animationInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = animationInfo.length;

        yield return new WaitForSeconds(animationDuration);
    }

    // player animation when he wins
    IEnumerator MoveToTarget(UnityEngine.Vector3 playerPosition, UnityEngine.Vector3 targetPosition, float duration)
    {
        UnityEngine.Vector3 startPosition = playerPosition;
        float timeElapsed = 0;

        Animator animator = player.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Attivazione");
        }

        while (timeElapsed < duration)
        {
            player.transform.position = UnityEngine.Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null; // Aspetta il prossimo frame
        }

        player.transform.position = targetPosition;
        if (animator != null)
        {
            animator.SetTrigger("Disattivazione");
        }
    }

    // Calcola le distanze da una casella iniziale (initialBFSNode) a tutte le altre caselle
    public Dictionary<string, int> CalculateDistances(string currentNode)
    {
        string initialBFSNode = "Casella 24";
        // Distanze inizializzate a infinito (o un grande valore arbitrario)
        Dictionary<string, int> distances = new Dictionary<string, int>();

        foreach (var node in adjacencyList.Keys)
        {
            distances[node] = int.MaxValue;
        }

        // La distanza del nodo iniziale da sé stesso è 0
        distances[initialBFSNode] = 0;

        // Coda per BFS
        Queue<string> queue = new Queue<string>();
        queue.Enqueue(initialBFSNode);

        foreach (string x in rightWrongBoxes)
        {
            distances[x] = -1;
        }

        // BFS
        while (queue.Count > 0)
        {
            string actualNode = queue.Dequeue();

            // Scorri i nodi adiacenti
            foreach (string neighbor in adjacencyList[actualNode])
            {
                if (neighbor == "Casella 0" || neighbor == "Casella 24")
                    continue;

                // Se non è stato ancora visitato (distanza infinita), calcola la distanza
                if (distances[neighbor] == int.MaxValue)
                {
                    distances[neighbor] = distances[actualNode] + 1;
                    queue.Enqueue(neighbor);
                }
            }
        }

        // extraction of the weights of the boxes adjacent to the current box
        Dictionary<string, int> currentAdjacency = new Dictionary<string, int>();
        foreach (string value in adjacencyList[currentNode])
        {
            if (distances[value] != -1 && distances[value] != int.MaxValue)
                currentAdjacency[value] = distances[value];
        }

        int betterRoute = 100;
        // search for the shortest route
        foreach (string key in currentAdjacency.Keys)
        {
            if (currentAdjacency[key] < betterRoute) betterRoute = currentAdjacency[key];
        }

        bool oneThree = false;
        Dictionary<string, int> result = new Dictionary<string, int>();

        // I assign the difficulty to adjacent levels
        bool diff = false;
        foreach (string key in currentAdjacency.Keys)
        {
            if (currentAdjacency[key] == betterRoute)
            {
                if (!oneThree)
                {
                    oneThree = true;
                    result[key] = 1;
                }
                else
                {
                    result[key] = 2;
                }
            }
            else
            {
                if (!diff)
                    result[key] = 3;
                else
                    result[key] = 2;
                diff = !diff;
            }
        }

        return result;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
