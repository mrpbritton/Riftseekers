using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Bullet : MonoBehaviour
{
    [SerializeField, Tooltip("Speed of the projectile")]
    private float speed;
    [HideInInspector, Tooltip("Direction the projectile is headed")]
    public Vector3 direction;
    [Tooltip("Time in seconds it takes for bullet to die")]
    public float lifetime;
    Transform playerTrans;
    [Tooltip("Does the bullet pierce?")]
    public bool bCanPierce = false;
    [Tooltip("How many times does the bullet pierce?")]
    public int pierceCount = 0;
    private int currentPierceCount;
    private void OnEnable()
    {
        playerTrans = FindObjectOfType<PlayerManager>().transform;
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        transform.position += Time.deltaTime * speed * direction.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        bool bTagIsEnemy = false;
        //dont collide with the player, other bullets, or the hitboxes on VFX
        if(!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Bullet") && !other.gameObject.CompareTag("FxTemporaire"))
        {
            if(other.gameObject.CompareTag("Enemy"))
            {
                bTagIsEnemy = true;
            }

            if (!other.gameObject.CompareTag("Enemy"))
            {
                AkSoundEngine.PostEvent("Object_Hit", gameObject);
            }
            var hitOffset = (transform.position - playerTrans.position).normalized * 1f;
            FindObjectOfType<ExplosionManager>().blueExplode(transform.position - hitOffset, .25f, 0f, 0f, ExplosionManager.explosionState.None);

            if (bCanPierce && bTagIsEnemy)
            {
                currentPierceCount++;
            }

            if (!bCanPierce || currentPierceCount >= pierceCount || !bTagIsEnemy)
            {
                //Debug.Log($"{!bCanPierce}, {currentPierceCount >= pierceCount}, {!bTagIsEnemy}");
                BulletManager.I.repoolBullet(gameObject);
            }
        }
    }
}
