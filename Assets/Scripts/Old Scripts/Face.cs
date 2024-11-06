using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class Face : MonoBehaviour
{
    public List<FaceElements> faces;
    FaceElements set;
    [HideInInspector] public GameObject emptyImage;
    List<Sprite> sprites;
    [HideInInspector] public bool hasGenerated = false;
    [HideInInspector] public int setSelector;
    [HideInInspector] public List<int> faceIndices;
    [HideInInspector] public List<int> colorIndices;
    [HideInInspector] public bool[] toColorArr;
    [HideInInspector] public bool[] distinctiveComponent;
    [HideInInspector] public RectTransform savedSpawnPoint;
    int elementsLen;

    public void GenerateFace(RectTransform spawnPoint)//This function generates a new face at random
    {
        if (!hasGenerated)//Check if a face has already been generated, if so recreate a face
        {
            savedSpawnPoint = spawnPoint;//save the spawn point parameter to a variable
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
        else
        {
            RecreateFace(faceIndices, colorIndices, savedSpawnPoint);//TODO Review if this is the vest way to implement this once saving is somewhat implemented
        }
    }

    public void RecreateFace(List<int> fIndex, List<int> cIndex, RectTransform spawnPoint)//Does much of the same as Generate face but uses both parameter values and other saved data to recreate the face of an already generated character
    {
        if (hasGenerated)
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
                Sprite itemSprite = set.parts.ElementAt(i).part[fIndex[i]];
                imageHolders[i].sprite = itemSprite;
                if (set.partsToBeColored[i] == true)
                {
                    imageHolders[i].color = set.colors.ElementAt(i).color[cIndex[i]];
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
    
    public void Save()
    {

    }
}
