using UnityEngine;

[CreateAssetMenu(fileName = "GameManagerConfig", menuName = "Config/GameManagerConfig")]
public class GameManagerConfig : ScriptableObject
{
    public int MazeWidth;
    public int MazeHeight;
    public int WallSize;
    public MazeCell mazeCellPrefab;
    public GameObject playerObj;
    public TextAsset quizFile;

    public float moveSpeed;
    public float groundDrag;
    public float airMultiplier;
    public float playerHeight;
    public LayerMask whatIsGround;
}
