using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Operator
{
    // Start is called before the first frame update
    void Start()
    {
        if (!inMission)
        {
            GenerateFace();
        }
        InitEffectiveValues();
        InitSkills();
        InitComponents();
    }

    // Update is called once per frame
    void Update()
    {
        if (movementInput == Vector2.zero)
        {
            animComp.SetBool("isWalking",false);
        }
        else
        {
            animComp.SetBool("isWalking",true);
        }
    }

    void FixedUpdate()
    {
        Vector2 movement = movementInput * effectiveSpeed;
        rb.AddForce(movement);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public void Walk(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void Look(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
        RotateToPoint(context.ReadValue<Vector2>());
    }
}
