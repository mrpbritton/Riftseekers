using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGO_GA : GameAction
{
    [SerializeField, Tooltip("GameObject that will be enabled")]
    private GameObject go;
    [SerializeField, Tooltip("Default enable state of the GameObject")]
    private bool bEnableState;
    public override void Action()
    {
        if(go != null)
            go.SetActive(true);
    }

    public override void DeAction()
    {
        if (go != null)
            go.SetActive(false);
    }
    public override void ResetToDefault()
    {

        go.SetActive(bEnableState);
    }
}
