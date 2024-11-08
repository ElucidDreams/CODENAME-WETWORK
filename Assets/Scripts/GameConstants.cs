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
        Beach = 3
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
        Unarmed = 1,
        BaseballBat = 2,
        Pistol = 3,
    }
}
