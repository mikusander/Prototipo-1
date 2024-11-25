using System.Linq;
using UnityEngine;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomBackground : MonoBehaviour
{
    // Array di Sprite che contiene tutte le immagini di sfondo
    public GameObject[] backgrounds;

    void Start()
    {
        System.Random random = new System.Random();
        int randomNumber = random.Next(backgrounds.Length);
        Instantiate(backgrounds[randomNumber], Vector3.zero, Quaternion.identity);
    }
}
