using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Weapon : MonoBehaviour
{
    [NonSerialized] public Animator weaponAnimator;
    [NonSerialized] public SpriteRenderer weaponSprite;
    [NonSerialized] public BoxCollider2D weaponCollider;
    [NonSerialized] public Rigidbody2D weaponRB;
    [Header("General Weapon Properties")]
    public WeaponType weaponType;
    public float checkDelay = 0.1f;
    public float velocityThreshold = 0.1f;

    public abstract void Attack();
    public abstract void Reload();

    public virtual void InitWeapon()
    {
        weaponAnimator = GetComponent<Animator>();
        weaponSprite = GetComponent<SpriteRenderer>();
        weaponCollider = GetComponent<BoxCollider2D>();
        weaponRB = GetComponent<Rigidbody2D>();
    }

    public IEnumerator CheckForStop()
    {
        while (weaponRB.velocity.magnitude > velocityThreshold)
        {
            yield return new WaitForSeconds(checkDelay);
        }
        weaponCollider.enabled = false;
    }
}
