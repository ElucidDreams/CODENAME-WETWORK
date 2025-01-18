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
    public void Update()
    {

    }
    public override void Attack()
    {
        wielder.armsAnimator.SetTrigger("Attack");
        isAttacking = true;
        StartCoroutine(IsAttacking());
    }
    public override void Reload()
    {

    }
    public IEnumerator IsAttacking()
    {
        yield return new WaitUntil(() => !wielder.armsAnimator.GetCurrentAnimatorStateInfo(0).IsName("AttackL") && !wielder.armsAnimator.GetCurrentAnimatorStateInfo(0).IsName("AttackR"));
        isAttacking = false;
    }
}
