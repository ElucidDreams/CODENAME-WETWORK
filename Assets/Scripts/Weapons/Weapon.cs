using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using static GameConstants;
[System.Serializable]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class Weapon : MonoBehaviour
{
    public WeaponType weaponID;
    [Tooltip("The readable name of the weapon")]
    public string weaponName;
    
    [NonSerialized] public SpriteRenderer worldSpriteRenderer;
    [NonSerialized] public BoxCollider2D pickupCollider;
    [NonSerialized] public Rigidbody2D worldRB;
    
    public bool isAttacking = false;
    public float checkDelay = 0.1f;
    public float weaponMass = 1f;
    public float weaponSpeed = 1f;
    public float velocityThreshold = 0.1f;
    public float hangTimeMax = 1f;
    public float groundedFriction = 10f;

    [NonSerialized] public bool inAir = false;

    public void Start()
    {
        //InitWeapon();
    }
    public void Awake()
    {
        //StartCoroutine(UpdateWeapon());
    }
    public abstract void Attack();
    public abstract void Reload();
    public abstract void ThrowWeapon();

    public IEnumerator UpdateWeapon()
    {
        throw new System.NotImplementedException();
        /*
        yield return new WaitUntil(() => weaponAnimator != null);
        weaponAnimator.SetFloat("Speed", weaponSpeed * wielder.baseAcceleration / 50);
        wielder.armsAnimator.SetFloat("Speed", weaponSpeed * wielder.baseAcceleration / 50);
        */
    }
    public virtual void InitWeapon()
    {
        throw new System.NotImplementedException();
        /*
        weaponAnimator = GetComponent<Animator>();
        weaponSpriteRenderer = GetComponent<SpriteRenderer>();
        weaponCollider = GetComponent<BoxCollider2D>();
        weaponRB = GetComponent<Rigidbody2D>();
        weaponCollider.enabled = false;
        */
    }
    public void BaseAttackLogic()
    {
        throw new System.NotImplementedException();
        /*
        //isAttacking = true;
        if (weaponAnimator.IsInTransition(0) == false && wielder.armsAnimator.IsInTransition(0) == false)
        {
            weaponAnimator.SetTrigger("Attack");
            wielder.armsAnimator.SetTrigger("Attack");
        }
        */
    }
    public void BaseThrowLogic()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator CheckForStop()
    {
        throw new System.NotImplementedException();
        /*
        while (worldRB.velocity.magnitude > velocityThreshold)
        {
            yield return new WaitForSeconds(checkDelay);
        }
        pickupCollider.enabled = false;
        */
    }
    public IEnumerator ThrowFrictionCalc()
    {
        throw new System.NotImplementedException();
        /*
        float hangTimeCounter = 0f;
        while (inAir)
        {
            yield return new WaitForSeconds(checkDelay);
            hangTimeCounter += checkDelay;
            if (hangTimeCounter > hangTimeMax)
            {
                inAir = false;
            }
        }
        yield return new WaitForSeconds(0.01f);
        worldRB.drag = groundedFriction;
        worldRB.angularDrag = groundedFriction;
        */
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        throw new System.NotImplementedException();
        /*
        if (inAir)
        {
            inAir = false;
        }
        */
    }
}
