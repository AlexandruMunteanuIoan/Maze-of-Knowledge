using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public MazeGenerator mazeGenerator;
    public BookSpawner bookSpawner;
    public PlayerController playerController;
    public QuestionManager questionManager;

    public int MazeWidth;
    public int MazeHeight;
    public const int WallSize = 4;

    private List<CellCenter> mazeCellsList;

    [SerializeField] private MazeCell _mazeCellPrefab;


    private void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        LoadMainMenu();
    }
    private void LoadMainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MenuScene");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
        // GameScene setup happens in OnSceneLoaded
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            SetupNewMaze();
        }
    }

    private void SetupNewMaze()
    {
        mazeGenerator.ClearMaze();
        mazeCellsList = mazeGenerator.CreateMaze(MazeWidth, MazeHeight, WallSize, _mazeCellPrefab);

        questionManager.LoadQuestions();
        List<Question> selectedQuestions = questionManager.SelectQuestions(10);

        bookSpawner.SpawnBooks(selectedQuestions, mazeCellsList, WallSize);

        playerController.SpawnPlayerInMaze(mazeCellsList, WallSize);

        // Additional setup for UI and game state
    }

    public void OnQuizCompleted(List<Question> unansweredQuestions)
    {
        if (unansweredQuestions.Count == 0)
        {
            // All questions answered correctly, generate next maze
            SetupNewMaze();
        }
        else
        {
            // Handle unanswered questions
            bookSpawner.RespawnBooks(unansweredQuestions);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
