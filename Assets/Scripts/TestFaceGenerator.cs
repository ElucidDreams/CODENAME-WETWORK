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

    public SpriteRenderer eyeRenderer;
    public SpriteRenderer mouthRenderer;
    public SpriteRenderer faceShapeRenderer;

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
        int eyeUpperLimit = eyes.Length - 1;
        int mouthUpperLimit = mouths.Length - 1;
        int faceShapeUpperLimit = faceShapes.Length - 1;
        int eyeSelector = UnityEngine.Random.Range(0,eyeUpperLimit);
        int mouthSelector = UnityEngine.Random.Range(0,mouthUpperLimit);
        int faceShapeSelector = UnityEngine.Random.Range(0,faceShapeUpperLimit);
        eyeRenderer.sprite = eyes[eyeSelector];
        mouthRenderer.sprite = mouths[mouthSelector];
        faceShapeRenderer.sprite = faceShapes[faceShapeSelector];
    }
}
