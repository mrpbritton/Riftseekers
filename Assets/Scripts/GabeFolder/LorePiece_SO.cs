using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class LorePiece_SO : ScriptableObject
{
    //although we could just use the name of the scriptable obj, we'd have to deal with spaces and such
    [Tooltip("Title of the entry")] 
    public string title;
    [TextArea(3, 10)]
    [Tooltip("The written lore")]
    public string lore;

    public Sprite image;

    public int value;
}
