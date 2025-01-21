using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class Utils
{
    // Function to create the name of the upper box
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

    // Function to create the name of the box on the left
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

    // Function to create the name of the box on the right
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

    // Function to create the name of the left diagonal box
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

    // Function to create the name of the right diagonal box
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

    // Function to create the name of the lower right diagonal box
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

    // Function to create the name of the lower left diagonal box
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

    // Function to create the name of the lower box
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

    // Function that takes the numbers of the box name
    public static int[] takeNumbers(string casella)
    {
        int[] ris = new int[2];

        string[] appoggio = casella.Split(" ");
        string[] numeri = appoggio[1].Split(",");
        ris[0] = int.Parse(numeri[0]);
        ris[1] = int.Parse(numeri[1]);

        return ris;
    }

    // Function thah creates a name of the box
    public static string CreaCasella(int x, int y)
    {
        return "Casella " + y.ToString() + "," + x.ToString();
    }

    // Function that checks for the presence of an element in a list
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

    // function that checks whether the top line is passable
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

    // Function that transforms a list of integers into a string with spaces in between
    public static string TransformListIntToString(int[] weightsList)
    {
        return string.Join(" ", weightsList);
    }

    // Function that transforms a string with integers into a list of integers
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

    // Function to print the entire contents of an array of integers
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
