using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Face))]
public class Person : MonoBehaviour
{
    [Header("Person Properties")]
    public string givenName;
    public string familyName;
    public bool isHired;
    public bool isPayroll;
    public float salary;
    [Space(5)]
    public PersonCard UICard;
    public Faction faction;//TODO implement faction
    [HideInInspector] public float personReputation;
    [HideInInspector] public Face personFace;

    public void SpawnFace()
    {
        if (UICard == null)
        {
            Debug.Log("UI Card is not set on GameObject: " + gameObject.name);
            return;
        }
        personFace = GetComponent<Face>();
        GameObject[] g = Resources.LoadAll<GameObject>("Prefabs/Face Element Sets");
        foreach (GameObject set in g)
        {
            FaceElements f = set.GetComponent<FaceElements>();
            if (faction.IdCheck(f.setFaction))
            {
                personFace.faces.Add(f);
            }
        }
        try
        {
            personFace.emptyImage = (GameObject)Resources.Load("Prefabs/Classless Prefabs/EmptyImage");
        }
        catch (Exception e)
        {
            Debug.LogError("Empty Sprite prefab has been moved from its hard-linked position");
            Debug.LogException(e);
        }
        personFace.GenerateFace(UICard.imageParent);
    }
}
