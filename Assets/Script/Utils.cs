using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class Utils
{
    // Esempio di funzione statica
    public static string Above(string pos)
    {
        int[] numeri = takeNumbers(pos);
        numeri[0] += 1;
        if(numeri[0] > 5)
        {
            return "";
        }
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static string Sinistra(string pos)
    {
        int[] numeri = takeNumbers(pos);
        numeri[1] += 1;
        if(numeri[1] > 3)
        {
            return "";
        }
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static string Destra(string pos)
    {
        int[] numeri = takeNumbers(pos);
        numeri[1] -= 1;
        if(numeri[1] < 0)
        {
            return "";
        }
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static string LeftDiagonal(string pos)
    {
        int[] numeri = takeNumbers(pos);
        numeri[0] += 1;
        numeri[1] += 1;
        if(numeri[0] > 5 || numeri[1] > 3)
        {
            return "";
        }
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static string RightDiagonal(string pos)
    {
        int[] numeri = takeNumbers(pos);
        numeri[0] += 1;
        numeri[1] -= 1;
        if(numeri[0] > 5 || numeri [1] < 0)
        {
            return "";
        }
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static string DiagonalRightBelow(string pos)
    {
        int[] numeri = takeNumbers(pos);
        numeri[0] -= 1;
        numeri[1] -= 1;
        if(numeri[0] < 0 || numeri[1] < 0)
        {
            return "";
        }
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static string LeftDiagonalBelow(string pos)
    {
        int[] numeri = takeNumbers(pos);
        numeri[0] -= 1;
        numeri[1] += 1;
        if(numeri[0] < 0 || numeri[1] > 3)
        {
            return "";
        }
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static string Below(string pos)
    {
        int[] numeri = takeNumbers(pos);
        numeri[0] -= 1;
        if(numeri[0] < 0)
        {
            return "";
        }
        return "Casella " + numeri[0].ToString() + "," + numeri[1].ToString();
    }

    public static int[] takeNumbers(string casella)
    {
        int[] ris = new int[2];

        string[] appoggio = casella.Split(" ");
        string[] numeri = appoggio[1].Split(",");
        ris[0] = int.Parse(numeri[0]);
        ris[1] = int.Parse(numeri[1]);

        return ris;
    }

    public static string CreaCasella(int x, int y)
    {
        return "Casella " + y.ToString() + "," + x.ToString();
    }

    public static bool presenceControl(List<string> caselle, string casella)
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

    public static bool SuperiorLine(string casella, List<string> wrongBoxes)
    {
        int[] numeri = takeNumbers(casella);
        numeri[0] += 1;
        string[] caselleSuperiori = { 
            "Casella " + numeri[0].ToString() + ",0",
            "Casella " + numeri[0].ToString() + ",1",
            "Casella " + numeri[0].ToString() + ",2",
            "Casella " + numeri[0].ToString() + ",3"
            };
        foreach( string i in caselleSuperiori )
        {
            if( !presenceControl(wrongBoxes, i) )
            {
                return false;
            }
        }
        return true;
    }

    public static string TransformListIntToString(int[] weightsList)
    {
        return string.Join(" ", weightsList);
    }

    public static int[] TransformStringToList(string weightsString)
    {
        int [] appo = new int[8];
        string [] appog = weightsString.Split(" ");
        for(int x = 0; x < appog.Length; x++)
        {
            appo[x] = int.Parse(appog[x]);
        }
        return appo;
    }

    public static bool SuperiorRightLine(string casella, List<string> wrongBoxes)
    {
        int[] numeri = takeNumbers(casella);
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
            if( presenceControl(wrongBoxes, i) )
            {
                return true;
            }
        }
        return false;
    }

    public static void PrintMatrix(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        string matrixx = "";

        for (int i = 0; i < rows; i++)
        {
            string row = "";
            for (int j = 0; j < cols; j++)
            {
                row += matrix[i, j].ToString("D2") + " ";
            }
            matrixx += row + "\n";
        }
        Debug.Log(matrixx);
    }
}
