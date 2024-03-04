using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTransition : MonoBehaviour
{
    public LevelManager sceneGuy;
    public bonuspicker manager;
    int[] scenes = new int[4];
    bool checker = true;
    public int Scene;

    // Start is called before the first frame update
    void Start()
    {
        sceneGuy = FindObjectOfType<LevelManager>();
        manager = FindObjectOfType<bonuspicker>();
        scenes = RoomSaver.loadroom();
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
        FindObjectOfType<PlayerStats>().save();
    }

    private void Update()
    {
        if (manager.picked == true)
        {
            sceneGuy.loadSpecific(Scene);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Gaming?");
        if (other.tag == "Player" && checker == true)
        {
            manager.setup();
            checker = false;
            //RoomTransitionz();
        }
    }
}
