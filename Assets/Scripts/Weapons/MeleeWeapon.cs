using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MeleeWeapon : Weapon
{
    // Start is called before the first frame update
    public new void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {

    }

    public override void Attack()
    {
        wielder.armsAnimator.SetTrigger("Attack");
    }
    public override void Reload()
    {

    }
}
