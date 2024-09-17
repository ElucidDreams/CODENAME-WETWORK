using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

public class Sharpshooter : OperatorSkill
{   
    [SerializeField] float staticBump = 0.1f;
    [SerializeField] float levelBump = 0.01f;
    
    public override void RunSkill()
    {
        Debug.Log("Pre-Skill effective accuracy: " + SkillUser.effectiveAccuracy);
        SkillUser.effectiveAccuracy = SkillUser.baseAccuracy + staticBump + (levelBump * Level);
        Debug.Log("Skill Ran\nPost-Skill effective accuracy: " + SkillUser.effectiveAccuracy);
    }

    public override bool RunSkill(float difficulty)
    {
        Debug.LogError("This function does nothing as it is a skill that requires no boolean return, please use void RunSkill() instead.");
        return true;
    }
    
}
