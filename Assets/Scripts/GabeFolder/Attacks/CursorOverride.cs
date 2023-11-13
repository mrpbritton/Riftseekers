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
    [SerializeField, Tooltip("Distance away from the player the cursor will be")]
    private float cursorDistance = 10;
    [SerializeField, Tooltip("Cursor object")]
    private Transform cursorObject;
    public CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero; //default

    private PInput pInput;
    private Vector3 direction;
    private Transform player;

    private Vector3 cachedMousePosition;

    private void OnEnable()
    {
        pInput = new();
        pInput.Enable();

        Cursor.visible = cursorVisibility; 
        hotSpot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

        cachedMousePosition = Input.mousePosition;

        player = FindObjectOfType<CharacterController>().transform;
        pInput.Player.ControllerAim.performed += ctxt => SetController();
    }

    private void Update()
    {
        direction = pInput.Player.ControllerAim.ReadValue<Vector2>();
        direction = new Vector3(direction.x, 0, direction.y);
        direction = direction.normalized;
        Debug.Log(direction);

        if (Input.mousePosition != cachedMousePosition && direction != Vector3.zero)
        {
            cachedMousePosition = Input.mousePosition;
            isController = false;
        }

        if(isController)
        {
            if(Cursor.visible)
                Cursor.visible = false;

            if (player == null)
            {
                player = FindObjectOfType<CharacterController>().transform;
            }

            if(direction == Vector3.zero)
            {
                cursorObject.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log(cursorObject.gameObject.activeInHierarchy);
                if (!cursorObject.gameObject.activeInHierarchy)
                {
                    cursorObject.gameObject.SetActive(true);
                }

                cursorObject.localPosition = new Vector3(direction.x * cursorDistance,
                                                         direction.y,
                                                         direction.z * cursorDistance);
            }
        }
        else
        {
            Cursor.visible = true;
        }
    }

    private void SetController()
    {
        isController = true;
    }

    private void OnDisable()
    {
        pInput.Disable();
        Cursor.visible = !cursorVisibility;
        pInput.Player.ControllerAim.started -= ctxt => SetController();
    }

}
