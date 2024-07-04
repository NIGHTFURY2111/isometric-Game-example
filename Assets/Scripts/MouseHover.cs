using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseHover : MonoBehaviour
{
    public TMP_Text Text;

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
        camera = Camera.main;
    }
    private void Update()
    {
        if (mouseClick.WasPressedThisFrame())
        {
            //creating a ray from the mouse position to world coordinates
            Ray ray = camera.ScreenPointToRay(mousePosi.ReadValue<Vector2>()); 
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();
                
                //displaying the coordinates
                if (tileInfo != null) { Text.text = "Position: " + tileInfo.x + ", " + tileInfo.y + ""; }
            }
        }
    }
}
