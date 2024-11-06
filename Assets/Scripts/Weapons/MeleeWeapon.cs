using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MeleeWeapon : Weapon
{
    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public void Update()
    {
        if (wielder != null)
        {
            weaponAnimator.SetFloat("Motion", (wielder.motionVec.magnitude / wielder.maxSpeed) * 2);
        }
    }

    public override void Attack()
    {
        BaseAttackLogic();

    }
    public override void Reload()
    {

    }
    public override void ThrowWeapon()
    {
        BaseThrowLogic();
    }
}
