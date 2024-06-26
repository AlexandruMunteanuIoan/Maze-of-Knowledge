using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeCell _mazeCellPrefab;
    [SerializeField] private int _mazeWidth;
    [SerializeField] private int _mazeDepth;
    [SerializeField] private GameObject[] topDecorObjects;
    [SerializeField] private GameObject BookPrefab;
    [SerializeField] private GameObject plane;
    [SerializeField] private Material planeMaterial;
    [SerializeField] private GameObject PlayerObject;
    [SerializeField] private TextAsset quizFile;
    [SerializeField] private TextMeshProUGUI QuestionText;
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private GameObject QuizPanel;
    [SerializeField] private GameObject GoPanel;
    [SerializeField] private GameObject[] options;

    private MazeCell[,] _mazeGrid;
    private List<CellCenter> _cellCenters;
    private static List<QandA> QnA = new List<QandA>();
    private const int WallSize = 4;
    private const int PlaneDefaultX = 10;
    private const int PlaneDefaultY = 10;
    private const int PlaneMaterialScaleFactor = 5;
    private int currentQuestion;
    private int totalQ;
    private int score;

    private class CellCenter
    {
        public Vector3 Position { get; set; }
        public bool IsOccupied { get; set; }

        public CellCenter(Vector3 position)
        {
            Position = position;
            IsOccupied = false;
        }
    }

    void Start()
    {
        _cellCenters = new List<CellCenter>();
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        InitializeMaze();
        GenerateMaze(null, _mazeGrid[0, 0]);
        GeneratePlane();
        SpawnPlayer();

        if (quizFile != null)
        {
            ParseDocument(quizFile);
            SelectRandomQuestions();
            totalQ = QnA.Count;
            SpawnQuestionsInMaze();
        }
        else
        {
            Debug.LogError("Quiz file is not assigned in the Inspector.");
        }
    }

    private void InitializeMaze()
    {
        float cellHalfSize = WallSize / 2f;

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                Vector3 cellPosition = new Vector3(x * WallSize + cellHalfSize, 0, z * WallSize + cellHalfSize);
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
        int x = (int)currentCell.transform.position.x / WallSize;
        int z = (int)currentCell.transform.position.z / WallSize;

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
        plane.transform.position = new Vector3(((_mazeWidth * WallSize) / 2) - 2.5f, 0, ((_mazeDepth * WallSize) / 2) - 2.5f);
        plane.transform.localScale = new Vector3((_mazeWidth * WallSize) / PlaneDefaultX, 1, (_mazeDepth * WallSize) / PlaneDefaultY);
    }

    private void SpawnPlayer()
    {
        CellCenter playerCellCenter = GetRandomAvailableCellCenter();
        if (playerCellCenter != null)
        {
            playerCellCenter.IsOccupied = true;
            Vector3 playerPosition = new Vector3(playerCellCenter.Position.x - WallSize / 2f, 1.0f, playerCellCenter.Position.z - WallSize / 2f);
            PlayerObject.transform.position = playerPosition;
        }
    }

    private void SpawnQuestionsInMaze()
    {
        foreach (var q in QnA)
        {
            CellCenter questionCellCenter = GetRandomAvailableCellCenter();
            if (questionCellCenter == null || questionCellCenter.IsOccupied)
            {
                break;
            }

            questionCellCenter.IsOccupied = true;
            Vector3 spawnPosition = new Vector3(questionCellCenter.Position.x - WallSize / 2f, 1.5f, questionCellCenter.Position.z - WallSize / 2f);
            Instantiate(BookPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private CellCenter GetRandomAvailableCellCenter()
    {
        var availableCenters = _cellCenters.Where(cell => !cell.IsOccupied).ToList();
        if (availableCenters.Count == 0) return null;

        int randomIndex = Random.Range(0, availableCenters.Count);
        return availableCenters[randomIndex];
    }

    private void ParseDocument(TextAsset quizFile)
    {
        QnA.Clear();
        string[] lines = quizFile.text.Split('\n');
        QandA currentQandA = null;

        foreach (string line in lines)
        {
            if (line.StartsWith("**Hint:**"))
            {
                if (currentQandA != null)
                {
                    QnA.Add(currentQandA);
                }
                currentQandA = new QandA();
                currentQandA.Hint = line.Replace("**Hint:**", "").Trim();
            }
            else if (line.StartsWith("**Întrebarea:**"))
            {
                currentQandA.Question = line.Replace("**Întrebarea:**", "").Trim();
            }
            else if (line.StartsWith("**Răspunsuri:**"))
            {
                currentQandA.Answers = new string[4];
            }
            else if (line.StartsWith("1."))
            {
                currentQandA.Answers[0] = line.Substring(3).Trim();
            }
            else if (line.StartsWith("2."))
            {
                currentQandA.Answers[1] = line.Substring(3).Trim();
            }
            else if (line.StartsWith("3."))
            {
                currentQandA.Answers[2] = line.Substring(3).Trim();
            }
            else if (line.StartsWith("4."))
            {
                currentQandA.Answers[3] = line.Substring(3).Trim();
            }
            else if (line.StartsWith("**Indexul:**"))
            {
                currentQandA.CorrectAnswer = int.Parse(line.Replace("**Indexul:**", "").Trim()) - 1;
            }
        }

        if (currentQandA != null)
        {
            QnA.Add(currentQandA);
        }
    }

    private void SelectRandomQuestions()
    {
        QnA = QnA.OrderBy(_ => Random.Range(0, QnA.Count)).Take(10).ToList();
    }

    private void GenerateQuestion()
    {
        if (QnA.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.Count);
            QuestionText.text = QnA[currentQuestion].Question;
            SetAnswer();
        }
        else
        {
            GameOver();
        }
    }

    private void SetAnswer()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnsersScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = QnA[currentQuestion].Answers[i];

            if (QnA[currentQuestion].CorrectAnswer == i)
            {
                options[i].GetComponent<AnsersScript>().isCorrect = true;
            }
        }
    }

    public void Correct()
    {
        score += 1;
        QnA.RemoveAt(currentQuestion);
        GenerateQuestion();
    }

    public void Wrong()
    {
        QnA.RemoveAt(currentQuestion);
        GenerateQuestion();
    }

    public void GameOver()
    {
        QuizPanel.SetActive(false);
        GoPanel.SetActive(true);
        scoreTxt.text = $"{score}/{totalQ}";
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Application has been closed.");
    }
}
