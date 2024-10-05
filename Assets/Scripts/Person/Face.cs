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
    FaceElements elementSet;
    [NonSerialized] public GameObject emptyImage;
    List<Sprite> sprites;

    int elementsLen;

    public void GenerateFace(Faction faction, RectTransform spawnPoint)
    {
        List<FaceElements> factionSet = new();
        foreach (FaceElements set in faces)
        {
            if (faction.IdCheck(set.setFaction))
            {
                factionSet.Add(set);
            }
        }
        int elementUpperLimit = factionSet.Count;
        int elementSelector = UnityEngine.Random.Range(0,elementUpperLimit);
        elementSet = factionSet[elementSelector];
        List<Image> imageHolders = new();
        elementsLen = elementSet.elements.Length;
        foreach (FaceElements.ElementsStruct sprites in elementSet.elements)//Create sprite renderers to hold the items of the face
        {
            GameObject newObject = Instantiate(emptyImage,spawnPoint);
            imageHolders.Add(newObject.GetComponent<Image>());
        }
        imageHolders.Reverse();
        for (int i = 0; i < elementsLen; i++)
        {
            Sprite itemSprite = elementSet.elements.ElementAt(i).element[GenerateSelector(elementSet.elements.ElementAt(i).element)];
            imageHolders[i].sprite = itemSprite;
            if (elementSet.elementsToBeColored[i] == true)
            {
                imageHolders[i].color = elementSet.colors.ElementAt(i).color[GenerateSelector(elementSet.colors.ElementAt(i).color)];
            }
        }
    }

    int GenerateSelector<T>(T[] arr)
    {
        return UnityEngine.Random.Range(0, arr.Length);
    }
}
