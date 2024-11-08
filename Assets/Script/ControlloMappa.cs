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
        if (gameData.stringValues.Count == 0)
        {
            Transform casellaTransform = baseScacchiera.transform.Find("Casella 0,0");
            if ( casellaTransform != null )
            {
                gameData.stringValues.Add("Casella 0,0");
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
            if(TempData.vittoria || !TempData.gioco)
            {
                string ultimaCasellaString = gameData.stringValues[gameData.stringValues.Count - 1];
                string penultimaCasellaString = gameData.stringValues[gameData.stringValues.Count - 2];
                Transform ultimaCasellaTransform = baseScacchiera.transform.Find(ultimaCasellaString);
                Transform penultimaCasellaTransform = baseScacchiera.transform.Find(penultimaCasellaString);
                for (int i = 0; i < gameData.stringValues.Count; i++)
                {
                    SpriteRenderer colore = GameObject.Find(gameData.stringValues[i]).GetComponent<SpriteRenderer>();
                    colore.color = Color.white;
                }
                if (ultimaCasellaTransform != null && penultimaCasellaTransform != null)
                {
                    GameObject ultimaCasella = ultimaCasellaTransform.gameObject;
                    GameObject penultimaCasella = penultimaCasellaTransform.gameObject;
                    Vector3 spawnPosition = penultimaCasella.transform.position;
                    player = Instantiate(player, spawnPosition, Quaternion.identity);
                      
                }
            }
            else
            {

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
