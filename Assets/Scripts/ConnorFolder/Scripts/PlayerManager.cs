using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {
    public GameObject getTallestParent() {
        var tallestParent = gameObject;
        while(tallestParent.transform.parent != null)
            tallestParent = tallestParent.transform.parent.gameObject;
        return tallestParent;
    }

    private void Awake() {
        if(SceneManager.GetActiveScene().buildIndex == 4) 
            SaveData.wipe();
    }
}
