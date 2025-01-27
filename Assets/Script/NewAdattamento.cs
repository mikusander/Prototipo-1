using UnityEngine;
using TMPro;

public class NewAdattamento : MonoBehaviour
{
    [SerializeField] private TMP_Text targetTextMeshPro; // TMP_Text è il tipo base per TextMeshPro e TextMeshProUGUI

    void Update()
    {
        if (targetTextMeshPro != null)
        {
            // Usa il fontSize per adattare la scala del GameObject
            float textScaleFactor = targetTextMeshPro.fontSize;
            transform.localScale = Vector3.one * textScaleFactor;
        }
    }
}
