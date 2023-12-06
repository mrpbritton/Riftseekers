using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeOut_GA : GameAction
{
    [SerializeField, Tooltip("Material the fade will be applied to")]
    private Material mat;
    [SerializeField, Tooltip("How long the fade takes")]
    private float time;
    [SerializeField, Tooltip("Reference string of the material. Ex: _Opacity")]
    private string reference;

    private void Start()
    {
        mat = gameObject.GetComponent<MeshRenderer>().material;
        DOTween.Init();
    }

    public override void Action()
    {
        mat.DOFloat(0, reference, time);
    }

    public override void DeAction()
    {
        mat.DOFloat(1, reference, time);
    }

    public override void ResetToDefault()
    {

    }
}
