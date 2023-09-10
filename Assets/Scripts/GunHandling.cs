using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandling : MonoBehaviour
{
    public float damage = 10f;
    public float shootingRange = 60f;
    public Camera camera;
    public ParticleSystem muzzleSpark;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            shoot();
        }
    }

    void shoot()
    {
        RaycastHit hitInfo;
        muzzleSpark.Play();
        if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hitInfo))
        {
            Debug.Log(hitInfo.transform.name);
            ObjectHandling obj = hitInfo.transform.GetComponent<ObjectHandling>();

            EnemyScript enemy = hitInfo.transform.GetComponent<EnemyScript>();

            if(obj != null)
            {
                obj.damageObject(damage);
            }
            else if(enemy != null)
            {
                enemy.enemyHealthDamage(damage);
            }
        }
    }
}
