using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    private MazeGenerator mazeGenerator;
    private BookSpawner bookSpawner;
    private PlayerController playerController;
    private QuestionManager questionManager;
    private TestPointSpawn testPoint;
    private GameTimer gameTimer;
    private BookStatisticController bookStatisticCanvas;

    private int MazeWidth;
    private int MazeHeight;
    private int WallSize;
    private MazeCell _mazeCellPrefab;

    private List<CellCenter> mazeCellsList;

    public int currentNumOfQuestions { get; private set; }

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
            currentNumOfQuestions = 10;
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
            return;
        }

        MazeWidth = config.MazeWidth;
        MazeHeight = config.MazeHeight;
        WallSize = config.WallSize;
        _mazeCellPrefab = config.mazeCellPrefab;
        
        Debug.Log("Config loaded successfully: " + config.name);
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

        testPoint = FindObjectOfType<TestPointSpawn>();
        if(testPoint == null)
        {
            Debug.LogError("TestPoint not found in scene.");
        }

        gameTimer = FindObjectOfType<GameTimer>();
        if(gameTimer == null)
        {
            Debug.LogError("GameTimer not found in scene.");
        }

        bookStatisticCanvas = FindObjectOfType<BookStatisticController>();
        if (bookStatisticCanvas == null)
        {
            Debug.LogError("BookStatisticCanvas not found in scene");
        }

        // locally checked - neede only as a convenience
        CanvasGroup canvasGroup = FindObjectOfType<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup not found in scene.");
        }
        else
        {
            canvasGroup.alpha = 1.0f;
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
            SetupNewMazeEnv();
        }
    }

    private void SetupNewMazeEnv()
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
        List<Question> selectedQuestions = questionManager.SelectQuestions(currentNumOfQuestions);

        playerController.SpawnPlayerInMaze(ref mazeCellsList, WallSize, MazeHeight, MazeWidth);
        bookSpawner.SpawnBooks(selectedQuestions, ref mazeCellsList, WallSize);
        testPoint.SpawnPoint(ref mazeCellsList, WallSize);

        // Additional setup for UI and game state
    }

    public void OnQuizCompleted(List<Question> unansweredQuestions)
    {
        if (unansweredQuestions.Count == 0)
        {
            GameObject[] bookInScene = GameObject.FindGameObjectsWithTag("Book");
            if(bookInScene.Length != 0)
            {
                return;
            }

            // All questions answered correctly, generate next maze
            throw new NotImplementedException();

            Time.timeScale = 1f; // Resume normal time scale

            Scene currentScene = SceneManager.GetActiveScene();
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if (obj.scene == currentScene)
                {
                    Destroy(obj);
                }
            }

            // Load the menu scene
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            // Handle unanswered questions
            bookSpawner.SpawnBooks(unansweredQuestions, ref mazeCellsList, WallSize);
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

    private void Update()
    {
        // Check for B key press to toggle the book statistic canvas visibility
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (bookStatisticCanvas != null)
            {
                if (bookStatisticCanvas.IsActive)
                {
                    bookStatisticCanvas.DeactivateBookStatisticCanvas();
                }
                else
                {
                    bookStatisticCanvas.ActivateBookStatisticCanvas();
                }
            }
            else
            {
                Debug.LogWarning("BookStatisticCanvas is not assigned or found.");
            }
        }
    }

}
