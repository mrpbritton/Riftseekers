using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTransition : MonoBehaviour
{
    public LevelManager sceneGuy;
    int[] scenes = new int[4];
    // Start is called before the first frame update
    void Start()
    {
        sceneGuy = FindObjectOfType<LevelManager>();
        scenes = RoomSaver.loadroom();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            RoomTransitionz();
        }*/
    }
    void RoomTransitionz()
    {
        /*  int index = SceneManager.GetActiveScene().buildIndex;
          if(index == 0)
          {
              SceneManager.LoadScene(scenes[0]);
          }
          else
          {
              for(int i = 0; i < 3; i++)
              {
                  if(index == scenes[i])
                  {
                      SceneManager.LoadScene(scenes[i + 1]);
                  }
              }   
          }
        */
        SceneManager.LoadScene(5);
    }

    private void OnEnable()
    {
        EnemyController.levelComplete += levelComplete;
    }

    private void OnDisable()
    {
        EnemyController.levelComplete -= levelComplete;
    }

    private void levelComplete()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            sceneGuy.loadNextScene();
            //RoomTransitionz();
        }
    }
}
