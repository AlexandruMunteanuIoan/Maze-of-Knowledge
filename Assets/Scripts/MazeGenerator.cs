using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Numerics;
using System.Threading;
using UnityEditorInternal;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private int _mazeWidth;

    [SerializeField]
    private int _mazeDepth;

    private MazeCell[,] _mazeGrid;

    public Transform Maze;

    public UnityEngine.GameObject plane;
    private int planeDefaultX = 10;
    private int planeDefaultY = 10;
    private int wallSize = 4;

    void Start()
    {
        _mazeCellPrefab.SetDecor();
        plane = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Plane);

        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];
        
        for(int x = 0; x < _mazeWidth; x++) 
        {
            for(int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new UnityEngine.Vector3(x * wallSize, 0, z * wallSize), UnityEngine.Quaternion.identity, Maze);
            }
        }

        GenerateMaze(null, _mazeGrid[0, 0]);

        plane.transform.position = new UnityEngine.Vector3((_mazeDepth/2)-0.5f, 0, (_mazeWidth/2)-0.5f);
        plane.transform.localScale = new UnityEngine.Vector3(_mazeDepth/planeDefaultX, 1, _mazeWidth/planeDefaultY);
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

        return unvisitedCells.OrderBy(_ => Random.Range(1,10)).FirstOrDefault();
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

    void Update()
    {
        
    }
}
