using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Operator
{
    private Vector2 movementInput;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        GenerateFace();
        InitEffectiveValues();
        InitSkills();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

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
}
