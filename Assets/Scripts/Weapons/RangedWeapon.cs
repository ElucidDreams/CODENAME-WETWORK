using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RangedWeapon : Weapon
{
    public Transform muzzle;
    public ParticleSystem muzzleEffect;
    public GameObject hitEffect;

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
        wielder.armsAnimator.SetTrigger("Fire");
        Instantiate(muzzleEffect,muzzle.position, Quaternion.Euler(0,0,0));
    }

    public override void Reload()
    {
        throw new System.NotImplementedException();
    }
}
