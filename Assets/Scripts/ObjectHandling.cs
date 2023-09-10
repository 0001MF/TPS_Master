using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandling : MonoBehaviour
{
    public float objectHealth = 100f;
    
    public void damageObject(float amount)
    {
        objectHealth -= amount;
        if(objectHealth <= 0f)
        {
            Die();
        } 
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
