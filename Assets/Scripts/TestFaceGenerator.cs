using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class TestFaceGenerator : MonoBehaviour
{
    public Sprite[] eyes;
    public Sprite[] mouths;
    public Sprite[] faceShapes;

    public Sprite[] hair;

    public SpriteRenderer eyeRenderer;
    public SpriteRenderer mouthRenderer;
    public SpriteRenderer faceShapeRenderer;
    public SpriteRenderer hairRenderer;

    // Start is called before the first frame update
    void Start()
    {
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
        int eyeUpperLimit = eyes.Length;
        int mouthUpperLimit = mouths.Length;
        int faceShapeUpperLimit = faceShapes.Length;
        int hairUpperLimit = hair.Length - 1;
        int eyeSelector = UnityEngine.Random.Range(0,eyeUpperLimit);
        int mouthSelector = UnityEngine.Random.Range(0,mouthUpperLimit);
        int faceShapeSelector = UnityEngine.Random.Range(0,faceShapeUpperLimit);
        int hairSelector = UnityEngine.Random.Range(0,hairUpperLimit);
        eyeRenderer.sprite = eyes[eyeSelector];
        mouthRenderer.sprite = mouths[mouthSelector];
        faceShapeRenderer.sprite = faceShapes[faceShapeSelector];
        hairRenderer.sprite = hair[hairSelector];
    }
}
