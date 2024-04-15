using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RocketLauncher : Attack {

    [SerializeField] GameObject rocketPreset;
    [SerializeField] float maxTravelDist, maxTravelTime, explosionSize, explosionDmg, explosionKnockback;
    float height = 2f;
    public override AttackType AType => AttackType.Special;
    public override AttackScript AScript => AttackScript.Rocket;

    private Vector3 cachedDir = new(1, 1, 1);
    private Transform origin;

    protected override void Start() {
        base.Start();
        origin = GameObject.FindWithTag("GunOrigin").transform;
        cachedDir = origin.forward;
    }

    public override void DoAttack() {
        var curRocket = Instantiate(rocketPreset.gameObject);
        var rocketEndExplosion = ExplosionManager.I.queueExplode(curRocket.transform, explosionSize, explosionDmg, explosionKnockback, ExplosionManager.explosionState.HurtsEnemies, maxTravelTime);
        curRocket.GetComponentInChildren<RocketInstance>().setup(rocketEndExplosion, ExplosionManager.I, explosionSize, explosionDmg, explosionKnockback);
        curRocket.transform.position = transform.position + Vector3.up * height;
        curRocket.transform.localEulerAngles = new Vector3(0f, curRocket.transform.eulerAngles.y, curRocket.transform.eulerAngles.z);
        Destroy(curRocket.gameObject, maxTravelTime + .1f);


        Vector3 dir;
        Vector3 direction;
        if(!InputManager.isUsingKeyboard()) {
            dir = new Vector3(pInput.Player.ControllerAim.ReadValue<Vector2>().x, 0, pInput.Player.ControllerAim.ReadValue<Vector2>().y);

            if(dir == Vector3.zero) {
                dir = cachedDir;
            }
            else {
                cachedDir = dir;
            }

            direction = dir;
        }
        else {
            dir = GetPoint();
            direction = new Vector3(dir.x - origin.position.x + origin.localPosition.x,
                                            dir.y,
                                            dir.z - origin.position.z + origin.localPosition.z);
        }
        FindObjectOfType<PlayerMovement>().slide(direction, -12f, .25f);
        curRocket.transform.DOMove(transform.position + (direction.normalized * maxTravelDist), maxTravelTime);
        curRocket.transform.LookAt(curRocket.transform.position + new Vector3(direction.x, 0f, direction.z).normalized);
    }

    public override void Anim(Animator anim, bool reset)
    {
    }
    public override void ResetAttack()
    {
    }
    protected override float SetDamage => 30f;
    protected override float SetCooldownTime => 3f;
}
