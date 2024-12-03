using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public ControlloMappa controlloMappa;
    public void AvviaGioco()
    {
        Transform casellaTransform = controlloMappa.baseScacchiera.transform.Find("Casella 0,0");
        if ( casellaTransform != null )
        {
            controlloMappa.gameData.stringValues.Add("Casella 0,0");
            controlloMappa.gameData.SaveData();
            GameObject casella = casellaTransform.gameObject;
            // Cambia colore usando il componente Renderer
            SpriteRenderer renderer = casella.GetComponent<SpriteRenderer>();

            // Verifica che il GameObject abbia un Renderer
            if (renderer != null)
            {
                // Modifica il colore del materiale
                renderer.color = Color.white;
            }
            Transform casellaSopraTransform = controlloMappa.baseScacchiera.transform.Find("Casella 1,0");
            if (casellaSopraTransform != null )
            {
                GameObject casellaSopra = casellaSopraTransform.gameObject;
                SpriteRenderer rendererSopra = casellaSopra.GetComponent<SpriteRenderer>();
                if (rendererSopra != null)
                {
                    rendererSopra.color = new Color (255f, 255f, 0f, 255f);
                }
            }
            Transform casellaDiagonaleTransform = controlloMappa.baseScacchiera.transform.Find("Casella 1,1");
            if (casellaDiagonaleTransform != null )
            {
                GameObject casellaDiagonale = casellaDiagonaleTransform.gameObject;
                SpriteRenderer rendererDiagonale = casellaDiagonale.GetComponent<SpriteRenderer>();
                if (rendererDiagonale != null)
                {
                    rendererDiagonale.color = Color.red;
                }
            }
            Transform casellaSinistraTransform = controlloMappa.baseScacchiera.transform.Find("Casella 0,1");
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
            controlloMappa.player = Instantiate(controlloMappa.player, spawnPosition, Quaternion.identity);
        }
        gameObject.SetActive(false);
        controlloMappa.scrittaIniziale.SetActive(false);
        controlloMappa.baseScacchiera.SetActive(true);
    }
}
