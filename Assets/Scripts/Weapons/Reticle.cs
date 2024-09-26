using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    private Camera cam;
    void Start()
    {
        cam = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y, 0);
    }
}
