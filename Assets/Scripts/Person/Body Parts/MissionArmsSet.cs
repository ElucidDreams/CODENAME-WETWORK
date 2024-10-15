using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

using static GameConstants;

[CreateAssetMenu(fileName ="New Mission Arms Set", menuName ="Mission Arms Set")]
public class MissionArmsSet : ScriptableObject
{
    [System.Serializable]
    public struct FactionalArms
    {
        [SerializeField] public AvailableFactions faction;
        [SerializeField] public AnimatorController animatorController;
    }   
    
    public FactionalArms[] arms;

}
