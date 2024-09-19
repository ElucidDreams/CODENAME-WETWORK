using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Operator
{
    // Start is called before the first frame update
    void Start()
    {
        GenerateFace();
        InitEffectiveValues();
        InitSkills();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}
