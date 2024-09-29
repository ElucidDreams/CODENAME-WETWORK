using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public Transform muzzle;
    public ParticleSystem muzzleEffect;

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
        RaycastHit2D hit = Physics2D.Raycast(muzzle.position, Vector3.up);
        Instantiate(muzzleEffect, hit.point, Quaternion.Euler(0,0,0));
        Debug.DrawLine(muzzle.position, (Vector3)hit.point, Color.red, 10f,false);
        Debug.Log((Vector3)hit.point);
    }
    public override void Reload()
    {
    }
}
