using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

public abstract class OperatorSkill : MonoBehaviour
{
    [SerializeField] int level = 1;
    [SerializeField] bool onDemand = false;
    [SerializeField] AvailableSkills skillType;
    Operator skillUser;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }
    public bool OnDemand
    {
        get { return onDemand; }
    }
    public Operator SkillUser
    {
        get { return skillUser; }
        set { skillUser = value; }
    }
    public AvailableSkills SkillType
    { 
        get { return skillType; } 
    }
    public abstract void RunSkill();
    public abstract bool RunSkill(float difficulty);
}

