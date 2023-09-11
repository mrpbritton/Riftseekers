using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class EnemyFiring : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private float reloadTime = 2f;
    private Quaternion rotation = Quaternion.identity;
//    private bool reload;
    private void Start()
    {
        StartCoroutine(nameof(Reloading));
//        reload = true;
    }

    private void FireShot()
    {
        rotation = gameObject.transform.rotation;
        Projectile project = Instantiate(projectile, transform.position, rotation).GetComponent<Projectile>();
        project.Direction = transform.forward;
        StartCoroutine(nameof(Reloading));
    }
    /*
        void Update()
        {

            if (reload == false)
            {
                FireShot();
                reload = true;
                StartCoroutine(nameof(Reloading));
            }
        }
    */
    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloadTime);
//        reload = false;
        FireShot();
    }

}
