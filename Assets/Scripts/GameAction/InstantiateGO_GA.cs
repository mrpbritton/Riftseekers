using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateGO_GA : GameAction
{
    [SerializeField, Tooltip("GameObject that will be instantiated")]
    private GameObject go;
    [SerializeField, Tooltip("Position GameObject will be instantiated at")]
    private Vector3 position;
    private GameObject tempGo; //this variable will hold the instance of the GO created

    public override void Action()
    {
        tempGo = Instantiate(go, position, go.transform.rotation, gameObject.transform);
    }

    /// <summary>
    /// If possible, use the DestroyGO game action. This will still work, but please try to use that.
    /// </summary>
    public override void DeAction()
    {
        if(tempGo != null)
        {
            Destroy(tempGo);
        }
        else
        {
            throw new System.Exception("No GameObject to be destroyed!");
        }
    }
    public override void ResetToDefault()
    {
        throw new System.NotImplementedException();
    }
}
