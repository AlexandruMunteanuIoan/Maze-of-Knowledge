using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private MazeGenerator mazeGenerator;
    private BookSpawner bookSpawner;
    private PlayerController playerController;
    private QuestionManager questionManager;

    private int MazeWidth;
    private int MazeHeight;
    private int WallSize;
    private MazeCell _mazeCellPrefab;

    private List<CellCenter> mazeCellsList;

    public static GameManager Instance { get; private set; }

    public GameManagerConfig config;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadConfig();
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadConfig()
    {
        config = Resources.Load<GameManagerConfig>("GameManagerConfig");

        if (config == null)
        {
            Debug.LogError("Failed to load GameManagerConfig from Resources folder.");
        }
        else
        {
            Debug.Log("Config loaded successfully: " + config.name);
        }
    }

    private void InitializeComponents()
    {
        mazeGenerator = FindObjectOfType<MazeGenerator>();
        if (mazeGenerator == null)
        {
            Debug.LogError("MazeGenerator not found in scene.");
            // Optionally, create a new instance or handle the error
        }

        bookSpawner = FindObjectOfType<BookSpawner>();
        if (bookSpawner == null)
        {
            Debug.LogError("BookSpawner not found in scene.");
            // Optionally, create a new instance or handle the error
        }

        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found in scene.");
            // Optionally, create a new instance or handle the error
        }

        questionManager = FindObjectOfType<QuestionManager>();
        if (questionManager == null)
        {
            Debug.LogError("QuestionManager not found in scene.");
            // Optionally, create a new instance or handle the error
        }
    }

    public void InitializeGame()
    {
        LoadMainMenu();
    }

    public void LoadMainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MenuScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            InitializeComponents();
            SetupNewMaze();
        }
    }

    private void SetupNewMaze()
    {
        // Ensure mazeGenerator is properly initialized
        if (mazeGenerator == null)
        {
            Debug.LogError("MazeGenerator is not initialized.");
            return;
        }

        mazeCellsList = mazeGenerator.CreateMaze(MazeWidth, MazeHeight, WallSize, _mazeCellPrefab);

        // Perform other setup based on game logic
        questionManager.LoadQuestions(config.quizFile);
        List<Question> selectedQuestions = questionManager.SelectQuestions(10);

        playerController.SpawnPlayerInMaze(mazeCellsList, WallSize, MazeHeight, MazeWidth);
        bookSpawner.SpawnBooks(selectedQuestions, mazeCellsList, WallSize);

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
