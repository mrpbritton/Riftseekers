using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLibrary : MonoBehaviour {
    public enum abilType {
        None, Rocket, Dash, Sword, Pistol
    }

    [SerializeField] List<Attack> abs = new List<Attack>();
    [SerializeField] Dictionary<abilType, Attack> abils = new Dictionary<abilType, Attack>();


    private void Awake() {
        foreach(var i in abs)
            abils.Add(i.abilType, i);
    }

    public Attack getAttack(abilType type) {
        if(type == abilType.None)
            return null;
        return abils[type];
    }
}
