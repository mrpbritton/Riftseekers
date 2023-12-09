using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHandler : MonoBehaviour
{
    public List<AnimEvent> animEvents;
    public void TriggerEvent(int index)
    {
        animEvents[index].uEvent.Invoke();
    }
}
[Serializable]
public struct AnimEvent
{
    public string name;
    public UnityEvent uEvent;
}
