using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : Operator
{
    [Header("Player Properties")]
    public Transform reticleTransform;
    [HideInInspector] private Vector2 aimPoint;
    [HideInInspector] public Vector2 movementInput;
    [HideInInspector] public Vector2 movementVector;
    private DefaultPlayerActions _defaultPlayerActions;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private Camera mainCamera;

    void Awake()
    {
        _defaultPlayerActions = new DefaultPlayerActions();//Sets up input system
        _defaultPlayerActions.Enable();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;//establishes camera for world point conversions
        }
    }

    private void OnEnable()//enables actions for input
    {
        _moveAction = _defaultPlayerActions.TopDown.Move;
        _moveAction.Enable();
        _lookAction = _defaultPlayerActions.TopDown.Look;
        _lookAction.Enable();
        _defaultPlayerActions.TopDown.Attack.Enable();
        _defaultPlayerActions.TopDown.Throw.Enable();
        _defaultPlayerActions.TopDown.Attack.performed += OnAttack;
        _defaultPlayerActions.TopDown.Throw.performed += OnThrow;
    }

    private void OnDisable()//disables actions for input
    {
        _moveAction.Disable();
        _lookAction.Disable();
        _defaultPlayerActions.TopDown.Attack.Disable();
        _defaultPlayerActions.TopDown.Throw.Disable();
        _defaultPlayerActions.TopDown.Attack.performed -= OnAttack;
        _defaultPlayerActions.TopDown.Throw.performed -= OnThrow;
    }
    // Start is called before the first frame update
    public new void Start()
    {
        rotTarget = reticleTransform;
        base.Start();
        
    }

    // Update is called once per frame
    void Update()
    {
        aimPoint = _lookAction.ReadValue<Vector2>();//reads mouse point in screen coordinates
        Vector3 reticleWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(aimPoint.x, aimPoint.y, 0));//converts screen coordinates to world coordinate 
        reticleWorldPos.z += -1*reticleWorldPos.z;//removes offset created by pixel perfect camera module
        reticleTransform.position = reticleWorldPos;//sets reticle to the mouse pos
        BodyUpdate();
    }

    void FixedUpdate()
    {
        movementInput = _moveAction.ReadValue<Vector2>();//reads the movement inputs
        movementVector = movementInput * effectiveSpeed;//multiplies the input by the speed of the character
        rbComp.AddForce(movementVector);//adds said force to the objects rigidbody
        if (rbComp.velocity.magnitude > maxSpeed)//checks if the speed is exceeding the maximum speed of the character
        {
            rbComp.velocity = rbComp.velocity.normalized * maxSpeed;//if it is, set the speed to the max speed
        }        
        motionVec = rbComp.velocity;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (activeWeapon != null)
            {
                activeWeapon.Attack();
            }
        }
    }
    public void OnThrow(InputAction.CallbackContext context)
    {
        WeaponThrow();
    }
}
