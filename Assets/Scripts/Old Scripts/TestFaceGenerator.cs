using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class TestFaceGenerator : MonoBehaviour
{
    public FaceElements[] faces;
    FaceElements elementSet;
    public GameObject emptySprite;

    List<SpriteRenderer> spriteHolders;

    int elementsLen;

    /*
    // Start is called before the first frame update
    void Start()
    {
        int elementUpperLimit = faces.Length;
        int elementSelector = UnityEngine.Random.Range(0,elementUpperLimit);
        elementSet = faces[elementSelector];
        spriteHolders = new List<SpriteRenderer>();
        elementsLen = elementSet.elements.Length;
        foreach (FaceElements.ElementsStruct sprites in elementSet.elements)
        {
            GameObject newObject = Instantiate(emptySprite,gameObject.transform);
            spriteHolders.Add(newObject.GetComponent<SpriteRenderer>());
            spriteHolders[spriteHolders.Count-1].sortingOrder = (spriteHolders.Count-1)*-1;
        }

        updateFace();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            updateFace();
        }
    }

    void updateFace()
    {
        for (int i = 0; i < elementsLen; i++)
        {
            Sprite itemSprite = elementSet.elements.ElementAt(i).element[GenerateSelector(elementSet.elements.ElementAt(i).element)];
            spriteHolders[i].sprite = itemSprite;
            if (elementSet.elementsToBeColored[i] == true)
            {
                spriteHolders[i].color = elementSet.colors.ElementAt(i).color[GenerateSelector(elementSet.colors.ElementAt(i).color)];
            }
        }
    }

    int GenerateSelector<T>(T[] arr)
    {
        return UnityEngine.Random.Range(0, arr.Length);
    }
    */
}
