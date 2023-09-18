using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnorTest : MonoBehaviour {

    int[] thing = { 3, 2, 5, 14, 567, 34 }; //  list to be saved

    private void Awake() {  //  doesn't need to be in awake
        //save  //  call this before you try to load
        load();
    }

    //  saves the list
    void save() {
        RoomSaver.saveroom(thing);
    }
    
    //  prints every element of the saved list
    void load() {
        foreach(var i in RoomSaver.loadroom())
            Debug.Log(i.ToString());
    }
}
