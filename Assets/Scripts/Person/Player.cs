using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Operator
{
    [Header("Player Properties")]
    public Transform reticleTransform;

    [NonSerialized] private Vector2 aimPoint;
    [NonSerialized] public Vector2 movementInput;
    [NonSerialized] public Vector2 movementVector;
    private DefaultPlayerActions _defaultPlayerActions;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private Camera mainCamera;

    void Awake()
    {
        _defaultPlayerActions = new DefaultPlayerActions();
        _defaultPlayerActions.Enable();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void OnEnable()
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

    private void OnDisable()
    {
        _moveAction.Disable();
        _lookAction.Disable();
        _defaultPlayerActions.TopDown.Attack.Disable();
        _defaultPlayerActions.TopDown.Throw.Disable();
        _defaultPlayerActions.TopDown.Attack.performed -= OnAttack;
        _defaultPlayerActions.TopDown.Throw.performed -= OnThrow;
    }
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
        animComp.SetBool("isWalking", movementInput != Vector2.zero);
        aimPoint = _lookAction.ReadValue<Vector2>();
        reticleTransform.position = mainCamera.ScreenToWorldPoint(new Vector3(aimPoint.x, aimPoint.y, mainCamera.nearClipPlane));
        RotateToPoint(reticleTransform.position);
    }

    void FixedUpdate()
    {
        movementInput = _moveAction.ReadValue<Vector2>();
        movementVector = movementInput * effectiveSpeed;
        rb.AddForce(movementVector);
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
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
        if (activeWeapon != null)
        {
            WeaponThrow();
        }
    }
}
