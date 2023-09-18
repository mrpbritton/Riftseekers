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

        var mPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0f, Input.mousePosition.y));
        var dir = mPos - transform.position;
        dir.y = 0f;
        Debug.Log(rotTrans.forward);
        curRocket.transform.DOMove(transform.position + rotTrans.right * maxTravelDist, maxTravelTime);
        Destroy(curRocket.gameObject, maxTravelTime);
    }
    public override attackType getAttackType() {
        return attackType.QAbility;
    }
    protected override int getDamage() {
        return 30;
    }
    protected override float getCooldownTime() {
        return 3f;
    }
}
