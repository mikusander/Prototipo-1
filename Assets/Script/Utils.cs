using UnityEngine;

public static class UtilityFunctions
{
    // Esempio di funzione statica
    public static string AumentaX(string pos)
    {
        string[] stringaSplittata = pos.Split(",");
        int charX = int.Parse(stringaSplittata[0]);
        charX += 1;
        string posAggiornata = charX.ToString() + stringaSplittata[1];
        return posAggiornata;
    }
}
