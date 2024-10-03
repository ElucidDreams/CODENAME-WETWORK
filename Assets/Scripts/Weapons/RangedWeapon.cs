using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public Transform muzzle;
    public ParticleSystem muzzleEffect;
    public GameObject hitEffect;

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
        weaponAnimator.SetTrigger("Fire");
        Instantiate(muzzleEffect,muzzle.position, Quaternion.Euler(0,0,0));
    }

    public override void Reload()
    {
    }
}
