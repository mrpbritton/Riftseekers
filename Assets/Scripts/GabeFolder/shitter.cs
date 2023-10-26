using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class shitter : MonoBehaviour
{
    [SerializeField, Tooltip("Scene to move to")]
    Scene nextScene;
    [SerializeField, Tooltip("Player")]
    GameObject player;

    private void OnEnable()
    {
        SceneManager.MoveGameObjectToScene(player, nextScene);

    }
}
