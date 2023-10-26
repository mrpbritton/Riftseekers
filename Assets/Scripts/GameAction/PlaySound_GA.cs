using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound_GA : GameAction
{
    [SerializeField, Tooltip("Name of event to be played")]
    private string eventName;
    [SerializeField, Tooltip("Name of alt event to be played")]
    private string eventName2;

    bool firstInteraction = true;
    public override void Action()
    {
        if (firstInteraction)
        {
            AkSoundEngine.PostEvent(eventName, gameObject);
            firstInteraction = false;
        }
        else
        {
            AkSoundEngine.PostEvent(eventName2, gameObject);
        }
    }
    public override void DeAction()
    {
        //nothing
    }
    public override void ResetToDefault()
    {
        //nothing
    }
}
