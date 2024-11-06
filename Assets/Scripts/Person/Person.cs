using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GameConstants;

[System.Serializable]
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
    public Faction faction;//The faction the person is affiliated with
    [HideInInspector] public float personReputation;//The reputation the player has with this person
    #region Face
    [JsonIgnore] public List<FaceElements> faces;
    FaceElements set;
    [NonSerialized] public GameObject emptyImage;
    List<Sprite> sprites;
    [HideInInspector] public bool hasGenerated = false;
    [HideInInspector] public int setSelector;
    [HideInInspector] public List<int> faceIndices;
    [HideInInspector] public List<int> colorIndices;
    [HideInInspector] public bool[] toColorArr;
    [HideInInspector] public bool[] distinctiveComponent;
    int elementsLen;
    #endregion

    public void Start()
    {
        LoadFaceAssets();
        
    }

    public void LoadFaceAssets()
    {
        GameObject[] g = Resources.LoadAll<GameObject>("Prefabs/Face Element Sets");//Load all of the face elements in the face element sets folder in resources
        foreach (GameObject set in g)
        {
            FaceElements f = set.GetComponent<FaceElements>();//Get the faceElements component of each prefab and check if the faction of the set matches the person, if so add the set to the list of potential face sets
            if (faction == f.setFaction)
            {
                faces.Add(f);
            }
        }
        try
        {
            emptyImage = (GameObject)Resources.Load("Prefabs/Classless Prefabs/EmptyImage");//load the empty image prefab to create an empty sprite renderer and transform for the face generation
        }
        catch (Exception e)
        {
            Debug.LogError("Empty Sprite prefab has been moved from its hard-linked position");
            Debug.LogException(e);
        }
    }
    public void GenerateFace(RectTransform spawnPoint)//This function generates a new face at random
    {
        if (hasGenerated)//Check if a face has already been generated, if so recreate a face
        {
            set = faces[setSelector];
            List<Image> imageHolders = new();
            elementsLen = set.parts.Length;
            foreach (FaceElements.PartStruct sprites in set.parts)//Create sprite renderers to hold the items of the face
            {
                GameObject newObject = Instantiate(emptyImage, spawnPoint);
                imageHolders.Add(newObject.GetComponent<Image>());
            }
            imageHolders.Reverse();
            for (int i = 0; i < elementsLen; i++)
            {
                Sprite itemSprite = set.parts.ElementAt(i).part[faceIndices[i]];
                imageHolders[i].sprite = itemSprite;
                if (set.partsToBeColored[i] == true)
                {
                    imageHolders[i].color = set.colors.ElementAt(i).color[colorIndices[i]];
                }
            }
        }
        else
        {
            hasGenerated = true;//set the face to have been generated before

            setSelector = GenerateSelector(faces);//Generate a selector for which face set to use

            set = faces[setSelector];//select a set and save data of the set to variables
            distinctiveComponent = set.distinctiveComponent;
            toColorArr = set.partsToBeColored;

            List<Image> imageHolders = new();
            elementsLen = set.parts.Length;
            foreach (FaceElements.PartStruct sprites in set.parts)//create a corresponding amount of image objects to the number of parts in the face
            {
                GameObject newObject = Instantiate(emptyImage, spawnPoint);
                imageHolders.Add(newObject.GetComponent<Image>());
            }
            imageHolders.Reverse();//reverse the order to have the lowest rendering image object first
            for (int i = 0; i < elementsLen; i++)//Go through each part category in the selected set
            {
                int faceIndex = GenerateSelector(set.parts.ElementAt(i).part);//get the index of which part option should be selected
                Sprite itemSprite = set.parts.ElementAt(i).part[faceIndex];//get the corresponding sprite from that index
                imageHolders[i].sprite = itemSprite;//assign it to the image object
                faceIndices.Add(faceIndex);//add the index to the faceIndices list to keep a record of the indices used for the face

                if (set.partsToBeColored[i] == true)//checks if the selected part needs to be coloured, selects a color index, uses that to select the color and then apply it to the image. 
                {
                    int colorIndex = GenerateSelector(set.colors.ElementAt(i).color);
                    imageHolders[i].color = set.colors.ElementAt(i).color[colorIndex];
                    colorIndices.Add(colorIndex);
                }
            }
        }
    }
    //Two functions for selecting a random number within the length of the array, maybe redundant but seemed useful
    int GenerateSelector<T>(T[] arr)
    {
        return UnityEngine.Random.Range(0, arr.Length);
    }
    int GenerateSelector<T>(List<T> arr)
    {
        return UnityEngine.Random.Range(0, arr.Count);
    }
}
