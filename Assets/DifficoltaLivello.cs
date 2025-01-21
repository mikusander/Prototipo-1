using System.Net.NetworkInformation;
using UnityEngine;

public static class TempData
{
    public static string difficolta; // It is the next level difficulty
    public static bool vittoria; // It is true when the player win
    public static bool game = false; // it is true if the player has performed at least one match
    public static string lastBox; // It is the box that the player has chosen
    public static string lastError = ""; // it is the last square that the player missed
    public static bool animazione = false; // this variable indicates that an animation is in progress
    public static string casellaCliccata = ""; // this variable indicates the last box clicked
}
