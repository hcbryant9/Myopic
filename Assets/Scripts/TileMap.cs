using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileMap : MonoBehaviour
{
    public GameObject selectedUnit;
    private Unit _unit;

    [SerializeField] TileType[] tileTypes; //tile information
    [SerializeField] private int _mapSizeX = 60;
    [SerializeField] private int _mapSizeY = 60;
    enum TerrainType {Grass,Swamp,Mountain};

    private Node[,] _graph; //pathfinding graph
    private TerrainType[,] _tiles; //tile type
    
   
    private void Start()
    {
        _unit = selectedUnit.GetComponent<Unit>();
        _unit.SetTile((int)selectedUnit.transform.position.x, (int)selectedUnit.transform.position.y);
        _unit.SetTileMap(this);
        GenerateMapData();
        GeneratePathFindingGraph();
        GenerateMapVisuals();
    }

    //function for getting the Tile Elevation of a certain tile
    public float GetTileElevation(int x, int y)
    {
        return tileTypes[(int)_tiles[x, y]].elevation;
    }
    
    private void GeneratePathFindingGraph()
    {
        
        _graph = new Node[_mapSizeX, _mapSizeY]; //initialize the array


        for (int x = 0; x < _mapSizeX; x++) //initialize a node for each spot in array
        {
            for (int y = 0; y < _mapSizeY; y++)
            {
                _graph[x, y] = new Node();
                _graph[x, y].x = x;
                _graph[x, y].y = y;
            }
        }
        // now that all nodes exist, calculate their neighbors
        for (int x = 0; x < _mapSizeX; x++)
        {
            for (int y = 0; y < _mapSizeY; y++)
            {
                //4-way connected map
                if (x > 0)
                    _graph[x, y].edges.Add(_graph[x - 1, y]);
                if(x < _mapSizeY - 1)
                    _graph[x, y].edges.Add(_graph[x + 1, y]);
                if (y > 0)
                    _graph[x, y].edges.Add(_graph[x, y - 1]);
                if (y < _mapSizeY - 1)
                    _graph[x, y].edges.Add(_graph[x, y + 1]);
            }
        }
    }
    private void GenerateMapVisuals()
    {
        for (int x = 0; x < _mapSizeX; x++)
        {
            for (int y = 0; y < _mapSizeY; y++)
            {
                TileType tt = tileTypes[(int)_tiles[x, y]];

                GameObject go = Instantiate(tt.tileVisualPrefab, new Vector3(x, 0, y), Quaternion.identity);
                ClickTile ct = go.GetComponent<ClickTile>();
                ct.SetTiles(x, y);
                ct.map = this;
            }
        }
    }

    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        return new Vector3(x, 0, y);
    }

    public bool UnitCanEnterTile(int x, int y)
    {
        return tileTypes[(int)_tiles[x, y]].isWalkable;
    }
    public float GeneratePathTo(int x, int y)
    {
        _unit.SetCurrentPath(null); // Clear old path

        if (!UnitCanEnterTile(x, y))
        {
            // Clicked on an impassable tile (e.g., mountain)
            return Mathf.Infinity;
        }

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
        List<Node> unvisited = new List<Node>();

        Node source = _graph[_unit.GetTileX, _unit.GetTileY];
        Node target = _graph[x, y];

        dist[source] = 0;
        prev[source] = null;

        // Initialize distances to infinity and add nodes to unvisited list
        foreach (Node v in _graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            unvisited.Add(v);
        }

        while (unvisited.Count > 0)
        {
            // Get node with the minimum distance
            Node u = unvisited.OrderBy(n => dist[n]).FirstOrDefault();

            if (u == null || dist[u] == Mathf.Infinity)
            {
                break; // No valid path to target
            }

            unvisited.Remove(u);

            foreach (Node v in u.edges)
            {
                float alt = dist[u] + CostToEnterTile(v.x, v.y);
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        // Calculate total movement cost to the target
        float totalCost = dist.ContainsKey(target) ? dist[target] : Mathf.Infinity;

        if (totalCost != Mathf.Infinity)
        {
            // Construct the path based on prev dictionary
            List<Node> currentPath = new List<Node>();
            Node curr = target;
            while (curr != null)
            {
                currentPath.Add(curr);
                curr = prev[curr];
            }
            currentPath.Reverse();

            // Set the current path for the unit
            _unit.SetCurrentPath(currentPath);
        }

        return totalCost;
    }


    private float CostToEnterTile(int x, int y)
    {
        if(UnitCanEnterTile(x,y) == false)
        {
            return Mathf.Infinity;
        }
        TileType tt = tileTypes[ (int)_tiles[x, y] ];
        return tt.movementCost + tt.elevation; //cost to move into tile is the movement cost + elevation
    }
    private void GenerateMapData()
    {
        //allocate map tiles
        _tiles = new TerrainType[_mapSizeX, _mapSizeY];

        //initialize our map (all grass)
        for (int x = 0; x < _mapSizeX; x++)
        {
            for (int y = 0; y < _mapSizeY; y++)
            {
                _tiles[x, y] = 0;
            }
        }

        _tiles[4, 6] = TerrainType.Mountain;
        _tiles[4, 7] = TerrainType.Mountain;
        _tiles[4, 8] = TerrainType.Mountain;
        _tiles[5, 7] = TerrainType.Swamp;
        _tiles[5, 8] = TerrainType.Swamp;
        _tiles[4, 5] = TerrainType.Swamp;
        _tiles[4, 4] = TerrainType.Swamp;
        _tiles[4, 3] = TerrainType.Swamp;
    }
}
