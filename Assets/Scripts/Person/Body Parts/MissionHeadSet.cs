using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Mission Head Set", menuName ="Mission Head Set")]
/*The mission head set creates a set of possible head sprites for the operator bodies for the particular faction, and defines a set of colors to color the heads.
This is used to take the faction of the operator, as well as the face of the operator and find the matching sprite and color to the distinctive component to make the in game head match the UI face*/
public class MissionHeadSet : ScriptableObject
{
    public GameConstants.AvailableFactions setFaction;
    [SerializeField] public Sprite[] heads;
    [SerializeField] public Color[] colors;
}
