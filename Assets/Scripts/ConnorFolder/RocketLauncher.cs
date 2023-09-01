using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RocketLauncher : Attack {

    [SerializeField] GameObject rocketPreset;
    [SerializeField] float maxTravelDist, maxTravelTime;

    public override void attack() {
        var curRocket = Instantiate(rocketPreset.gameObject);
        curRocket.GetComponent<RocketInstance>().setParentAttack(this, maxTravelTime);
        curRocket.transform.position = transform.position;

        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dir.Normalize();
        curRocket.transform.DOMove(dir * maxTravelDist, maxTravelTime);
        Destroy(curRocket.gameObject, maxTravelTime);
    }
    public override attackType getAttackType() {
        return attackType.QAbility;
    }
    public override int getDamage() {
        return 30;
    }
    public override float cooldownTime() {
        return 3f;
    }
}
