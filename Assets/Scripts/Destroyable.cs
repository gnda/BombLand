using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Destroyable : MonoBehaviour
{
    private char[,] tilesState;

    private void Start()
    {
        tilesState = GameManager.Instance.Level.TilesState;
    }

    private void OnDestroy()
    {
        tilesState[(int) transform.position.x, (int) transform.position.z] = '.';
    }
}