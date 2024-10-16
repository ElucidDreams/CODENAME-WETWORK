using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using static GameConstants;
// Does not need to be saved as it is saved as a prefab and loaded in the person script. 
public class FaceElements : MonoBehaviour
{    
    [System.Serializable]
    public struct PartStruct//A struct so that an array of arrays can be used and interacted with in the unity editor
    {
        [SerializeField] public Sprite[] part;//the array of individual sprites, making up a selection of potential options for that particular part (i.e mouth a, mouth b)
    }

    [System.Serializable]
    public struct ColorStruct//A struct so that an array of arrays can be used and interacted with in the unity editor
    {
        [SerializeField] public Color[] color;//the array of different colors for that particular part
    }
    [Tooltip("Use this to designate the Faction of this set of face elements")]
    public GameConstants.AvailableFactions setFaction;
    
    [Tooltip("Higher position in the editor correlates to higher in the render order")]
    public PartStruct[] parts;//The array of part types that construct the face (i.e eyes, face shape, mouth)
    [Tooltip("Even if an element is not colored, place an empty array to keep spacing correct")]
    public ColorStruct[] colors;//An array of colorStructs, with each entry corresponding to a part of the face
    [Tooltip("Match to the number of elements and toggle what elements you want colored")]
    public bool[] partsToBeColored;//A checklist in the form of a boolean array that tells what elements should be colored, potentially could be removed by checking which colors elements are empty, but if the ability to toggle coloring on and off a part is wanted, this array is needed
    [Tooltip("Use this to set which component determines the appearance of the head from top down. \nONLY SET ONE TO BE TRUE AND SAME LENGTH AS NUMBER OF PART CATEGORIES")]
    public bool[] distinctiveComponent;
}
