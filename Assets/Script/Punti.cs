using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Punti : MonoBehaviour
{

    public Text punti;
    public Controllo controllo;

    // Start is called before the first frame update
    void Start()
    {
        punti.GetComponent<Text>();
        punti.text = "0/10";
    }

    // Update is called once per frame
    void Update()
    {
        
        punti.text = controllo.puntiAttuali.ToString() + "/10";
    }
}
