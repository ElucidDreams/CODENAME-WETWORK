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
    public Faction faction;//TODO implement faction
    public bool isHired;
    public bool isPayroll;
    public float salary;
    public float personReputation;
    public Face personFace;
    
    public void GenerateFace()
    {
        personFace = GetComponent<Face>();
        GameObject[] g = Resources.LoadAll<GameObject>("Prefabs/Face Element Sets");
        foreach (GameObject set in g)
        {
            FaceElements f = set.GetComponent<FaceElements>();
            personFace.faces.Add(f);
        }
        try{
            personFace.emptySprite = (GameObject)Resources.Load("Prefabs/Classless Prefabs/EmptySprite");
        }
        catch(Exception e)
        {
            Debug.LogError("Empty Sprite prefab has been moved from its hard-linked position");
            Debug.LogException(e);
        }
        personFace.CreateFace(faction);
    }
}
