using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour {

    public abstract int getDamage();
    public abstract void attack();
}