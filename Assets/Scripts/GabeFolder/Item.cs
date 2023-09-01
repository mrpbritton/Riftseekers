using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interact
{
    [SerializeField, Tooltip("What this item does")]
    private string description;
    [SerializeField, Tooltip("How much this item sells for")]
    private int value;

    protected override void Interacted()
    {
        if (!canInteract) return;
        Debug.Log("item picked up!");
        interactSequence.Play();
    }
}
