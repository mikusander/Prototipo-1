using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class ControlloMappa : MonoBehaviour
{
    public GameObject player;
    public int[] posizione = new int[2];
    public GameObject baseScacchiera;

    // Start is called before the first frame update
    void Start()
    {
        GameObject personaggio = GameObject.Find("avatar (Clone)");
        if (personaggio == null)
        {
            Transform casellaTransform = baseScacchiera.transform.Find("Casella 0,0");
            if ( casellaTransform != null )
            {
                posizione[0] = 0;
                posizione[0] = 0;
                GameObject casella = casellaTransform.gameObject;
                Vector3 spawnPosition = casella.transform.position;
                player = Instantiate(player, spawnPosition, Quaternion.identity);
            }
        }
        else
        {
            player = personaggio;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
