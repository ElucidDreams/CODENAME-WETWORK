using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

using static GameConstants;

[CreateAssetMenu(fileName ="New Mission Arms Set", menuName ="Mission Arms Set")]
/*An object that defines what arms a in mission body should use depending on the characters faction. 
The struct is basically a dictionary where the key is the faction, and value is the animation controller for the corresponding arms. 
A struct was used as a dictionary cannot be accessed in the unity editor, and it was easier to do this and manually assign the relations than creating a programmatic solution*/
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
