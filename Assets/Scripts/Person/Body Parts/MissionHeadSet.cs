using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Mission Head Set", menuName ="Mission Head Set")]
public class MissionHeadSet : ScriptableObject
{
    public GameConstants.AvailableFactions setFaction;
    [SerializeField] public Sprite[] heads;
    [SerializeField] public Color[] colors;
}
