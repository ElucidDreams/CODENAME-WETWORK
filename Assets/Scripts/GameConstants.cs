using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants : MonoBehaviour
{
    public enum AvailableFactions
    {
        Army,
        Robot,
        Beach
    }
    public enum AvailableSkills
    {
        LockPicking,
        Sharpshooter
    }

    public enum WeaponType
    {
        Melee,
        Projectile,
        Hitscan
    }
}
