using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControlloMappa : MonoBehaviour
{

    [SerializeField] private GameObject theEnd;
    [SerializeField] private GameObject gameoverLogo;
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
    [SerializeField] private GameObject finishLineLogo;
    public Dictionary<string, int> weights;
    public int[] actualWeights;
    private List<string> rightWrongBoxes = new List<string>();
    public Dictionary<string, List<string>> adjacencyList = new Dictionary<string, List<string>>
    {
        {"Casella 1", new List<string> { "Casella 2", "Casella 6" } },
        {"Casella 2", new List<string> { "Casella 1", "Casella 3", "Casella 6", "Casella 7", "Casella 8" } },
        {"Casella 3", new List<string> { "Casella 2", "Casella 4", "Casella 8" } },
        {"Casella 4", new List<string> { "Casella 3", "Casella 8", "Casella 9", "Casella 10", "Casella 5" } },
        {"Casella 5", new List<string> { "Casella 4", "Casella 10" } },
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
    };

    // list of the initial position of the start position player and the position of the finish line
    public List<Vector3> initialPosition = new List<Vector3> {
            new Vector3(1f, -1.7f, 0f),
            new Vector3(-1f, -1.7f, 0f),
            new Vector3(-1f, 4.1f, 0f),
            new Vector3(1f, 4.1f, 0f)
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

        // colors the boxes that have been exceeded white
        for (int i = 0; i < gameData.correctBoxes.Count; i++)
        {
            if (gameData.correctBoxes[i] != gameData.start && gameData.correctBoxes[i] != gameData.finishLine)
            {
                SpriteRenderer colore = GameObject.Find(gameData.correctBoxes[i]).GetComponent<SpriteRenderer>();
                colore.color = Color.white;
            }
        }

        // if there are no checked boxes it loads the home screen
        if (gameData.correctBoxes.Count == 0)
        {
            initialButton.SetActive(true);
            initialWriting.SetActive(true);
        }
        /*

        // if the correct boxes are one load the initial start game
        else if (gameData.correctBoxes.Count == 1)
        {
            // It takes the initial box and load the boxes around it to choose the level
            mainWriting.SetActive(true);
            rightWrongBoxes.AddRange(gameData.correctBoxes);
            rightWrongBoxes.AddRange(gameData.wrongBoxes);

            string lastBoxString = gameData.correctBoxes[gameData.correctBoxes.Count - 1];
            Transform lastBoxTransform = chessboardBase.transform.Find(lastBoxString);

            if (TempData.game && !TempData.vittoria)
            {
                gameData.lastLose[0] = gameData.correctBoxes[gameData.correctBoxes.Count - 1];
                gameData.lastLose[1] = "yes";
                gameData.SaveData();
            }

            // load a weights of the boxes near the current box
            actualWeights = ConditionGameOver(rightWrongBoxes, lastBoxString, gameData.finishLine);
            if (
                gameData.lastLose[0] == gameData.correctBoxes[gameData.correctBoxes.Count - 1]
                &&
                gameData.lastLose[1] == "yes"
              )
            {
                weights = Utils.TransformStringToList(gameData.lastLose[2]);
            }
            else
            {
                weights = actualWeights;
                gameData.lastLose[2] = Utils.TransformListIntToString(weights);
                gameData.SaveData();
            }

            Transform boxTransform = chessboardBase.transform.Find(gameData.start);
            if (boxTransform != null)
            {
                GameObject casella = boxTransform.gameObject;
                SpriteRenderer renderer = casella.GetComponent<SpriteRenderer>();

                // change the color of the box
                if (renderer != null)
                {
                    renderer.color = Color.white;
                }

                // assign the number and the color at the boxes near current box
                ProcessBoxes(lastBoxString);

                // check if there is a game over
                if (actualWeights.Max() == 0)
                {
                    gameoverLogo.SetActive(true);
                    restart.SetActive(true);
                }
            }
        }
        else
        {
            mainWriting.SetActive(true);

            rightWrongBoxes.AddRange(gameData.correctBoxes);
            rightWrongBoxes.AddRange(gameData.wrongBoxes);

            string lastBoxString = gameData.correctBoxes[gameData.correctBoxes.Count - 1];
            Transform lastBoxTransform = chessboardBase.transform.Find(lastBoxString);

            if (TempData.game && !TempData.vittoria)
            {
                gameData.lastLose[0] = gameData.correctBoxes[gameData.correctBoxes.Count - 1];
                gameData.lastLose[1] = "yes";
                gameData.SaveData();
            }

            // load a weights of the near boxes
            actualWeights = ConditionGameOver(rightWrongBoxes, lastBoxString, gameData.finishLine);
            if (
                gameData.lastLose[0] == gameData.correctBoxes[gameData.correctBoxes.Count - 1]
                &&
                gameData.lastLose[1] == "yes"
              )
            {
                weights = Utils.TransformStringToList(gameData.lastLose[2]);
            }
            else
            {
                weights = actualWeights;
                gameData.lastLose[2] = Utils.TransformListIntToString(weights);
                gameData.SaveData();
            }

            // assign the number and the color of the boxes near the current box
            ProcessBoxes(lastBoxString);

            // if the player win the game start animaton from penultimate box to last box
            if (TempData.game && TempData.vittoria)
            {
                gameData.lastLose[1] = "no";
                gameData.SaveData();
                string penultimateString = gameData.correctBoxes[gameData.correctBoxes.Count - 2];
                Transform penultimateTransform = chessboardBase.transform.Find(penultimateString);
                if (lastBoxTransform != null && penultimateTransform != null)
                {
                    GameObject lastBox = lastBoxTransform.gameObject;
                    GameObject penultimateBox = penultimateTransform.gameObject;
                    UnityEngine.Vector3 spawnPosition = penultimateBox.transform.position;
                    player = Instantiate(player, spawnPosition, UnityEngine.Quaternion.identity);
                    StartCoroutine(MoveToTarget(spawnPosition, lastBox.transform.position, moveDuration));
                }
            }
            else
            {
                // spawn player
                if (lastBoxTransform != null)
                {
                    GameObject lastBox = lastBoxTransform.gameObject;
                    UnityEngine.Vector3 spawnPosition = lastBox.transform.position;
                    player = Instantiate(player, spawnPosition, UnityEngine.Quaternion.identity);
                }

                // Check if there is a lose condition
                if (Utils.SuperiorLine(lastBoxString, rightWrongBoxes))
                {
                    gameoverLogo.SetActive(true);
                    restart.SetActive(true);
                }
                else if (actualWeights.Max() == 0)
                {
                    gameoverLogo.SetActive(true);
                    restart.SetActive(true);
                }
            }

            // check if there is a win condition
            if (gameData.correctBoxes[gameData.correctBoxes.Count - 1] == gameData.finishLine)
            {
                finishLineFlag.SetActive(false);
                theEnd.SetActive(true);
                restart.SetActive(true);
            }
        }*/
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
    public Dictionary<string, int> CalculateDistances(Dictionary<string, List<string>> adjacencyList, string initialBFSNode, string currentNode)
    {
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

        // BFS
        while (queue.Count > 0)
        {
            string actualNode = queue.Dequeue();

            // Scorri i nodi adiacenti
            foreach (string neighbor in adjacencyList[actualNode])
            {
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
            currentAdjacency[value] = distances[value];
        }

        int betterRoute = 100;
        // search for the shortest route
        foreach (string key in currentAdjacency.Keys)
        {
            if (currentAdjacency[key] < betterRoute) betterRoute = currentAdjacency[key];
        }

        int secondBetterRoute = 100;
        // search for the second shortest route
        foreach (string key in currentAdjacency.Keys)
        {
            if (currentAdjacency[key] != betterRoute && currentAdjacency[key] < secondBetterRoute)
                secondBetterRoute = currentAdjacency[key];
        }

        Dictionary<string, int> result = new Dictionary<string, int>();
        // I assign the difficulty to adjacent levels
        foreach (string key in currentAdjacency.Keys)
        {
            if (currentAdjacency[key] == betterRoute)
            {
                result[key] = 1;
            }
            else if (currentAdjacency[key] == secondBetterRoute)
            {
                result[key] = 2;
            }
            else
            {
                result[key] = 3;
            }
        }

        string stamp = "";
        foreach (string key in distances.Keys)
        {
            stamp += key + ": " + distances[key].ToString() + "\n";
        }
        Debug.Log(stamp);

        return result;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
