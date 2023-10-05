using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItem_GA : GameAction
{
    [SerializeField, Tooltip("The item that will be added to the inventory")]
    private ConItem item;

    /// <summary>
    /// Adds an item defined in the inspector to the inventory.
    /// </summary>

    private void Start()
    {
        Inventory.loadInventory();
    }

    public override void Action()
    {
        Inventory.addItem(item);
        Inventory.saveInventory();
        //Debug.Log($"{Inventory.getItem(Inventory.getItemIndex(item), FindObjectOfType<ItemLibrary>()).title}");
    }

    /// <summary>
    /// Adds an item i into the inventory.
    /// </summary>
    public void Action(ConItem i)
    {
        Inventory.addItem(i);
        Inventory.saveInventory();
    }

    public override void DeAction()
    {
        Inventory.removeItem( Inventory.getItemIndex(item) );
        Inventory.saveInventory();
    }

    public override void ResetToDefault()
    {
        throw new System.NotImplementedException();
    }
}
