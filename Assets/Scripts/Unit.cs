using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    
    private int _tileX;
    private int _tileY;
    private int _moveSpeed = 2;
    private float verticalScaleFactor = 1f;
    private List<Node> _currentPath = null; // assuming one unit
    private TileMap map;
    private Race race;
    [SerializeField] GameObject line;

    private void Start()
    {
        
    }
    /*
     * Todo - handle how to show the path to the user
     */
    private void Update()
    {
        if (_currentPath != null)
        {
            int currNode = 0;
            
            while(currNode < _currentPath.Count - 1)
            {
                Vector3 start = map.TileCoordToWorldCoord(_currentPath[currNode].x, _currentPath[currNode].y) +
                    new Vector3(0,1f,0);
                Vector3 end = map.TileCoordToWorldCoord(_currentPath[currNode+1].x, _currentPath[currNode+1].y) +
                    new Vector3(0, 1f, 0); 
                
                Instantiate(line, end, Quaternion.identity);

                currNode++;
                
            }
        }
        
    }
    private int GetMoveSpeed { get { return _moveSpeed; } }
    private void SetMoveSpeed(int speed) { _moveSpeed = speed; }
    public void SetTile(int x, int y)
    {
        _tileX = x;
        _tileY = y;
    }
    public int GetTileX{ get{ return _tileX; } }
    public int GetTileY { get { return _tileY; } }
    public void SetTileMap(TileMap tilemap) { map = tilemap; }
    public void SetCurrentPath(List<Node> path)
    {
        _currentPath = path;
    }

    /* --- to do ---
    *  implement movement mechanics here 
    *  how many tiles should the user be able to move each turn?
    */
    public void MoveNextTile()
    {
        if (_currentPath == null || _currentPath.Count == 0)
        {
            return; // No valid path or path completed
        }

        // Remove current/first node from the path
        _currentPath.RemoveAt(0);

        if (_currentPath.Count > 0)
        {
            // Get coordinates of the next node in the path
            Node nextNode = _currentPath[0];
            int nextTileX = nextNode.x;
            int nextTileY = nextNode.y;

            // Calculate target world position for the next tile
            Vector3 targetPosition = map.TileCoordToWorldCoord(nextTileX, nextTileY);

            // Retrieve the elevation of the next tile
            float tileElevation = map.GetTileElevation(nextTileX, nextTileY);

            // Calculate the vertical position based on tile elevation
            float yPosition = tileElevation * verticalScaleFactor;

            // Move the character to the target position with adjusted y position
            transform.position = new Vector3(targetPosition.x, yPosition, targetPosition.z);

            // Update the current tile coordinates of the unit
            this.SetTile(nextTileX, nextTileY);
        }
        else
        {
            // Path completed (reached the target)
            _currentPath = null;
        }
    }


}
