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
    public bool isHirable;//Is the person available to be hired (i.e. an operator for hire vs quest giver)
    public bool isHired;//If the person is hirable, then are they actually hired
    public bool isPayroll;//If the person is hired, are they taking a cut of the operation, or are they hired as an employee
    public float salary;//If the person is an employee, then what is their salary
    [Space(5)]
    public PersonCard UICard;//The UI card where their face and info should be rendered 
    public Faction faction;//The faction the person is affiliated with
    [HideInInspector] public float personReputation;//The reputation the player has with this person
    [HideInInspector] public Face personFace;//The face object of the player. 

    public void SpawnFace()
    {
        if (UICard == null)//Check if the player has a UI card, if they don't, don't progress and throw a warning
        {
            Debug.LogWarning("UI Card is not set on GameObject: " + gameObject.name);
            return;
        }
        personFace = GetComponent<Face>();
        GameObject[] g = Resources.LoadAll<GameObject>("Prefabs/Face Element Sets");//Load all of the face elements in the face element sets folder in resources
        foreach (GameObject set in g)
        {
            FaceElements f = set.GetComponent<FaceElements>();//Get the faceElements component of each prefab and check if the faction of the set matches the person, if so add the set to the list of potential face sets
            if (faction.IdCheck(f.setFaction))
            {
                personFace.faces.Add(f);
            }
        }
        try
        {
            personFace.emptyImage = (GameObject)Resources.Load("Prefabs/Classless Prefabs/EmptyImage");//load the empty image prefab to create an empty sprite renderer and transform for the face generation
        }
        catch (Exception e)
        {
            Debug.LogError("Empty Sprite prefab has been moved from its hard-linked position");
            Debug.LogException(e);
        }
        personFace.GenerateFace(UICard.imageParent);//Generate the persons face
    }
}
