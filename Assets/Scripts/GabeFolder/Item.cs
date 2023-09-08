using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Item : Interact
{
    [Tooltip("Name of the item")]
    public string name;
    [Tooltip("What this item does")]
    public string description;
    [SerializeField, Tooltip("How much this item sells for")]
    public int value;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Interacted()
    {
        if (!canInteract) return;
        Debug.Log("item picked up!");
        frame.movementSpeed += 0.5f;
        CharacterFrame.Restat();
        interactSequence.Play();
    }
}
