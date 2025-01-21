using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Punti : MonoBehaviour
{

    [SerializeField] private Text punti;
    [SerializeField] private Controllo controllo;

    // Start is called before the first frame update
    void Start()
    {
        // assign initial points of 0 to the writing
        punti.GetComponent<Text>();
        punti.text = "0/" + controllo.totaleDomande.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // assign 0 points to the writing
        punti.text = controllo.puntiAttuali.ToString() + "/" + controllo.totaleDomande.ToString();
    }
}
