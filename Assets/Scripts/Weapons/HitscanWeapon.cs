using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanWeapon : RangedWeapon
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attack()
    {
        base.Attack();
        RaycastHit2D hit = Physics2D.Raycast(muzzle.position, muzzle.up);
        if (hit.collider != null)
        {
            Instantiate(hitEffect, hit.point, Quaternion.Euler(0,0,0));
        }
    }
}
