using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class MazeGenerator : MonoBehaviour
{
    //


    //[SerializeField] private TextAsset quizFile;
    //[SerializeField] private TextMeshProUGUI QuestionText;
    //[SerializeField] private TextMeshProUGUI scoreTxt;
    //[SerializeField] private GameObject QuizPanel;
    //[SerializeField] private GameObject GoPanel;
    //[SerializeField] private GameObject[] options;

    //private static List<Question> QnA = new List<Question>();
    [SerializeField] private GameObject[] topDecorObjects;

    [SerializeField] private GameObject plane;
    [SerializeField] private Material planeMaterial;

    private const int PlaneDefaultX = 10;
    private const int PlaneDefaultY = 10;
    private const int PlaneMaterialScaleFactor = 5;
    //private int currentQuestion;
    //private int totalQ;
    //private int score;


    private int _mazeWidth;
    private int _mazeDepth;
    private int _wallSize;
    private MazeCell _mazeCellPrefab;
    private MazeCell[,] _mazeGrid;
    private List<CellCenter> _cellCenters;

    private void InitializeMaze()
    {
        float cellHalfSize = _wallSize / 2f;

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                Vector3 cellPosition = new Vector3(x * _wallSize + cellHalfSize, 0, z * _wallSize + cellHalfSize);
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, cellPosition, Quaternion.identity, transform);
                _mazeGrid[x, z].Position = cellPosition;
                _cellCenters.Add(new CellCenter(cellPosition));

                if (Random.Range(1, 100) > 40)
                {
                    int objectIndex = Random.Range(0, topDecorObjects.Length);
                    Vector3 local = _mazeGrid[x, z].topDecorPlaceHolder.transform.position;
                    var decorObject = Instantiate(topDecorObjects[objectIndex], local, Quaternion.identity, transform);

                    float resizeMultiplier = Random.Range(1.5f, 2.0f);
                    decorObject.transform.localScale = new Vector3(resizeMultiplier, resizeMultiplier, resizeMultiplier);

                    Material decorMaterial = Resources.Load<Material>("Maze Material");
                    if (decorMaterial != null)
                    {
                        decorObject.GetComponent<MeshRenderer>().material = decorMaterial;
                    }
                    else
                    {
                        Debug.LogError("Material not existing");
                    }

                    _mazeGrid[x, z].topDecorPlaceHolder = decorObject;
                }
            }
        }
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;
        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);
            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell).ToList();
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x / _wallSize;
        int z = (int)currentCell.transform.position.z / _wallSize;

        if (x + 1 < _mazeWidth && !_mazeGrid[x + 1, z].IsVisited)
            yield return _mazeGrid[x + 1, z];
        if (x - 1 >= 0 && !_mazeGrid[x - 1, z].IsVisited)
            yield return _mazeGrid[x - 1, z];
        if (z + 1 < _mazeDepth && !_mazeGrid[x, z + 1].IsVisited)
            yield return _mazeGrid[x, z + 1];
        if (z - 1 >= 0 && !_mazeGrid[x, z - 1].IsVisited)
            yield return _mazeGrid[x, z - 1];
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null) return;

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
        }
        else if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
        }
        else if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
        }
        else if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
        }
    }

    private void GeneratePlane()
    {
        planeMaterial.mainTextureScale = new Vector2(_mazeWidth * PlaneMaterialScaleFactor, _mazeDepth * PlaneMaterialScaleFactor);
        plane.transform.position = new Vector3(((_mazeWidth * _wallSize) / 2) - 2.5f, 0, ((_mazeDepth * _wallSize) / 2) - 2.5f);
        plane.transform.localScale = new Vector3((_mazeWidth * _wallSize) / PlaneDefaultX, 1, (_mazeDepth * _wallSize) / PlaneDefaultY);
    }

    public static CellCenter GetRandomAvailableCellCenter(List<CellCenter> mazeCellsList)
    {
        var availableCenters = mazeCellsList.Where(cell => !cell.IsOccupied).ToList();
        if (availableCenters.Count == 0) return null;

        int randomIndex = Random.Range(0, availableCenters.Count);
        return availableCenters[randomIndex];
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Application has been closed.");
    }

    internal void ClearMaze()
    {
        throw new System.NotImplementedException();
    }

    public List<CellCenter> CreateMaze(int _mazeWidth, int _mazeDepth, int _wallSize, MazeCell _mazeCellPrefab)
    {
        this._mazeWidth = _mazeWidth;
        this._mazeDepth = _mazeDepth;
        this._wallSize = _wallSize;
        this._mazeCellPrefab = _mazeCellPrefab;

        _cellCenters = new List<CellCenter>();
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        InitializeMaze();
        GenerateMaze(null, _mazeGrid[0, 0]);
        GeneratePlane();

        return _cellCenters;
    }
}
