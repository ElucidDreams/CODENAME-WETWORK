using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEditor.UIElements;
using UnityEngine;

public class Operator : Person
{
    [Header("Operator Properties")]
    public int Level;    
    public float baseHealth;
    public float baseArmour;
    public float baseSpeed;
    public float baseStrength;
    public float baseAccuracy;
    [NonSerialized] public float effectiveHealth;
    [NonSerialized] public float effectiveArmour;
    [NonSerialized] public float effectiveSpeed;
    [NonSerialized] public float effectiveStrength;
    [NonSerialized] public float effectiveAccuracy;
    [SerializeReference]
    public OperatorSkill[] skills;

    public float rating;
    // Start is called before the first frame update
    void Start()
    {
        GenerateFace();
        InitEffectiveValues();
        InitSkills();
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
