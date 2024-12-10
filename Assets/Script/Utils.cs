using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Utils
{
    // Esempio di funzione statica
    public static string Sopra(string pos)
    {
        int[] numeri = PrendiNumeri(pos);
        numeri[0] += 1;
        if(numeri[0] > 5)
        {
            return "";
        }
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static string Sinistra(string pos)
    {
        int[] numeri = PrendiNumeri(pos);
        numeri[1] += 1;
        if(numeri[1] > 3)
        {
            return "";
        }
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static string Destra(string pos)
    {
        int[] numeri = PrendiNumeri(pos);
        numeri[1] -= 1;
        if(numeri[1] < 0)
        {
            return "";
        }
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static string DiagonaleSinistra(string pos)
    {
        int[] numeri = PrendiNumeri(pos);
        numeri[0] += 1;
        numeri[1] += 1;
        if(numeri[0] > 5 || numeri[1] > 3)
        {
            return "";
        }
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static string DiagonaleDestra(string pos)
    {
        int[] numeri = PrendiNumeri(pos);
        numeri[0] += 1;
        numeri[1] -= 1;
        if(numeri[0] > 5 || numeri [1] < 0)
        {
            return "";
        }
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static string DiagonaleSottoDestra(string pos)
    {
        int[] numeri = PrendiNumeri(pos);
        numeri[0] -= 1;
        numeri[1] -= 1;
        if(numeri[0] < 0 || numeri[1] < 0)
        {
            return "";
        }
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static string DiagonaleSottoSinistra(string pos)
    {
        int[] numeri = PrendiNumeri(pos);
        numeri[0] -= 1;
        numeri[1] += 1;
        if(numeri[0] < 0 || numeri[1] > 3)
        {
            return "";
        }
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static string Sotto(string pos)
    {
        int[] numeri = PrendiNumeri(pos);
        numeri[0] -= 1;
        if(numeri[0] < 0)
        {
            return "";
        }
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

    public static bool ControlloPresenza(List<string> caselle, string casella)
    {
        foreach(string i in caselle)
        {
            if(i == casella)
            {
                return true;
            }
        }
        return false;
    }

    public static bool RigaSuperiore(string casella, List<string> caselleSbagliate)
    {
        int[] numeri = PrendiNumeri(casella);
        numeri[0] += 1;
        string[] caselleSuperiori = { 
            "Casella " + numeri[0].ToString() + ",0",
            "Casella " + numeri[0].ToString() + ",1",
            "Casella " + numeri[0].ToString() + ",2",
            "Casella " + numeri[0].ToString() + ",3"
            };
        foreach( string i in caselleSuperiori )
        {
            if( !ControlloPresenza(caselleSbagliate, i) )
            {
                return false;
            }
        }
        return true;
    }

    public static bool RigaSuperioreEDestra(string casella, List<string> caselleSbagliate)
    {
        int[] numeri = PrendiNumeri(casella);
        numeri[1] += 1;
        string casellaDestra = "Casella " + numeri[0].ToString() + numeri[1].ToString();
        numeri[0] += 1;
        string[] caselleSuperiori = { 
            "Casella " + numeri[0].ToString() + ",0",
            "Casella " + numeri[0].ToString() + ",1",
            "Casella " + numeri[0].ToString() + ",2",
            "Casella " + numeri[0].ToString() + ",3",
            casellaDestra
            };
        foreach( string i in caselleSuperiori )
        {
            if( ControlloPresenza(caselleSbagliate, i) )
            {
                return true;
            }
        }
        return false;
    }
}
