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
    int elementsLen;

    public void GenerateFace(RectTransform spawnPoint)
    {
        if (!hasGenerated)
        {
            hasGenerated = true;
            int elementUpperLimit = faces.Count;
            setSelector = UnityEngine.Random.Range(0, elementUpperLimit);
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
                int faceIndex = GenerateSelector(set.parts.ElementAt(i).part);
                Sprite itemSprite = set.parts.ElementAt(i).part[faceIndex];
                imageHolders[i].sprite = itemSprite;
                faceIndices.Add(faceIndex);
                if (set.partsToBeColored[i] == true)
                {
                    int colorIndex = GenerateSelector(set.colors.ElementAt(i).color);
                    imageHolders[i].color = set.colors.ElementAt(i).color[colorIndex];
                    colorIndices.Add(colorIndex);
                }
            }
        }
    }

    public void GenerateFace(List<int> fIndex, List<int> cIndex, RectTransform spawnPoint)
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

    int GenerateSelector<T>(T[] arr)
    {
        return UnityEngine.Random.Range(0, arr.Length);
    }

    public void Save()
    {
        
    }
}
