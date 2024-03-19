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
            Debug.LogError("Add InteractableUI Prefab to scene");
        helperUI = FindObjectOfType<InteractUI>();

        //  gets info
        if(TryGetComponent<AddItem_GA>(out var it))
            helperUI.addInteractable(transform, new InteractInfo(it.item.title, InteractInfo.interactType.Item));
        else if(TryGetComponent<AddAugment_GA>(out var aa))
            helperUI.addInteractable(transform, new InteractInfo(aa.refType.ToString(), InteractInfo.interactType.Augment));
        else if(TryGetComponent<HealthPack_GA>(out var hp))
            helperUI.addInteractable(transform, new InteractInfo("Health Pack", InteractInfo.interactType.Health));

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

    public bool inRange() {
        if(player == null) return false;
        var d = Vector3.Distance(transform.position, player.position);
        return d < getInteractRange();
    }
}
