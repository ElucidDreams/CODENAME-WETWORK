using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using static GameConstants;
using AYellowpaper.SerializedCollections;
[System.Serializable]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class Weapon : MonoBehaviour
{
    public WeaponType weaponID;
    [Tooltip("The readable name of the weapon")]
    public string weaponName = "Unnamed Weapon";
    public AYellowpaper.SerializedCollections.SerializedDictionary<Faction, RuntimeAnimatorController> armsAnimators;
    [NonSerialized] public SpriteRenderer worldSpriteRenderer;
    public BoxCollider2D throwCollider;
    [HideInInspector] public Rigidbody2D worldRB;
    public Operator wielder;
    
    public bool isAttacking = false;
    public float checkDelay = 0.1f;
    public float weaponMass = 1f;
    public float weaponSpeed = 1f;
    public float velocityThreshold = 0.1f;
    public float hangTimeMax = 1f;
    public float groundedFriction = 10f;

    [HideInInspector] public bool inAir = false;

    public void Start()
    {
        worldSpriteRenderer = GetComponent<SpriteRenderer>();
        throwCollider = GetComponent<BoxCollider2D>();
    }

    public abstract void Attack();
    public abstract void Reload();
    public void ThrowWeapon()
    {
        transform.SetParent(null);//un-parents the weapon
        
        throwCollider.enabled = true;//Enable the weapons collider
        worldSpriteRenderer.enabled = true;
        worldSpriteRenderer.sortingOrder = 1;

        float wielderFacing = wielder.armTransform.eulerAngles.z;//get the facing of the arms
        Vector2 throwDirection = new(Mathf.Cos(wielderFacing * Mathf.Deg2Rad), Mathf.Sin(wielderFacing * Mathf.Deg2Rad));//generate a vector from the facing

        worldRB = gameObject.AddComponent<Rigidbody2D>();//add a rigid body here to ensure that the weapon can have physics but not interfere with the player (Joints where not plausible with the current implementation)
        worldRB.gravityScale = 0;//Deactivate gravity on the weapon
        worldRB.mass = weaponMass;//set the rigidbody mass to be the preset mass of the weapon;
        worldRB.AddForce(throwDirection * wielder.effectiveStrength, ForceMode2D.Impulse);//add the force to the object, taking the operators strength into effect
        worldRB.AddTorque(UnityEngine.Random.Range(0f,1f));//add a slight spin to the item
        inAir = true;//set the object to be in the air
        wielder = null;
        StartCoroutine(CheckForStop());//start the timer to disable the collider again
        StartCoroutine(ThrowFrictionCalc());//start the friction function
    }

    public void Initialize()
    {
        worldSpriteRenderer = GetComponent<SpriteRenderer>();
        throwCollider = GetComponent<BoxCollider2D>();
    }
    public IEnumerator CheckForStop()
    {
        yield return new WaitUntil(() => worldRB.velocity.magnitude > velocityThreshold);
        throwCollider.enabled = false;
    }
    public IEnumerator ThrowFrictionCalc()
    {
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
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (inAir)
        {
            inAir = false;
        }
    }
}
