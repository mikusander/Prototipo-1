using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public ControlloMappa controlloMappa;
    public void AvviaGioco()
    {
        controlloMappa.scrittaPrincipale.SetActive(true);
        string[] possibleChars = { "1", "2", "3", "0" };
        int randomIndex = UnityEngine.Random.Range(0, possibleChars.Length);
        controlloMappa.gameData.traguardo = "Casella " + "5," + possibleChars[randomIndex];
        Transform casellaFinale = controlloMappa.baseScacchiera.transform.Find(controlloMappa.gameData.traguardo);
        if (casellaFinale != null)
        {
            UnityEngine.Vector3 spawnPos = casellaFinale.transform.position;
            controlloMappa.bandieraTraguardo = Instantiate(controlloMappa.bandieraTraguardo, spawnPos, UnityEngine.Quaternion.identity);
        }
        int randomIndexInizio = UnityEngine.Random.Range(0, possibleChars.Length);
        controlloMappa.gameData.inizio = "Casella " + "0," + possibleChars[randomIndexInizio];
        controlloMappa.gameData.stringValues.Add(controlloMappa.gameData.inizio);
        Transform casellaInizio = controlloMappa.baseScacchiera.transform.Find(controlloMappa.gameData.inizio);
        if (casellaInizio != null)
        {
            UnityEngine.Vector3 spawnPos = casellaInizio.transform.position;
            controlloMappa.player = Instantiate(controlloMappa.player, spawnPos, UnityEngine.Quaternion.identity);
        }
        List<string> singleElementList = new List<string> { controlloMappa.gameData.inizio };
        controlloMappa.pesi = controlloMappa.CondizioneGameOver(singleElementList, controlloMappa.gameData.inizio, controlloMappa.gameData.traguardo);
        Transform casellaTransform = controlloMappa.baseScacchiera.transform.Find(controlloMappa.gameData.inizio);
        if (casellaTransform != null)
        {
            GameObject casella = casellaTransform.gameObject;
            // Cambia colore usando il componente Renderer
            SpriteRenderer renderer = casella.GetComponent<SpriteRenderer>();

            // Verifica che il GameObject abbia un Renderer
            if (renderer != null)
            {
                // Modifica il colore del materiale
                renderer.color = Color.white;
            }
            GameObject casellaSopraGameObject = controlloMappa.baseScacchiera.transform.Find(Utils.Sopra(casella.name)).gameObject;
            if (casellaSopraGameObject != null)
            {
                SpriteRenderer casellaSopra = casellaSopraGameObject.GetComponent<SpriteRenderer>();
                if(controlloMappa.pesi[0] == 1)
                {
                    casellaSopra.color = Color.red;
                    UnityEngine.Vector3 spawnPos = casellaSopraGameObject.transform.position;
                    Instantiate(controlloMappa.difficoltaTre, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if(controlloMappa.pesi[0] == 2)
                {
                    casellaSopra.color = new Color(255f, 255f, 0f, 255f);
                    UnityEngine.Vector3 spawnPos = casellaSopraGameObject.transform.position;
                    Instantiate(controlloMappa.difficoltaDue, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if(controlloMappa.pesi[0] == 3)
                {
                    casellaSopra.color = Color.green;
                    UnityEngine.Vector3 spawnPos = casellaSopraGameObject.transform.position;
                    Instantiate(controlloMappa.difficoltaUno, spawnPos, UnityEngine.Quaternion.identity);
                }
            }
            GameObject casellaDiagonaleGameObject = controlloMappa.baseScacchiera.transform.Find(Utils.DiagonaleSinistra(casella.name)).gameObject;
            if (casellaDiagonaleGameObject != null)
            {
                SpriteRenderer casellaDiagonale = casellaDiagonaleGameObject.GetComponent<SpriteRenderer>();
                if(controlloMappa.pesi[7] == 1)
                {
                    casellaDiagonale.color = Color.red;
                    UnityEngine.Vector3 spawnPos = casellaDiagonaleGameObject.transform.position;
                    Instantiate(controlloMappa.difficoltaTre, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if(controlloMappa.pesi[7] == 2)
                {
                    casellaDiagonale.color = new Color(255f, 255f, 0f, 255f);
                    UnityEngine.Vector3 spawnPos = casellaDiagonaleGameObject.transform.position;
                    Instantiate(controlloMappa.difficoltaDue, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if(controlloMappa.pesi[7] == 3)
                {
                    casellaDiagonale.color = Color.green;
                    UnityEngine.Vector3 spawnPos = casellaDiagonaleGameObject.transform.position;
                    Instantiate(controlloMappa.difficoltaUno, spawnPos, UnityEngine.Quaternion.identity);
                }
            }
            GameObject casellaSinistraGameObject = controlloMappa.baseScacchiera.transform.Find(Utils.Sinistra(casella.name)).gameObject;
            if (casellaSinistraGameObject != null)
            {
                SpriteRenderer casellaSinistra = casellaSinistraGameObject.GetComponent<SpriteRenderer>();
                if(controlloMappa.pesi[6] == 1)
                {
                    casellaSinistra.color = Color.red;
                    UnityEngine.Vector3 spawnPos = casellaSinistraGameObject.transform.position;
                    Instantiate(controlloMappa.difficoltaTre, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if(controlloMappa.pesi[6] == 2)
                {
                    casellaSinistra.color = new Color(255f, 255f, 0f, 255f);
                    UnityEngine.Vector3 spawnPos = casellaSinistraGameObject.transform.position;
                    Instantiate(controlloMappa.difficoltaDue, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if(controlloMappa.pesi[6] == 3)
                {
                    casellaSinistra.color = Color.green;
                    UnityEngine.Vector3 spawnPos = casellaSinistraGameObject.transform.position;
                    Instantiate(controlloMappa.difficoltaUno, spawnPos, UnityEngine.Quaternion.identity);
                }
            }
            GameObject casellaDiagonaleDestraGameObject = controlloMappa.baseScacchiera.transform.Find(Utils.DiagonaleDestra(casella.name)).gameObject;
            if(casellaDiagonaleDestraGameObject != null)
            {
                SpriteRenderer casellaDiagonaleDestra = casellaDiagonaleDestraGameObject.GetComponent<SpriteRenderer>();
                if (controlloMappa.pesi[1] == 1)
                {
                    casellaDiagonaleDestra.color = Color.red;
                    UnityEngine.Vector3 spawnPos = casellaDiagonaleDestraGameObject.transform.position;
                    Instantiate(controlloMappa.difficoltaTre, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if (controlloMappa.pesi[1] == 2)
                {
                    casellaDiagonaleDestra.color = new Color(255f, 255f, 0f, 255f);
                    UnityEngine.Vector3 spawnPos = casellaDiagonaleDestraGameObject.transform.position;
                    Instantiate(controlloMappa.difficoltaDue, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if (controlloMappa.pesi[1] == 3)
                {
                    casellaDiagonaleDestra.color = Color.green;
                    UnityEngine.Vector3 spawnPos = casellaDiagonaleDestraGameObject.transform.position;
                    Instantiate(controlloMappa.difficoltaUno, spawnPos, UnityEngine.Quaternion.identity);
                }
            }
            GameObject casellaDestraGameObject = controlloMappa.baseScacchiera.transform.Find(Utils.Destra(casella.name)).gameObject;
            if (casellaDiagonaleDestraGameObject != null)
            {
                SpriteRenderer casellaDestra = casellaDestraGameObject.GetComponent<SpriteRenderer>();
                if (controlloMappa.pesi[2] == 1)
                {
                    casellaDestra.color = Color.red;
                    UnityEngine.Vector3 spawnPos = casellaDestraGameObject.transform.position;
                    Instantiate(controlloMappa.difficoltaTre, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if (controlloMappa.pesi[2] == 2)
                {
                    casellaDestra.color = new Color(255f, 255f, 0f, 255f);
                    UnityEngine.Vector3 spawnPos = casellaDestraGameObject.transform.position;
                    Instantiate(controlloMappa.difficoltaDue, spawnPos, UnityEngine.Quaternion.identity);
                }
                else if (controlloMappa.pesi[2] == 3)
                {
                    casellaDestra.color = Color.green;
                    UnityEngine.Vector3 spawnPos = casellaDestraGameObject.transform.position;
                    Instantiate(controlloMappa.difficoltaUno, spawnPos, UnityEngine.Quaternion.identity);
                }
            }
        }
        gameObject.SetActive(false);
        controlloMappa.scrittaIniziale.SetActive(false);
        controlloMappa.baseScacchiera.SetActive(true);
        controlloMappa.gameData.SaveData();
    }
}
