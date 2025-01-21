using System.Linq;
using UnityEngine;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomBackground : MonoBehaviour
{
    // Sprite array containing all background images
    [SerializeField] private GameObject[] backgrounds;

    void Start()
    {
        // assign a random background
        System.Random random = new System.Random();
        int randomNumber = random.Next(backgrounds.Length);
        Instantiate(backgrounds[randomNumber], Vector3.zero, Quaternion.identity);
    }
}
