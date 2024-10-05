using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEditor.UIElements;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(FixedJoint2D))]
public class Operator : Person
{
    [Header("Operator Properties")]
    public int Level; 
    public bool inMission = true;
    public float maxSpeed = 10f;
    [NonSerialized] public Animator animComp;
    [NonSerialized] public Transform transformComp;

    [NonSerialized] public Rigidbody2D rbComp;
    [NonSerialized] public FixedJoint2D activeWeaponJoint;
    [Space(10)]
    public float baseHealth;
    public float baseArmour;
    public float baseSpeed;
    public float baseStrength;
    public float baseAccuracy;
    [Space(10)]
    public float effectiveHealth;
    public float effectiveArmour;
    public float effectiveSpeed;
    public float effectiveStrength;
    public float effectiveAccuracy;
    [Space(10)]
    [SerializeReference]
    public OperatorSkill[] skills;
    public Weapon activeWeapon;
    
    [Space(10)]
    public float rating;

    // Start is called before the first frame update
    void Start()
    {
        SetMissionUI();
        InitEffectiveValues();
        InitSkills();
        InitComponents();
    }

    void Update()
    {
        AnimateBody();
    }

    public void AnimateBody()
    {
        animComp.SetFloat("movementSpeed",rbComp.velocity.magnitude);
    }

    public void InitEffectiveValues()
    {
        //Initialize effective values
        effectiveHealth = baseHealth;
        effectiveArmour = baseArmour;
        effectiveSpeed = baseSpeed;
        effectiveStrength = baseStrength;
        effectiveAccuracy = baseAccuracy;
    }
    public void InitSkills()
    {
        //Initialize all skills
        foreach(OperatorSkill skill in skills)
        {
            skill.SkillUser = this;
            if (!skill.OnDemand)
            {
                skill.RunSkill();
            }
        }
    }
    public void InitComponents()
    {
        animComp = GetComponentInChildren<Animator>();
        transformComp = GetComponent<Transform>();
        rbComp = GetComponent<Rigidbody2D>();
        activeWeaponJoint = GetComponent<FixedJoint2D>();
    }
    public void RotateToPoint(Vector2 point)
    {
        Vector2 difference = (Vector3)point - transform.position;
        float angle = Mathf.Atan2(difference.y,difference.x) * Mathf.Rad2Deg;
        transformComp.rotation = Quaternion.Euler(0,0, angle);
    }
    public void WeaponThrow()
    {
        activeWeapon.transform.SetParent(null);
        activeWeapon.weaponCollider.enabled = true;
        activeWeaponJoint.connectedBody = rbComp;
        float wielderFacing = transform.eulerAngles.z;
        Vector2 throwDirection = new(Mathf.Cos(wielderFacing * Mathf.Deg2Rad), Mathf.Sin(wielderFacing * Mathf.Deg2Rad));
        activeWeapon.weaponRB.mass = activeWeapon.weaponMass;
        activeWeapon.weaponRB.AddForce(throwDirection * effectiveStrength, ForceMode2D.Impulse);
        activeWeapon.weaponRB.AddTorque(UnityEngine.Random.Range(0f,1f));
        activeWeapon.inAir = true;
        StartCoroutine(activeWeapon.CheckForStop());
        StartCoroutine(activeWeapon.ThrowFrictionCalc());
        activeWeapon = null;
    }
    public void SetMissionUI()
    {
        SpawnFace();
        UICard.nameText.text = givenName + " " + familyName;
        UICard.factionText.text = faction.factionID.ToString();
    }
}
