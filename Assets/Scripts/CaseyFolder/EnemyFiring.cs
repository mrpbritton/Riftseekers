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
    private void Start()
    {
        StartCoroutine(nameof(Reloading));
    }

    private void FireShot()
    {
        rotation = gameObject.transform.rotation;
        Projectile project = Instantiate(projectile, transform.position, rotation).GetComponent<Projectile>();
        project.Direction = transform.forward;
        StartCoroutine(nameof(Reloading));
    }
    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloadTime);
        FireShot();
    }

}
