using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameActionSequence))]
public class Interact : MonoBehaviour
{
    private PInput pInput;
    protected Transform player;

    [SerializeField, Tooltip("Range the player can be from the interactable to interact with it")]
    protected float interactRange;

    [SerializeField, Tooltip("Sequence exectuted when interacted with.")]
    public GameActionSequence interactSequence;

    InteractUI helperUI;

    protected virtual void Awake() { }
    protected void Start()
    {
        pInput = new();
        pInput.Enable();
        pInput.Player.Interact.performed += ctxt => Interacted();

        if(FindObjectOfType<InteractUI>() == null)
            Debug.LogError("Add InteractableUI Prefab to scene (under connor's folder in scripts)");
        helperUI = FindObjectOfType<InteractUI>();
        helperUI.addInteractable(transform);

        if (player == null)
        {
            player = FindObjectOfType<PlayerMovement>().transform;
        }
    }

    protected void OnDestroy()
    {
        pInput.Player.Interact.performed -= ctxt => Interacted();
        pInput.Disable();
    }

    protected virtual void Interacted() 
    {
        var pPos = new Vector2(player.position.x, player.position.z);
        var mePos = new Vector2(transform.position.x, transform.position.z);
        if (Vector3.Distance(pPos, mePos) > interactRange) return; //if cant interact, return
        interactSequence.Play();
        helperUI.completeInteraction(transform);
    }

    public float getInteractRange() {
        return interactRange;
    }
}
