using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

[System.Serializable]
public abstract class OperatorSkill : MonoBehaviour
{
    public int level = 1;
    public bool onDemand = false;
    public AvailableSkills skillType;
    [NonSerialized] public Operator skillUser;
    public AvailableSkills SkillType
    { 
        get { return skillType; } 
    }
    public abstract void RunSkill();
    public abstract bool RunSkill(float difficulty);
}

