using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorOverride : MonoBehaviour
{
    [SerializeField, Tooltip("Using controller or not")]
    private bool isController;
    [SerializeField, Tooltip("Cursor sprite")]
    private Texture2D cursorTexture;
    [SerializeField, Tooltip("Visibility of the cursor")]
    private bool cursorVisibility;
    public CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero; //default

    private PInput pInput;
    private Vector3 direction;
    private Transform player;

    private void OnEnable()
    {
        pInput = new();
        pInput.Enable();

        Cursor.visible = cursorVisibility; 
        hotSpot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

        player = FindObjectOfType<CharacterController>().transform;
    }

    private void Update()
    {
        if(isController)
        {
            if(player == null)
            {
                player = FindObjectOfType<CharacterController>().transform;
            }
            direction = pInput.Player.ControllerAim.ReadValue<Vector3>();
            direction = direction.normalized;

        }
    }

    private void OnDisable()
    {
        pInput.Disable();
        Cursor.visible = !cursorVisibility;
    }

}
