using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnorTest : MonoBehaviour {
    //  saves the list
    void saveRoomOrder() {
        //  creates the room order filled with the indexes of the scenes randomized (I didn't randomize it here but you will in your script)
        int[] myRoomOrder = new int[10] { 2, 5, 3, 1, 3, 6, 4, 3, 1, 9 };   //  10 elements for the 10 different rooms the player will go through
        RoomSaver.saveroom(myRoomOrder);    //  saves myRoomOrder to be used later
    }
    
    //  prints every element of the saved list
    void loadNextScene() {
        int[] loadedRoomOrder = RoomSaver.loadroom();   //  gets the roomorder from the saved data (will be the same as the most recent saved list)
        int nextSceneIndex = loadedRoomOrder[0];    //  finds the next scene to load

        //  removes the next scene index from the loadedRoomOrder because we are going to load that scene next
        List<int> temp = new List<int>();
        for(int i = 1; i < loadedRoomOrder.Length; i++) {
            temp.Add(loadedRoomOrder[i]);
        }
        var reducedRoomOrder = temp.ToArray();  //  converts the list into an array

        RoomSaver.saveroom(reducedRoomOrder);   //  saves this new, reduces array to be used later

        SceneManager.LoadScene(nextSceneIndex); //  loads the next scene
    }
}
