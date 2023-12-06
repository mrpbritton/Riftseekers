using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBoots : MonoBehaviour
{
    [SerializeField, Tooltip("How much speed is added to the player")]
    private float speedBuff = 10;
    public void Equip(CharacterFrame frame)
    {
        frame.movementSpeed += speedBuff;
    }

    public void UnEquip(CharacterFrame frame)
    {
        frame.movementSpeed -= speedBuff;
    }
}
