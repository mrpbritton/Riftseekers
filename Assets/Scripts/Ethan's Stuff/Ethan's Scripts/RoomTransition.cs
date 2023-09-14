using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTransition : MonoBehaviour
{

    int[] scenes = new int[4];
    // Start is called before the first frame update
    void Start()
    {
        scenes = RoomSaver.loadroom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void RoomTransitionz()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if(index == 0)
        {
            SceneManager.LoadScene(scenes[0]);
        }
        else
        {
            for(int i = 0; i < 10; i++)
            {
                if(index == scenes[i])
                {
                    SceneManager.LoadScene(scenes[i + 1]);
                }
            }
        }
    }
}
