using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
    public Weapon parentWeapon;
    public bool canAttack = true;
    public float damageCooldown = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (canAttack)
            {
                StartCoroutine(DamageCooldown());
                other.gameObject.GetComponent<Operator>().TakeDamage(parentWeapon, parentWeapon.weaponDamage * parentWeapon.wielder.effectiveStrength);
            }
        }
    }

    public IEnumerator DamageCooldown()
    {
        canAttack = false;
        yield return new WaitUntil(() => !parentWeapon.wielder.armsAnimator.GetCurrentAnimatorStateInfo(0).IsName("AttackL") && !parentWeapon.wielder.armsAnimator.GetCurrentAnimatorStateInfo(0).IsName("AttackR"));
        canAttack = true;
    }
}
