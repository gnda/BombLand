using System;
using UnityEngine;

namespace Common
{
    public class DebugTools
    {
        public static void printTilesState(char[,] tilesState)
        {
            string s = "\n";
            
            for (int j = tilesState.GetLength(1) - 1; j >= 0; j--)
            {
                for (int i = 0; i < tilesState.GetLength(0); i++)
                    s += tilesState[i, j];
                s += "\n";
            }
            
            Debug.Log(s);
        }
    }
}