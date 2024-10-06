using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float Health = 99;

    private bool isDead = false;
    
    public void cannonHit(float damage)
    {
        Health -= damage;
    }

    private void Update()
    {
        if (Health < 0)
            isDead = true;
    }
}