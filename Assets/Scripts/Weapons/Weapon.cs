using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static GameConstants;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class Weapon : MonoBehaviour
{
    [NonSerialized] public Animator weaponAnimator;
    [NonSerialized] public SpriteRenderer weaponSpriteRenderer;
    [NonSerialized] public BoxCollider2D weaponCollider;
    [NonSerialized] public Rigidbody2D weaponRB;
    [Header("General Weapon Properties")]
    public WeaponType weaponType;
    public Sprite heldSprite;
    public Sprite worldSprite;
    public float checkDelay = 0.1f;
    [Space(10)]
    [Header("Throwing Properties")]
    public float weaponMass = 1f;
    public float velocityThreshold = 0.1f;
    public float hangTimeMax = 1f;
    public float groundedFriction = 10f;
    [NonSerialized] public bool inAir = false;

    public abstract void Attack();
    public abstract void Reload();
    public virtual void InitWeapon()
    {
        weaponAnimator = GetComponent<Animator>();
        weaponSpriteRenderer = GetComponent<SpriteRenderer>();
        weaponCollider = GetComponent<BoxCollider2D>();
        weaponRB = GetComponent<Rigidbody2D>();
        weaponCollider.enabled = false;
    }

    public IEnumerator CheckForStop()
    {
        while (weaponRB.velocity.magnitude > velocityThreshold)
        {
            yield return new WaitForSeconds(checkDelay);
        }
        weaponCollider.enabled = false;
    }
    public IEnumerator ThrowFrictionCalc()
    {
        float hangTimeCounter = 0f;
        while(inAir)
        {
            yield return new WaitForSeconds(checkDelay);
            hangTimeCounter += checkDelay;
            if (hangTimeCounter > hangTimeMax)
            {
                inAir = false;
            }
        }
        yield return new WaitForSeconds(0.01f);
        weaponRB.drag = groundedFriction;
        weaponRB.angularDrag = groundedFriction;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (inAir)
        {
            inAir = false;
        }
    }
}
