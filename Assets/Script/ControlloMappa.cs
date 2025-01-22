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
    public int[] weights;
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
            SpriteRenderer colore = GameObject.Find(gameData.correctBoxes[i]).GetComponent<SpriteRenderer>();
            colore.color = Color.white;
        }

        // if there are no checked boxes it loads the home screen
        if (gameData.correctBoxes.Count == 0)
        {
            initialButton.SetActive(true);
            initialWriting.SetActive(true);
        }

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
        }
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

    public int[] ConditionGameOver(List<string> rightWrongBoxes, string lastBox, string finishLineFlag)
    {

        int[,] matrix = new int[6, 4];

        for (int x = 0; x < rightWrongBoxes.Count; x++)
        {
            int[] numeri = Utils.takeNumbers(rightWrongBoxes[x]);
            matrix[numeri[0], numeri[1]] = -1;
        }

        int[] numerifinalBox = Utils.takeNumbers(finishLineFlag);

        // departure of the bfs
        int startX = numerifinalBox[0], startY = numerifinalBox[1];

        // call bfs algoritm
        BFS(matrix, startX, startY);

        // assign the weight 1 at the final box
        matrix[numerifinalBox[0], numerifinalBox[1]] = 1;

        // take the position of the player box position
        int[] appo = Utils.takeNumbers(lastBox);

        // return the list of weights of the box near the player box position
        return minMax(matrix, appo[0], appo[1]);
    }

    private int[] minMax(int[,] matrix, int varX, int varY)
    {
        int[] miMa = new int[2];
        miMa[0] = 100;
        miMa[1] = 100;

        // directions of the boxes adjacent to the current one
        var directions = new List<(int, int)>
        {
            (-1,  0), // above
            ( 1,  0), // below
            ( 0, -1), // left
            ( 0,  1), // right
            (-1, -1), // above-left
            ( 1,  1), // below-right
            ( 1, -1), // below-left
            (-1,  1)  // above-right
        };

        // Map of directions and corresponding indices for 'weights'
        var directionIndices = new Dictionary<(int, int), int>
        {
            { (1,  0), 0 }, // Su
            { (1,  -1), 1 }, // Alto-destra
            { ( 0,  -1), 2 }, // Destra
            { ( -1,  -1), 3 }, // Basso-destra
            { ( -1,  0), 4 }, // Giù
            { ( -1, 1), 5 }, // Basso-sinistra
            { ( 0, 1), 6 }, // Sinistra
            { (1, 1), 7 }  // Alto-sinistra
        };

        int secondMaxCount = 0, maxCount = 0, boxCount = 0;

        // loop to check what the maximum value of the adjacent boxes is
        foreach (var (dx, dy) in directions)
        {
            int newX = varX + dx;
            int newY = varY + dy;

            // Check if the new position is valid
            if (newX >= 0 && newX < 6 && newY >= 0 && newY < 4 && matrix[newX, newY] != -1)
            {
                boxCount += 1;
                if (miMa[0] > matrix[newX, newY]) miMa[0] = matrix[newX, newY];
            }
        }

        // loop to check what the second maximum value of the adjacent boxes is
        foreach (var (dx, dy) in directions)
        {
            int newX = varX + dx;
            int newY = varY + dy;

            // Check if the new position is valid
            if (newX >= 0 && newX < 6 && newY >= 0 && newY < 4 && matrix[newX, newY] != -1)
            {
                if (miMa[0] != matrix[newX, newY] && miMa[1] > matrix[newX, newY]) miMa[1] = matrix[newX, newY];
            }
        }

        // loop to count how many maximum and minimum weight boxes there are
        foreach (var (dx, dy) in directions)
        {
            int newX = varX + dx, newY = varY + dy;

            if (newX >= 0 && newX < 6 && newY >= 0 && newY < 4 && matrix[newX, newY] != -1)
            {
                if (miMa[0] == matrix[newX, newY]) secondMaxCount += 1;
                if (miMa[1] == matrix[newX, newY]) maxCount += 1;
            }
        }

        int[] weights = new int[8];

        int numberBoxOne = 0, numberBoxTwo = 0, numberBoxOneTwo = boxCount - secondMaxCount;

        foreach (var (dx, dy) in directions)
        {
            int newX = varX + dx, newY = varY + dy;

            if (newX >= 0 && newX < 6 && newY >= 0 && newY < 4 && matrix[newX, newY] != -1)
            {
                int indice = directionIndices[(dx, dy)];
                // if there are no excess boxes between maximum and minimum I divide the weight 1 and 2 between the boxes that do not have the maximum weight
                if (boxCount - maxCount - secondMaxCount == 0)
                {
                    if (matrix[newX, newY] == miMa[0])
                    {
                        weights[indice] = 1;
                    }
                    else
                    {
                        if (numberBoxOne == (int)numberBoxOneTwo / 2)
                        {
                            weights[indice] = 2;
                            numberBoxTwo += 1;
                        }
                        else if (numberBoxTwo == (int)numberBoxOneTwo / 2)
                        {
                            weights[indice] = 3;
                            numberBoxOne += 1;
                        }
                        else
                        {
                            weights[indice] = UnityEngine.Random.Range(2, 4);
                            if (weights[indice] == 3)
                            {
                                numberBoxOne += 1;
                            }
                            else
                            {
                                numberBoxTwo += 1;
                            }
                        }
                    }
                }
                else
                {
                    if (matrix[newX, newY] == miMa[0])
                    {
                        weights[indice] = 1;
                    }
                    else if (matrix[newX, newY] == miMa[1])
                    {
                        weights[indice] = 2;
                    }
                    else
                    {
                        weights[indice] = 3;
                    }
                }
            }
        }
        return weights;
    }


    public static void BFS(int[,] matrix, int startX, int startY)
    {
        int rows = matrix.GetLength(0), cols = matrix.GetLength(1);

        // Directions to move (up, down, left, right)
        int[] dirX = { -1, 1, 0, 0 }, dirY = { 0, 0, -1, 1 };

        // Initialize the queue for the BFS
        Queue<(int, int)> queue = new Queue<(int, int)>();

        // Add the starting point to the queue and set the starting distance to 0
        queue.Enqueue((startX, startY));
        matrix[startX, startY] = 0;

        while (queue.Count > 0)
        {
            // Extract the item at the head of the queue
            var (x, y) = queue.Dequeue();

            // Explore adjacent directions
            for (int i = 0; i < dirX.Length; i++)
            {
                int newX = x + dirX[i], newY = y + dirY[i];

                if (newX >= 0 && newX < rows && newY >= 0 && newY < cols)
                {
                    // Continue only if the box is reachable and not yet visited
                    if (matrix[newX, newY] == 0)
                    {
                        matrix[newX, newY] = matrix[x, y] + 1; // Aggiorna il numero di passi
                        queue.Enqueue((newX, newY)); // Aggiungi la nuova posizione alla coda
                    }
                }
            }
        }

        // Set inaccessible boxes that have not been reached to -1
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (matrix[i, j] == 0 && (i != startX || j != startY)) matrix[i, j] = -1;
            }
        }
    }

    private void HandleBox(GameObject boxGameObject, GameObject boxWrong, int weightIndex, string finishLineName, UnityEngine.Vector3 spawnPosition, GameObject difficultyOne, GameObject difficultyTwo, GameObject difficultyThree)
    {
        if (boxGameObject != null && boxWrong == null)
        {
            SpriteRenderer boxRenderer = boxGameObject.GetComponent<SpriteRenderer>();

            // If the box is the finish line
            if (boxGameObject.name == finishLineName)
            {
                finishLineFlag.SetActive(false);
                Instantiate(finishLineLogo, spawnPosition, Quaternion.identity);
                boxRenderer.color = Color.white;
            }
            else if (boxRenderer.color != Color.white && actualWeights[weightIndex] != 0)
            {
                switch (weights[weightIndex])
                {
                    case 1:
                        boxRenderer.color = Color.red;
                        Instantiate(difficultyThree, spawnPosition, Quaternion.identity);
                        break;
                    case 2:
                        boxRenderer.color = new Color(255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
                        Instantiate(difficultyTwo, spawnPosition, Quaternion.identity);
                        break;
                    case 3:
                        boxRenderer.color = Color.green;
                        Instantiate(difficultyOne, spawnPosition, Quaternion.identity);
                        break;
                }
            }
        }
    }

    public void ProcessBoxes(string lastBoxString)
    {
        var directions = new (string UtilsMethod, int WeightIndex)[]
        {
            ("Above", 0),
            ("RightDiagonal", 1),
            ("Destra", 2),
            ("DiagonalRightBelow", 3),
            ("Below", 4),
            ("LeftDiagonalBelow", 5),
            ("Sinistra", 6),
            ("LeftDiagonal", 7)
        };

        foreach (var (methodName, weightIndex) in directions)
        {
            string boxName = (string)typeof(Utils).GetMethod(methodName).Invoke(null, new object[] { lastBoxString });
            string errorBoxName = "Errore " + boxName;

            GameObject boxGameObject = GameObject.Find(boxName);
            GameObject boxWrong = GameObject.Find(errorBoxName);

            UnityEngine.Vector3 spawnPosition = boxGameObject != null ? boxGameObject.transform.position : Vector3.zero;

            HandleBox(boxGameObject, boxWrong, weightIndex, gameData.finishLine, spawnPosition, difficultyOne, difficultyTwo, difficultyThree);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
