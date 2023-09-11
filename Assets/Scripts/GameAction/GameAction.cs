using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameAction : MonoBehaviour
{
    [SerializeField]
    public float delay;
    public abstract void Action();
    public abstract void DeAction();
    public abstract void ResetToDefault();
}
