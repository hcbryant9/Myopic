using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickTile : MonoBehaviour
{
    private int _tileX;
    private int _tileY;
    public TileMap map;
    [SerializeField] private TextMeshProUGUI movementCostText;
    private void OnMouseUp()
    {
        //generate and visual the path to x and y
        float totalMovementCost = map.GeneratePathTo(_tileX, _tileY);

        if (totalMovementCost < 60)
        {
            //movementCostText.text = "Move Cost: " + totalMovementCost.ToString();
            Debug.Log(totalMovementCost.ToString());
        }
        else
        {
            //movementCostText.text = "Cannot Reach Tile";
            Debug.Log("Cannot Reach Tile");
        }
        //todo -> update move cost UI !
    }

    public void SetTiles(int x, int y)
    {
        _tileX = x;
        _tileY = y;
    }
    
}
