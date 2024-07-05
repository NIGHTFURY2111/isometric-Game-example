using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public ObstacleData obstacleData;

    int gridSize;
    Vector3 targetPosition;
    bool isMoving = false;
    Queue<Vector3> path;

    TMP_Text Text;

    [HideInInspector]
    public Controls Controls;

    InputAction mousePosi;
    InputAction mouseClick;

    private new Camera camera;

    #region initializing the new input system
    private void Awake()
    {
        Controls = new();

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
        gridSize = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>().gridSize;
        Text = GameObject.FindGameObjectWithTag("GridLocation").GetComponent<TMP_Text>();
        camera = Camera.main;
    }

    private void Update()
    {
        //---------------creating a ray---------------
        Ray ray = camera.ScreenPointToRay(mousePosi.ReadValue<Vector2>());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();

            //---------------displaying the coordinates---------------
            if (tileInfo != null) { Text.text = "Position: " + tileInfo.x + ", " + tileInfo.y + ""; }

            //---------------Moving the player----------------
            if (!isMoving && mouseClick.WasPressedThisFrame())
            {
                Vector3 target = hit.collider.transform.position;
                Vector2Int startGridPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
                Vector2Int targetGridPos = new Vector2Int(Mathf.RoundToInt(target.x), Mathf.RoundToInt(target.z));

                path = new Queue<Vector3>(AStarPathfinding.FindPath(startGridPos, targetGridPos, obstacleData.obstacleGrid, gridSize));
                if (path.Count > 0)
                {
                    StartCoroutine(MoveAlongPath());
                }
            }
        }

    }

    private IEnumerator MoveAlongPath()
    {
        isMoving = true;
        while (path.Count > 0)
        {
            targetPosition = path.Dequeue();
            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
        }
        isMoving = false;
    }
}
