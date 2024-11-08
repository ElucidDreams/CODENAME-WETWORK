using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameConstants : MonoBehaviour
{
    [Serializable]
    public enum Faction
    {
        Army = 1,
        Robot = 2,
        Beach = 3,
        Wizard = 4
    }
    [Serializable]
    public enum AvailableSkills
    {
        LockPicking = 1,
        Sharpshooter = 2
    }
    [Serializable]
    public enum WeaponType
    {
        Melee = 1,
        Projectile = 2,
        Hitscan = 3
    }
}
