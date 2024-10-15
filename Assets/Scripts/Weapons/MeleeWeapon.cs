using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        InitWeapon();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attack()
    {
        isAttacking = true;
    }
    public override void Reload()
    {

    }
}
