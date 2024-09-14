using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class TestFaceGenerator : MonoBehaviour
{
    Sprite[] eyes;
    Sprite[] mouths;
    Sprite[] faceShapes;
    Sprite[] hair;

    public FaceElements[] faces;
    public Color[] hairColors;
    public Color[] mouthColors;

    public SpriteRenderer eyeRenderer;
    public SpriteRenderer mouthRenderer;
    public SpriteRenderer faceShapeRenderer;
    public SpriteRenderer hairRenderer;

    // Start is called before the first frame update
    void Start()
    {
        int elementUpperLimit = faces.Length;
        int elementSelector = UnityEngine.Random.Range(0,elementUpperLimit);
        eyes = faces[elementSelector].eyes;
        mouths = faces[elementSelector].mouths;
        faceShapes = faces[elementSelector].faceShapes;
        hair = faces[elementSelector].hair;
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
        eyeRenderer.sprite = eyes[GenerateSelector(eyes)];
        mouthRenderer.sprite = mouths[GenerateSelector(mouths)];
        faceShapeRenderer.sprite = faceShapes[GenerateSelector(faceShapes)];
        hairRenderer.sprite = hair[GenerateSelector(hair)];
        hairRenderer.color = hairColors[GenerateSelector(hairColors)];
        mouthRenderer.color = mouthColors[GenerateSelector(mouthColors)];
    }

    int GenerateSelector<T>(T[] arr)
    {
        return UnityEngine.Random.Range(0, arr.Length);
    }
}
