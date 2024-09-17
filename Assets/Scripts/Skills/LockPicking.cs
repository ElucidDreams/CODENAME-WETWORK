using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

public class LockPicking : OperatorSkill
{
    public override void RunSkill()
    {
        Debug.LogError("This function does nothing as it is a skill that requires a boolean return, please use bool RunSkill(float difficulty) instead.");
    }

    public override bool RunSkill(float difficulty)
    {
        Debug.Log("Skill Ran"); //TODO Implement actual skill function;
        return true;
    }

}
