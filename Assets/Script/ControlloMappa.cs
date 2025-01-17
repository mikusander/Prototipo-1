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
    private List<string> rightWrongBoxes = new List<string>();

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

        // Random choice of finish line location
        if(gameData.finishLine == "")
        {
            string[] possibleChars = { "1", "2", "3", "0" };
            int randomIndex = UnityEngine.Random.Range(0, possibleChars.Length);
            gameData.finishLine = "Casella " + "5," + possibleChars[randomIndex];
            gameData.SaveData();
            GameObject finalBox = GameObject.Find(gameData.finishLine);
            if(finalBox != null)
            {
                UnityEngine.Vector3 spawnPos = finalBox.transform.position;
                finishLineFlag = Instantiate(finishLineFlag, spawnPos, UnityEngine.Quaternion.identity);
            }
        }
        else    // if the finish line has already been initialized, spawn it
        {
            GameObject finalBox = GameObject.Find(gameData.finishLine);
            if(finalBox != null)
            {
                UnityEngine.Vector3 spawnPos = finalBox.transform.position;
                finishLineFlag = Instantiate(finishLineFlag, spawnPos, UnityEngine.Quaternion.identity);
            }
        }

        // random choice of the initial box
        if(gameData.start == "")
        {
            string[] possibleChars = { "1", "2", "3", "0" };
            int randomIndex = UnityEngine.Random.Range(0, possibleChars.Length);
            gameData.start = "Casella " + "0," + possibleChars[randomIndex];
            gameData.SaveData();
            GameObject startBox = GameObject.Find(gameData.start);
            if(startBox != null)
            {
                UnityEngine.Vector3 spawnPos = startBox.transform.position;
                player = Instantiate(player, spawnPos, UnityEngine.Quaternion.identity);
            }
        }
        else if (gameData.correctBoxes.Count < 2) // if the start box has already been initialized, player spawn in this box
        {
            GameObject startBox = GameObject.Find(gameData.start);
            if(startBox != null)
            {
                UnityEngine.Vector3 spawnPos = startBox.transform.position;
                player = Instantiate(player, spawnPos, UnityEngine.Quaternion.identity);
            }
        }
        // if the player have lost the last game, activate the wrong box animation
        bool newError = false;
        if(gameData.wrongBoxes.Count > 0 && TempData.lastError != gameData.wrongBoxes[gameData.wrongBoxes.Count - 1])
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
        if(gameData.wrongBoxes.Count > 1)
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
            
            if(TempData.game && !TempData.vittoria)
            {
                gameData.lastLose[0] = gameData.correctBoxes[gameData.correctBoxes.Count - 1];
                gameData.lastLose[1] = "yes";
                gameData.SaveData();
            }

            if(
                gameData.lastLose[0] == gameData.correctBoxes[gameData.correctBoxes.Count - 1]
                &&
                gameData.lastLose[1] == "yes"
              )
            {
                weights = Utils.TransformStringToList(gameData.lastLose[2]);
            }
            else
            {
                weights = ConditionGameOver(rightWrongBoxes, lastBoxString, gameData.finishLine);
                gameData.lastLose[2] = Utils.TransformListIntToString(weights);
                gameData.SaveData();
            }

            Transform boxTransform = chessboardBase.transform.Find(gameData.start);
            if ( boxTransform != null )
            {
                GameObject casella = boxTransform.gameObject;
                SpriteRenderer renderer = casella.GetComponent<SpriteRenderer>();
                
                // change the color of the box
                if (renderer != null)
                {
                    renderer.color = Color.white;
                }

                GameObject boxAboveGameObject = GameObject.Find(Utils.Above(casella.name));
                GameObject boxAboveWrong = GameObject.Find("Errore " + Utils.Above(casella.name));
                // assign the color and the number of the difficolt of the box above
                if (boxAboveGameObject != null && boxAboveWrong == null)
                {
                    SpriteRenderer boxAbove = boxAboveGameObject.GetComponent<SpriteRenderer>();
                    if(weights[0] == 1)
                    {
                        boxAbove.color = Color.red;
                        UnityEngine.Vector3 spawnPos = boxAboveGameObject.transform.position;
                        Instantiate(difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[0] == 2)
                    {
                        boxAbove.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = boxAboveGameObject.transform.position;
                        Instantiate(difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[0] == 3)
                    {
                        boxAbove.color = Color.green;
                        UnityEngine.Vector3 spawnPos = boxAboveGameObject.transform.position;
                        Instantiate(difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
                
                GameObject leftDiagonalBoxGameObject = GameObject.Find(Utils.LeftDiagonal(casella.name));
                GameObject diagonalBoxError = GameObject.Find("Errore " + Utils.LeftDiagonal(casella.name));
                // assign the color and the number of the difficolt of the box left diagonal
                if (leftDiagonalBoxGameObject != null && diagonalBoxError == null)
                {
                    SpriteRenderer diagonalBox = leftDiagonalBoxGameObject.GetComponent<SpriteRenderer>();
                    if(weights[7] == 1)
                    {
                        diagonalBox.color = Color.red;
                        UnityEngine.Vector3 spawnPos = leftDiagonalBoxGameObject.transform.position;
                        Instantiate(difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[7] == 2)
                    {
                        diagonalBox.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = leftDiagonalBoxGameObject.transform.position;
                        Instantiate(difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[7] == 3)
                    {
                        diagonalBox.color = Color.green;
                        UnityEngine.Vector3 spawnPos = leftDiagonalBoxGameObject.transform.position;
                        Instantiate(difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }

                GameObject leftBoxGameObject = GameObject.Find(Utils.Sinistra(casella.name));
                GameObject leftBoxWrong = GameObject.Find("Errore " + Utils.Sinistra(casella.name));
                // assign the color and the number of the difficolt of the box left
                if (leftBoxGameObject != null && leftBoxWrong == null)
                {
                    SpriteRenderer leftBox = leftBoxGameObject.GetComponent<SpriteRenderer>();
                    if(weights[6] == 1)
                    {
                        leftBox.color = Color.red;
                        UnityEngine.Vector3 spawnPos = leftBoxGameObject.transform.position;
                        Instantiate(difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[6] == 2)
                    {
                        leftBox.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = leftBoxGameObject.transform.position;
                        Instantiate(difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[6] == 3)
                    {
                        leftBox.color = Color.green;
                        UnityEngine.Vector3 spawnPos = leftBoxGameObject.transform.position;
                        Instantiate(difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }

                GameObject rightDiagonalBoxGameObject = GameObject.Find(Utils.rightDiagonal(casella.name));
                GameObject rightDiagonalBoxWrongGameObject = GameObject.Find("Errore " + Utils.rightDiagonal(casella.name));
                // assign the color and the number of the difficolt of the box right diagonal
                if(rightDiagonalBoxGameObject != null && rightDiagonalBoxWrongGameObject == null)
                {
                    SpriteRenderer rightDiagonalBox = rightDiagonalBoxGameObject.GetComponent<SpriteRenderer>();
                    if (weights[1] == 1)
                    {
                        rightDiagonalBox.color = Color.red;
                        UnityEngine.Vector3 spawnPos = rightDiagonalBoxGameObject.transform.position;
                        Instantiate(difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if (weights[1] == 2)
                    {
                        rightDiagonalBox.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = rightDiagonalBoxGameObject.transform.position;
                        Instantiate(difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if (weights[1] == 3)
                    {
                        rightDiagonalBox.color = Color.green;
                        UnityEngine.Vector3 spawnPos = rightDiagonalBoxGameObject.transform.position;
                        Instantiate(difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }

                GameObject rightBoxGameObject = GameObject.Find(Utils.Destra(casella.name));
                GameObject rightBoxWrongGameobject = GameObject.Find("Errore " + Utils.Destra(casella.name));
                // assign the color and the number of the difficolt of the box right
                if (rightDiagonalBoxGameObject != null && rightDiagonalBoxWrongGameObject == null)
                {
                    SpriteRenderer rightBox = rightBoxGameObject.GetComponent<SpriteRenderer>();
                    if (weights[2] == 1)
                    {
                        rightBox.color = Color.red;
                        UnityEngine.Vector3 spawnPos = rightBoxGameObject.transform.position;
                        Instantiate(difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if (weights[2] == 2)
                    {
                        rightBox.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = rightBoxGameObject.transform.position;
                        Instantiate(difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if (weights[2] == 3)
                    {
                        rightBox.color = Color.green;
                        UnityEngine.Vector3 spawnPos = rightBoxGameObject.transform.position;
                        Instantiate(difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }

                if ( weights.Max() == 0 )
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

            if(TempData.game && !TempData.vittoria)
            {
                gameData.lastLose[0] = gameData.correctBoxes[gameData.correctBoxes.Count - 1];
                gameData.lastLose[1] = "yes";
                gameData.SaveData();
            }

            if(
                gameData.lastLose[0] == gameData.correctBoxes[gameData.correctBoxes.Count - 1]
                &&
                gameData.lastLose[1] == "yes"
              )
            {
                weights = Utils.TransformStringToList(gameData.lastLose[2]);
            }
            else
            {
                weights = ConditionGameOver(rightWrongBoxes, lastBoxString, gameData.finishLine);
                gameData.lastLose[2] = Utils.TransformListIntToString(weights);
                gameData.SaveData();
            }
            
            GameObject boxAboveGameObject = GameObject.Find(Utils.Above(lastBoxString));
            GameObject boxAboveWrong = GameObject.Find("Errore " + Utils.Above(lastBoxString));
            // assign the color and the number of difficolt of the box above
            if (boxAboveGameObject != null && boxAboveWrong == null)
            {
                SpriteRenderer boxAbove = boxAboveGameObject.GetComponent<SpriteRenderer>();
                // if the upper box is the finish line, assign the "the end" logo to that box
                if(boxAboveGameObject.name == gameData.finishLine)
                {
                    finishLineFlag.SetActive(false);
                    UnityEngine.Vector3 spawnPos = GameObject.Find(gameData.finishLine).transform.position;
                    Instantiate(finishLineLogo, spawnPos, UnityEngine.Quaternion.identity);
                    boxAbove.color = Color.white;
                }
                else if(boxAbove.color != Color.white)
                {
                    if(weights[0] == 1)
                    {
                        boxAbove.color = Color.red;
                        UnityEngine.Vector3 spawnPos = boxAboveGameObject.transform.position;
                        Instantiate(difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[0] == 2)
                    {
                        boxAbove.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = boxAboveGameObject.transform.position;
                        Instantiate(difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[0] == 3)
                    {
                        boxAbove.color = Color.green;
                        UnityEngine.Vector3 spawnPos = boxAboveGameObject.transform.position;
                        Instantiate(difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
            }

            GameObject leftDiagonalBoxGameObject = GameObject.Find(Utils.LeftDiagonal(lastBoxString));
            GameObject diagonalBoxError = GameObject.Find("Errore " + Utils.LeftDiagonal(lastBoxString));
            // assign the color and the number of difficolt of the box diagonal
            if (leftDiagonalBoxGameObject != null && diagonalBoxError == null)
            {
                SpriteRenderer diagonalBox = leftDiagonalBoxGameObject.GetComponent<SpriteRenderer>();
                // if the diagonal box is the finish line, assign the "the end" logo to that box
                if(leftDiagonalBoxGameObject.name == gameData.finishLine)
                {
                    finishLineFlag.SetActive(false);
                    UnityEngine.Vector3 spawnPos = GameObject.Find(gameData.finishLine).transform.position;
                    Instantiate(finishLineLogo, spawnPos, UnityEngine.Quaternion.identity);
                    diagonalBox.color = Color.white;
                }
                else if(diagonalBox.color != Color.white)
                {
                    if(weights[7] == 1)
                    {
                        diagonalBox.color = Color.red;
                        UnityEngine.Vector3 spawnPos = leftDiagonalBoxGameObject.transform.position;
                        Instantiate(difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[7] == 2)
                    {
                        diagonalBox.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = leftDiagonalBoxGameObject.transform.position;
                        Instantiate(difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[7] == 3)
                    {
                        diagonalBox.color = Color.green;
                        UnityEngine.Vector3 spawnPos = leftDiagonalBoxGameObject.transform.position;
                        Instantiate(difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
            }

            GameObject leftBoxGameObject = GameObject.Find(Utils.Sinistra(lastBoxString));
            GameObject leftBoxWrong = GameObject.Find("Errore " + Utils.Sinistra(lastBoxString));
            // assign the color and the number of difficolt of the box left
            if (leftBoxGameObject != null && leftBoxWrong == null)
            {
                SpriteRenderer leftBox = leftBoxGameObject.GetComponent<SpriteRenderer>();
                // if the left box is the finish line, assign the "the end" logo to that box
                if(leftBoxGameObject.name == gameData.finishLine)
                {
                    finishLineFlag.SetActive(false);
                    UnityEngine.Vector3 spawnPos = GameObject.Find(gameData.finishLine).transform.position;
                    Instantiate(finishLineLogo, spawnPos, UnityEngine.Quaternion.identity);
                    leftBox.color = Color.white;
                }
                else if(leftBox.color != Color.white)
                {
                    if(weights[6] == 1)
                    {
                        leftBox.color = Color.red;
                        UnityEngine.Vector3 spawnPos = leftBoxGameObject.transform.position;
                        Instantiate(difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[6] == 2)
                    {
                        leftBox.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = leftBoxGameObject.transform.position;
                        Instantiate(difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[6] == 3)
                    {
                        leftBox.color = Color.green;
                        UnityEngine.Vector3 spawnPos = leftBoxGameObject.transform.position;
                        Instantiate(difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
            }

            GameObject rightDiagonalBoxGameObject = GameObject.Find(Utils.rightDiagonal(lastBoxString));
            GameObject rightDiagonalBoxWrong = GameObject.Find("Errore " + Utils.rightDiagonal(lastBoxString));
            // assign the color and the number of difficolt of the box right diagonal
            if(rightDiagonalBoxGameObject != null && rightDiagonalBoxWrong == null)
            {
                SpriteRenderer rightDiagonalBox = rightDiagonalBoxGameObject.GetComponent<SpriteRenderer>();
                // if the diagonal right box is the finish line, assign the "the end" logo to that box
                if(rightDiagonalBoxGameObject.name == gameData.finishLine)
                {
                    finishLineFlag.SetActive(false);
                    UnityEngine.Vector3 spawnPos = GameObject.Find(gameData.finishLine).transform.position;
                    Instantiate(finishLineLogo, spawnPos, UnityEngine.Quaternion.identity);
                    rightDiagonalBox.color = Color.white;
                }
                else if(rightDiagonalBox.color != Color.white)
                {
                    if(weights[1] == 1)
                    {
                        rightDiagonalBox.color = Color.red;
                        UnityEngine.Vector3 spawnPos = rightDiagonalBoxGameObject.transform.position;
                        Instantiate(difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[1] == 2)
                    {
                        rightDiagonalBox.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = rightDiagonalBoxGameObject.transform.position;
                        Instantiate(difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[1] == 3)
                    {
                        rightDiagonalBox.color = Color.green;
                        UnityEngine.Vector3 spawnPos = rightDiagonalBoxGameObject.transform.position;
                        Instantiate(difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
            }

            GameObject rightBoxGameObject = GameObject.Find(Utils.Destra(lastBoxString));
            GameObject rightBoxWrong = GameObject.Find("Errore " + Utils.Destra(lastBoxString));
            // assign the color and the number of difficolt of the box right
            if(rightBoxGameObject != null && rightBoxWrong == null)
            {
                SpriteRenderer rightBox = rightBoxGameObject.GetComponent<SpriteRenderer>();
                // if the right box is the finish line, assign the "the end" logo to that box
                if(rightBoxGameObject.name == gameData.finishLine)
                {
                    finishLineFlag.SetActive(false);
                    UnityEngine.Vector3 spawnPos = GameObject.Find(gameData.finishLine).transform.position;
                    Instantiate(finishLineLogo, spawnPos, UnityEngine.Quaternion.identity);
                    rightBox.color = Color.white;
                }
                else if(rightBox.color != Color.white)
                {
                    if(weights[2] == 1)
                    {
                        rightBox.color = Color.red;
                        UnityEngine.Vector3 spawnPos = rightBoxGameObject.transform.position;
                        Instantiate(difficultyThree, spawnPos, UnityEngine.Quaternion.identity); 
                    }
                    else if(weights[2] == 2)
                    {
                        rightBox.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = rightBoxGameObject.transform.position;
                        Instantiate(difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[2] == 3)
                    {
                        rightBox.color = Color.green;
                        UnityEngine.Vector3 spawnPos = rightBoxGameObject.transform.position;
                        Instantiate(difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
            }

            GameObject DiagonalBelowRightBoxGameObject = GameObject.Find(Utils.diagonalRightBelow(lastBoxString));
            GameObject wrongDiagonalBoxBelowRight = GameObject.Find("Errore " + Utils.diagonalRightBelow(lastBoxString));
            // assign the color and the number of difficolt of the box diagonal below right
            if(DiagonalBelowRightBoxGameObject != null && wrongDiagonalBoxBelowRight == null)
            {
                SpriteRenderer DiagonalBelowRightBox = DiagonalBelowRightBoxGameObject.GetComponent<SpriteRenderer>();
                // if the diagonal right below box is the finish line, assign the "the end" logo to that box
                if(DiagonalBelowRightBox.color != Color.white)
                {
                    if(weights[3] == 1)
                    {
                        DiagonalBelowRightBox.color = Color.red;
                        UnityEngine.Vector3 spawnPos = DiagonalBelowRightBoxGameObject.transform.position;
                        Instantiate(difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[3] == 2)
                    {
                        DiagonalBelowRightBox.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = DiagonalBelowRightBoxGameObject.transform.position;
                        Instantiate(difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[3] == 3)
                    {
                        DiagonalBelowRightBox.color = Color.green;
                        UnityEngine.Vector3 spawnPos = DiagonalBelowRightBoxGameObject.transform.position;
                        Instantiate(difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
            }

            GameObject diagonalBelowLeftBoxGameObject = GameObject.Find(Utils.leftDiagonalBelow(lastBoxString));
            GameObject wrongDiagonalBoxBelowLeft = GameObject.Find("Errore " + Utils.leftDiagonalBelow(lastBoxString));
            // assign the color and the number of difficolt of the box diagonal below left
            if(diagonalBelowLeftBoxGameObject != null && wrongDiagonalBoxBelowLeft == null)
            {
                SpriteRenderer diagonalBelowLeftBox = diagonalBelowLeftBoxGameObject.GetComponent<SpriteRenderer>();
                // if the diagonal left below box is the finish line, assign the "the end" logo to that box
                if(diagonalBelowLeftBox.color != Color.white)
                {
                    if(weights[5] == 1)
                    {
                        diagonalBelowLeftBox.color = Color.red;
                        UnityEngine.Vector3 spawnPos = diagonalBelowLeftBoxGameObject.transform.position;
                        Instantiate(difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[5] == 2)
                    {
                        diagonalBelowLeftBox.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = diagonalBelowLeftBoxGameObject.transform.position;
                        Instantiate(difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[5] == 3)
                    {
                        diagonalBelowLeftBox.color = Color.green;
                        UnityEngine.Vector3 spawnPos = diagonalBelowLeftBoxGameObject.transform.position;
                        Instantiate(difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
            }

            GameObject belowBoxGameObject = GameObject.Find(Utils.Below(lastBoxString));
            GameObject belowBoxWrong = GameObject.Find("Errore " + Utils.Below(lastBoxString));
            // assign the color and the number of difficolt of the box below
            if(belowBoxGameObject != null && belowBoxWrong == null)
            {
                SpriteRenderer belowBox = belowBoxGameObject.GetComponent<SpriteRenderer>();
                // if the box below is the finish line, assign the "the end" logo to that box
                if(belowBox.color != Color.white)
                {
                    if(weights[4] == 1)
                    {
                        belowBox.color = Color.red;
                        UnityEngine.Vector3 spawnPos = belowBoxGameObject.transform.position;
                        Instantiate(difficultyThree, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[4] == 2)
                    {
                        belowBox.color = new Color(255f, 255f, 0f, 255f);
                        UnityEngine.Vector3 spawnPos = belowBoxGameObject.transform.position;
                        Instantiate(difficultyTwo, spawnPos, UnityEngine.Quaternion.identity);
                    }
                    else if(weights[4] == 3)
                    {
                        belowBox.color = Color.green;
                        UnityEngine.Vector3 spawnPos = belowBoxGameObject.transform.position;
                        Instantiate(difficultyOne, spawnPos, UnityEngine.Quaternion.identity);
                    }
                }
            }

            // if the player win the game start animaton from penultimate box to last box
            if(TempData.game && TempData.vittoria)
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
                if(Utils.SuperiorLine(lastBoxString, rightWrongBoxes))
                {
                    gameoverLogo.SetActive(true);
                    restart.SetActive(true);
                }
                else if(weights.Max() == 0)
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

    public int[] ConditionGameOver(List<string> rightWrongBoxes, string lastBox, string finishLineFlag){
        
        int[,] matrix = new int[6, 4];

        for( int x = 0; x < rightWrongBoxes.Count; x++ )
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
        miMa[1] = 0;

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

        int minCount = 0;
        int maxCount = 0;
        int boxCount = 0;

        foreach (var (dx, dy) in directions)
        {
            int newX = varX + dx;
            int newY = varY + dy;
            
            // Controlla che la nuova posizione sia valida
            if (newX >= 0 && newX < 6 && newY >= 0 && newY < 4 && matrix[newX, newY] != -1)
            {
                boxCount += 1;
                if (miMa[0] > matrix[newX, newY])
                {
                    miMa[0] = matrix[newX, newY];
                }
                if (miMa[1] < matrix[newX, newY])
                {
                    miMa[1] = matrix[newX, newY];
                }
            }
        }

        foreach (var (dx, dy) in directions)
        {
            int newX = varX + dx;
            int newY = varY + dy;
            
            // Controlla che la nuova posizione sia valida
            if (newX >= 0 && newX < 6 && newY >= 0 && newY < 4 && matrix[newX, newY] != -1)
            {
                if (miMa[0] == matrix[newX, newY])
                {
                    minCount += 1;
                }
                if (miMa[1] == matrix[newX, newY])
                {
                    maxCount += 1;
                }
            }
        }

        // Mappa di direzioni e indici corrispondenti per 'weights'
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

        int[] weights = new int[8];

        int numberBoxOne = 0, numberBoxTwo = 0, numberBoxOneTwo = boxCount - minCount;

        foreach (var (dx, dy) in directions)
        {
            int newX = varX + dx;
            int newY = varY + dy;

            // Controlla che la nuova posizione sia valida
            if (newX >= 0 && newX < 6 && newY >= 0 && newY < 4 && matrix[newX, newY] != -1)
            {
                int indice = directionIndices[(dx, dy)]; // Ottieni l'indice associato alla direzione
                if(boxCount - maxCount - minCount == 0)
                {
                    if (matrix[newX, newY] == miMa[0])
                    {
                        weights[indice] = 1;
                    }
                    else
                    {
                        if (numberBoxOne == (int) numberBoxOneTwo / 2)
                        {
                            weights[indice] = 2;
                            numberBoxTwo += 1;
                        }
                        else if (numberBoxTwo == (int) numberBoxOneTwo / 2)
                        {
                            weights[indice] = 3;
                            numberBoxOne += 1;
                        }
                        else
                        {
                            weights[indice] = UnityEngine.Random.Range(2,4);
                            if(weights[indice] == 3)
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
                        weights[indice] = 3;
                    }
                    else
                    {
                        weights[indice] = 2;
                    }
                }
            }
        }

        return weights;
    }


    public static void BFS(int[,] matrix, int startX, int startY)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        // Direzioni per muoversi (su, giù, sinistra, destra)
        int[] dirX = { -1,  1,  0,  0 };
        int[] dirY = {  0,  0, -1,  1 };

        // Inizializza la coda per il BFS
        Queue<(int, int)> queue = new Queue<(int, int)>();

        // Aggiungi il punto di partenza alla coda e imposta la distanza iniziale a 0
        queue.Enqueue((startX, startY));
        matrix[startX, startY] = 0;

        while (queue.Count > 0)
        {
            // Estrai l'elemento in testa alla coda
            var (x, y) = queue.Dequeue();

            // Esplora le direzioni adiacenti
            for (int i = 0; i < dirX.Length; i++)
            {
                int newX = x + dirX[i];
                int newY = y + dirY[i];

                // Controlla se la nuova posizione è valida
                if (newX >= 0 && newX < rows && newY >= 0 && newY < cols)
                {
                    // Continua solo se la casella è raggiungibile e non ancora visitata
                    if (matrix[newX, newY] == 0)
                    {
                        matrix[newX, newY] = matrix[x, y] + 1; // Aggiorna il numero di passi
                        queue.Enqueue((newX, newY)); // Aggiungi la nuova posizione alla coda
                    }
                }
            }
        }

        // Imposta a -1 le caselle inaccessibili che non sono state raggiunte
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (matrix[i, j] == 0 && (i != startX || j != startY))
                {
                    matrix[i, j] = -1; // Non raggiunto
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
