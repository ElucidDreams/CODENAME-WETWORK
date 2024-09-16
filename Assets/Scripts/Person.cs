using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    public string givenName;
    public string familyName;
    public Faction faction;//TODO implement faction
    public bool isHired;
    public bool isPayroll;
    public float salary;
    public float personReputation;
    public Face personFace;

    public Person(string _givenName, string _familyName, Faction _faction, bool _isHired, bool _isPayroll, float _salary, float _personReputation)
    {
        givenName = _givenName;
        familyName = _familyName;
        faction = _faction;
        isHired = _isHired;
        isPayroll = _isPayroll;
        salary = _salary;
        personReputation = _personReputation;
        personFace = gameObject.AddComponent<Face>();
    }
}
