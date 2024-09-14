using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceElements : MonoBehaviour
{    
    [System.Serializable]
    public struct ElementsStruct
    {
        [SerializeField] public Sprite[] element;
    }

    [System.Serializable]
    public struct ColorsStruct
    {
        [SerializeField] public Color[] color;
    }
    
    [Tooltip("Higher position in the editor correlates to higher in the render order")]
    public ElementsStruct[] elements;
    [Tooltip("Even if an element is not colored, place an empty array to keep spacing correct")]
    public ColorsStruct[] colors;
    [Tooltip("Match to the number of elements and toggle what elements you want colored")]
    public bool[] elementsToBeColored;

}
