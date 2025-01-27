using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewPunti : MonoBehaviour
{

    [SerializeField] private TMP_Text score;
    [SerializeField] private ControlNewGameplay controlNewGameplay;

    // Start is called before the first frame update
    void Start()
    {
        // assign initial points of 0 to the writing
        score.text = "0/5";
    }

    // Update is called once per frame
    void Update()
    {
        // assign 0 points to the writing
    }
}
