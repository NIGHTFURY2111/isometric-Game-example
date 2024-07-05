using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public ObstacleData obstacleData;

    [HideInInspector]
    public Controls Controls;

    private int gridSize;
    private bool _isMoving = false;
    private EnemyAI enemyAI;
    private GameObject enemy;
    private Vector3 targetPosition;
    private Queue<Vector3> path;

    private TMP_Text gridPositionText;

    private InputAction mousePosi;
    private InputAction mouseClick;

    private new Camera camera;

    #region Initializing the input system
    private void Awake()
    {
        Controls = new Controls();

        mousePosi = Controls.Player.MousePosi;
        mouseClick = Controls.Player.MouseClick;
    }

    private void OnEnable()
    {
        mousePosi.Enable();
        mouseClick.Enable();
    }

    private void OnDisable()
    {
        mousePosi.Disable();
        mouseClick.Disable();
    }
    #endregion

    private void Start()
    {
        targetPosition = transform.position;
        camera = Camera.main;

        // Get references to necessary components
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyAI = enemy.GetComponent<EnemyAI>();
        gridSize = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>().gridSize;
        gridPositionText = GameObject.FindGameObjectWithTag("GridLocation").GetComponent<TMP_Text>();
    }

    private void Update()
    {
        // Create a ray from the camera to the mouse position
        Ray ray = camera.ScreenPointToRay(mousePosi.ReadValue<Vector2>());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();

            // Display the coordinates of the tile the mouse is over
            if (tileInfo != null)
            {
                gridPositionText.text = "Position: " + tileInfo.X + ", " + tileInfo.Y;
            }

            // Move the player if the mouse is clicked, the player is not already moving, and the enemy is not moving
            if (!_isMoving && mouseClick.WasPressedThisFrame() && !enemyAI.isMoving)
            {
                Vector3 target = hit.collider.transform.position;
                Vector2Int startGridPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
                Vector2Int targetGridPos = new Vector2Int(Mathf.RoundToInt(target.x), Mathf.RoundToInt(target.z));

                // Pass the enemy's position as a dynamic obstacle to the pathfinding method
                path = new Queue<Vector3>(AStarPathfinding.FindPath(startGridPos, targetGridPos, obstacleData.obstacleGrid, gridSize,
                    new List<Vector2Int> {
                        new Vector2Int(Mathf.RoundToInt(enemy.transform.position.x), Mathf.RoundToInt(enemy.transform.position.z))
                    }));

                // Start moving along the path if a valid path is found
                if (path.Count > 0)
                {
                    StartCoroutine(MoveAlongPath());
                }
            }
        }
    }

    // Coroutine to move the player along the path
    private IEnumerator MoveAlongPath()
    {
        _isMoving = true;
        while (path.Count > 0)
        {
            targetPosition = path.Dequeue();
            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
        }
        _isMoving = false;
    }

    // Property to check if the player is currently moving
    public bool IsMoving { get => _isMoving; }
}
