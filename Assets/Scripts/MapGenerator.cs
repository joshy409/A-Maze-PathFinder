using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// to do
// traversal cost for blocked by walls

// generates the tilemap, handles state

namespace TowerDefense
{
    public class MapGenerator : MonoBehaviour
    {       
        public int width;
        public int height;
        public TileType currentSelection;
        public GameObject basePrefab;
        //public int highlightOpacity;
        //public float spawnTime;
        //public int wallTraversalCost;
        //public int turretHealth;

        public Color groundColor;
        public Color wallColor;
        //public Color turretColor;
        //public Color spawnColor;
        //public Color objectiveColor;

        public GameObject ground;
        public GameObject wall;
        //public GameObject turret;
        //public GameObject spawn;
        //public GameObject objective;
        //public GameObject enemy;

        public Tile[,] tiles;
        //Node[,] nodes;
        //Node[,] nodes_CanBreak;

        Stage stage;
        int typeLength = 2;

        //AStar pathfinder;
        
        // Use this for initialization
        void Start()
        {
            stage = Stage.build;
            tiles = new Tile[height, width];
            //nodes = new Node[height, width];
            //nodes_CanBreak = new Node[height, width];

            GenerateTileMap();
            InitializeData();
           
            //pathfinder = new AStar();
            
            Camera.main.transform.position = new Vector3(width / 2f - 0.5f, Mathf.Max(width, height), height / 2f - 0.5f);
        }

        // initializes the tilemap
        void GenerateTileMap()
        {
            float s = 0.9f; // tile size
            basePrefab.transform.localScale = new Vector3(s,s,s);

            GameObject tileObj;

            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    tileObj = Instantiate(basePrefab, new Vector3(x, 0, y), Quaternion.Euler(90,0,0));
                    tiles[y, x] = tileObj.GetComponent<Tile>();
                    tiles[y,x].Set(TileType.ground, new Index(x, y), tileObj, this);

                    //nodes[y, x] = new Node(1, new Index(x, y));
                    //nodes_CanBreak[y, x] = new Node(1, new Index(x, y));
                }
            }
        }

        // makes a 2d array of nodes given conditions
        //Node[,] GenerateNodeMap(bool canBreakWalls)
        //{
        //    Node[,] tempNodes = (Node[,])nodes.Clone();
        //    for (int y = 0; y < height; y++)
        //    {
        //        for (int x = 0; x < width; x++)
        //        {
        //            tempNodes[y, x].nextNodes.Clear();
        //            // for now only ground traversal, no breaking things

        //            // FIX
        //            if (y > 0 && tiles[y - 1, x].type != TileType.wall && tiles[y - 1, x].type != TileType.turret) { tempNodes[y, x].nextNodes.Add(nodes[y - 1, x]); }
        //            if (y < height - 1 && tiles[y + 1, x].type != TileType.wall && tiles[y + 1, x].type != TileType.turret) { tempNodes[y, x].nextNodes.Add(nodes[y + 1, x]); }
        //            if (x > 0 && tiles[y, x - 1].type != TileType.wall && tiles[y, x - 1].type != TileType.turret) { tempNodes[y, x].nextNodes.Add(nodes[y, x - 1]); }
        //            if (x < width - 1 && tiles[y, x + 1].type != TileType.wall && tiles[y, x + 1].type != TileType.turret) { tempNodes[y, x].nextNodes.Add(nodes[y, x + 1]); }
        //        }
        //    }
        //    return tempNodes;
        //}

        // makes type - color dictionary
        void InitializeData()
        {
            Tile.colors = new Dictionary<TileType, Color>();
            Tile.colors.Add(TileType.ground, groundColor);
            Tile.colors.Add(TileType.wall, wallColor);
            //Tile.colors.Add(TileType.turret, turretColor);
            //Tile.colors.Add(TileType.spawn, spawnColor);
            //Tile.colors.Add(TileType.objective, objectiveColor);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return)) { InitRun(); }

            switch (stage)
            {
                case Stage.build:
                    UpdateBuild();
                    break;
                case Stage.defend:
                    UpdateRun();
                    break;
            }
        }

        void InitBuild()
        {
            
        }

        // initializes run stage
        void InitRun()
        {
            // instantiate all
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    GameObject obj;
                    switch (tiles[y, x].type)
                    {
                        case TileType.ground:
                            obj = Instantiate(ground, tiles[y, x].transform.position, ground.transform.rotation);
                            break;
                        case TileType.wall:
                            obj = Instantiate(wall, tiles[y, x].transform.position, wall.transform.rotation);
                            break;
                        default:
                            return;                       
                    }
                }
            }
        }

        // updates build stage
        void UpdateBuild()
        {
            if(Input.mouseScrollDelta.y > 0)
            {
                currentSelection++;
                if((int)currentSelection >= typeLength) { currentSelection = 0; }
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                currentSelection--;
                if (currentSelection < 0) { currentSelection = (TileType)(typeLength - 1); }
            }
        }

        void UpdateRun()
        {

        }
    }
}

