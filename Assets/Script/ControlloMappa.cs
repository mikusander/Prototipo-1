using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class ControlloMappa : MonoBehaviour
{
    public GameObject player;
    public GameObject baseScacchiera;
    public GameData gameData;

    // Start is called before the first frame update
    void Start()
    {
        // Carica i dati salvati (se esistono)
        gameData = GetComponent<GameData>();
        if (gameData != null)
        {
            gameData.LoadData();
        }
        GameObject personaggio = GameObject.Find("avatar (Clone)");
        if (personaggio == null)
        {
            Debug.Log("ciao");
            Transform casellaTransform = baseScacchiera.transform.Find("Casella 0,0");
            if ( casellaTransform != null )
            {
                gameData.stringValues.Add("0,0");
                gameData.intValues[0] = 0;
                gameData.intValues[1] = 0;
                gameData.SaveData();
                GameObject casella = casellaTransform.gameObject;
                // Cambia colore usando il componente Renderer
                SpriteRenderer renderer = casella.GetComponent<SpriteRenderer>();

                // Verifica che il GameObject abbia un Renderer
                if (renderer != null)
                {
                    // Modifica il colore del materiale
                    renderer.color = Color.white;
                }
                Transform casellaSopraTransform = baseScacchiera.transform.Find("Casella 1,0");
                if (casellaSopraTransform != null )
                {
                    GameObject casellaSopra = casellaSopraTransform.gameObject;
                    SpriteRenderer rendererSopra = casellaSopra.GetComponent<SpriteRenderer>();
                    if (rendererSopra != null)
                    {
                        rendererSopra.color = new Color (255f, 255f, 0f, 255f);
                    }
                }
                Transform casellaDiagonaleTransform = baseScacchiera.transform.Find("Casella 1,1");
                if (casellaDiagonaleTransform != null )
                {
                    GameObject casellaDiagonale = casellaDiagonaleTransform.gameObject;
                    SpriteRenderer rendererDiagonale = casellaDiagonale.GetComponent<SpriteRenderer>();
                    if (rendererDiagonale != null)
                    {
                        rendererDiagonale.color = Color.red;
                    }
                }
                Transform casellaSinistraTransform = baseScacchiera.transform.Find("Casella 0,1");
                if ( casellaSinistraTransform != null )
                {
                    GameObject casellaSinistra = casellaSinistraTransform.gameObject;
                    SpriteRenderer rendererSinistra = casellaSinistra.GetComponent<SpriteRenderer>();
                    if (rendererSinistra != null)
                    {
                        rendererSinistra.color = Color.green;
                    }
                }
                Vector3 spawnPosition = casella.transform.position;
                player = Instantiate(player, spawnPosition, Quaternion.identity);
            }
        }
        else
        {
            player = personaggio;
            if (TempData.vittoria)
            {
                GameObject nuovaPosizione = GameObject.Find("Casella " + gameData.stringValues[gameData.stringValues.Count - 1]);
                Debug.Log(nuovaPosizione.name + nuovaPosizione.transform.position);
                player.transform.position = nuovaPosizione.transform.position;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
