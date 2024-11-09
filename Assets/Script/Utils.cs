using UnityEngine;

public static class Utils
{
    // Esempio di funzione statica
    public static string Sopra(string pos)
    {
        int[] numeri = PrendiNumeri(pos);
        numeri[0] += 1;
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static string Sinistra(string pos)
    {
        int[] numeri = PrendiNumeri(pos);
        numeri[1] += 1;
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static string Destra(string pos)
    {
        int[] numeri = PrendiNumeri(pos);
        numeri[1] -= 1;
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static string DiagonaleSinistra(string pos)
    {
        int[] numeri = PrendiNumeri(pos);
        numeri[0] += 1;
        numeri[1] += 1;
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static string DiagonaleDestra(string pos)
    {
        int[] numeri = PrendiNumeri(pos);
        numeri[0] += 1;
        numeri[1] -= 1;
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static int[] PrendiNumeri(string casella)
    {
        int[] ris = new int[2];

        string[] appoggio = casella.Split(" ");
        string[] numeri = appoggio[1].Split(",");
        ris[0] = int.Parse(numeri[0]);
        ris[1] = int.Parse(numeri[1]);

        return ris;
    }
}
