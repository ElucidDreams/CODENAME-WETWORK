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
    public float maxSpeed = 10f;
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
    [Space(10)]
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
