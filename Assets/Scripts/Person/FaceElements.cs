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
        [SerializeField] public Color[] color;//the array of different colours for that particular part
    }
    [Tooltip("Use this to designate the Faction of this set of face elements")]
    public GameConstants.AvailableFactions setFaction;
    
    [Tooltip("Higher position in the editor correlates to higher in the render order")]
    public PartStruct[] parts;//The array of part types that construct the face (i.e eyes, face shape, mouth)
    [Tooltip("Even if an element is not colored, place an empty array to keep spacing correct")]
    public ColorStruct[] colors;
    [Tooltip("Match to the number of elements and toggle what elements you want colored")]
    public bool[] partsToBeColored;

}
