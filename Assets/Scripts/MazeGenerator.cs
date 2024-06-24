using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private int _mazeWidth;

    [SerializeField]
    private int _mazeDepth;

    public GameObject[] topDecorObjects;

    public MazeCell[,] _mazeGrid;

    public UnityEngine.GameObject plane;
    private readonly int planeDefaultX = 10;
    private readonly int planeDefaultY = 10;
    private readonly int wallSize = 4;

    public UnityEngine.Material planeMaterial;
    private readonly int planeMaterialScaleFactor = 5;

    public UnityEngine.GameObject PlayerObject;

    void Start()
    {
        _mazeCellPrefab.SetDecor();

        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];
        
        for(int x = 0; x < _mazeWidth; x++) 
        {
            for(int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new UnityEngine.Vector3(x * wallSize, 0, z * wallSize), UnityEngine.Quaternion.identity, transform);
                _mazeGrid[x, z].Position = new UnityEngine.Vector3(x * wallSize, 0, z * wallSize);
                // Instantiate a new random item from the list on top of the column
                if (UnityEngine.Random.Range(1, 100) > 40)
                {
                    // Get an object index inside the object array
                    int objectIndex = UnityEngine. Random.Range(0, topDecorObjects.Length);

                    UnityEngine.Vector3 local = _mazeGrid[x, z].topDecorPlaceHolder.transform.position;
                    _mazeGrid[x, z].topDecorPlaceHolder = Instantiate(topDecorObjects[objectIndex], local, UnityEngine.Quaternion.identity, transform);

                    // Resize objects using random range value
                    float resizeMultiplier = UnityEngine.Random.Range(1.5f, 2.0f);
                    _mazeGrid[x, z].topDecorPlaceHolder.transform.localScale = new Vector3(resizeMultiplier, resizeMultiplier, resizeMultiplier);

                    // Apply material to the decor objects
                    Material DecorMaterial = (Material)Resources.Load("Maze Material", typeof(Material)) ?? throw new System.Exception("Material not existing");
                    _mazeGrid[x, z].topDecorPlaceHolder.GetComponent<MeshRenderer>().material = DecorMaterial;
                }
               
            }
        }

        // Shape the maze 
        GenerateMaze(null, _mazeGrid[0, 0]);

        // Generate the plane respective to maze size
        PlaneGenerate();

        // Spawn the player somewhere inside the maze
        SpawnPlayer();

        //_mazeGrid[0, 0].transform.position;
    }

    private void SpawnPlayer()
    {
        // Get random value inside the maze
        int playerPosX = UnityEngine.Random.Range(0, _mazeWidth);
        int playerPosY = UnityEngine.Random.Range(0, _mazeDepth);

        // Get cell position at that location in the grid
        Vector3 cellPosition = _mazeGrid[playerPosX, playerPosY].Position;

        // Increase spawn player location
        cellPosition.z += 1; // Above the ground
        cellPosition.y += 2.5f; // Middle of the cell on y axis
        cellPosition.x += 2.5f; // Middle of the cell on x axis

        // Set player position to that position
        PlayerObject.transform.position = new Vector3(cellPosition.x, cellPosition.y, cellPosition.z);
    }

    private void PlaneGenerate()
    {
        // Translate and resize the plane accordingly
        planeMaterial.mainTextureScale = new Vector2(_mazeWidth * planeMaterialScaleFactor, _mazeDepth * planeMaterialScaleFactor);
        plane.transform.position = new UnityEngine.Vector3((_mazeWidth * wallSize / 2) - 4.6f, 0, (_mazeDepth * wallSize / 2) - 4.6f);
        plane.transform.localScale = new UnityEngine.Vector3(_mazeWidth * wallSize / planeDefaultX, 1, _mazeDepth * wallSize / planeDefaultY);

    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if(nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        }while(nextCell != null);

    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCell(currentCell);

        return unvisitedCells.OrderBy(_ => UnityEngine.Random.Range(1,10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCell(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x / wallSize;
        int z = (int)currentCell.transform.position.z / wallSize;

        if( x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];

            if(cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }

        if(x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];

            if(cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if(z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];

            if(cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }

        if(z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];       

            if(cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if(previousCell == null)
        {
            return ;
        }

        if(previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if(previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;

        }

        if(previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }

        if(previousCell.transform.position.z > currentCell.transform.position.z)
        {
            
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }
    }
}
