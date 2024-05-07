using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileType
{
    public string name;
    public GameObject tileVisualPrefab;
    public float movementCost = 1f;
    public bool isWalkable = true;
    public float elevation = 0f;
}
