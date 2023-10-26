using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerManager : MonoBehaviour {
    private void Awake() {
        //  finds main obj
        var tallestParent = gameObject;
        while(tallestParent.transform.parent != null)
            tallestParent = tallestParent.transform.parent.gameObject;

        //  checks if needs a current player manager
        if(Inventory.getPlayerManager() == null) {
            DontDestroyOnLoad(tallestParent);
            Inventory.setPlayerManager(this);   //  sets as curPlayerManager
            GetComponentInParent<SpawnCam>().manageCamera();
        } 
        //  checks if not the current player manager
        else if(Inventory.getPlayerManager().gameObject.GetInstanceID() != gameObject.GetInstanceID()) {
            Inventory.overridePlayerManagersTransform(this);
            Destroy(tallestParent.gameObject);  //  sets curPlayerManager's transform to equal this ones
        }

        //GameObject.FindWithTag("VirtCam").GetComponent<CinemachineVirtualCamera>().Follow = FindFirstObjectByType<CharacterFrame>().gameObject.transform;
        //FindFirstObjectByType<CharacterFrame>().gameObject.transform
    }
}
