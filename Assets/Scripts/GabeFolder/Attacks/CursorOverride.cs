using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorOverride : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lRenderer;
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
        pInput.Player.ControllerAim.performed += ctxt => UpdateDirection(ctxt.ReadValue<Vector2>());
        pInput.Player.ControllerAim.canceled += ctxt => ResetDefaults();
    }

    private void UpdateDirection(Vector2 newDir)
    {
        //negative 1 lets it be accurate at the correct level
        direction = new Vector3(newDir.x, -1, newDir.y);
    }

    private void ResetDefaults()
    {
        
    }

    private void Update()
    {
        /*
         * BUG:
         * cursor disappears when click when controller plugged in.
         * 
         */
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
                //Debug.Log(cursorObject.gameObject.activeInHierarchy);
                if (!cursorObject.gameObject.activeInHierarchy)
                {
                    cursorObject.gameObject.SetActive(true);
                }

/*                lRenderer.SetPosition(0, PlayerMovement.playerTrans.position);
                lRenderer.SetPosition(1, new Vector3(pInput.Player.ControllerAim.ReadValue<Vector2>().x, 0,
                                                        pInput.Player.ControllerAim.ReadValue<Vector2>().y) *3
                                                        + PlayerMovement.playerTrans.position);*/
                //Vector3 originPos = Camera.main.WorldToViewportPoint(transform.position); //this is on the player
                //Vector3 originPos = player.localPosition;
                cursorObject.localPosition = direction * cursorDistance;
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
