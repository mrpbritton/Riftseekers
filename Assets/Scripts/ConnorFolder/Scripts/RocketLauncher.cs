using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RocketLauncher : Attack {

    [SerializeField] GameObject rocketPreset;
    [SerializeField] float maxTravelDist, maxTravelTime;
    [SerializeField] Transform rotTrans;

    public override void attack() {
        var curRocket = Instantiate(rocketPreset.gameObject);
        curRocket.GetComponent<RocketInstance>().setParentAttack(this, maxTravelTime);
        curRocket.transform.position = transform.position;
        curRocket.transform.DOMove(transform.position + rotTrans.right * maxTravelDist, maxTravelTime);
        Destroy(curRocket.gameObject, maxTravelTime);
    }

    public override void reset()
    {
    }
    public override attackType getAttackType() {
        return attackType.QAbility;
    }
    protected override float getDamage() {
        return 30f;
    }
    protected override float getCooldownTime() {
        return 3f;
    }
}
