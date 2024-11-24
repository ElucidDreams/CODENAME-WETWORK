using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
[RequireComponent(typeof(Operator))]
public class Player : MonoBehaviour
{
    public Operator operatorComp; 
    [Header("Player Properties")]
    public InteractSensor reticleSensor; 
    public Transform reticleTransform;
    [HideInInspector] private Vector2 aimPoint;
    [NonSerialized] public Vector2 movementInput;
    [NonSerialized] public Vector2 movementVector;
    private DefaultPlayerActions defaultPlayerActions;
    private InputAction moveAction;
    private InputAction lookAction;
    private Camera mainCamera;

    public void Awake()
    {
        operatorComp = GetComponent<Operator>();
        defaultPlayerActions = new DefaultPlayerActions();//Sets up input system
        defaultPlayerActions.Enable();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;//establishes camera for world point conversions
        }
    }
    private void OnEnable()//enables actions for input
    {
        moveAction = defaultPlayerActions.TopDown.Move;
        moveAction.Enable();
        lookAction = defaultPlayerActions.TopDown.Look;
        lookAction.Enable();
        defaultPlayerActions.TopDown.Attack.Enable();
        defaultPlayerActions.TopDown.Throw.Enable();
        defaultPlayerActions.TopDown.Interact.Enable();
        defaultPlayerActions.TopDown.Attack.performed += OnAttack;
        defaultPlayerActions.TopDown.Throw.performed += OnThrow;
        defaultPlayerActions.TopDown.Interact.performed += OnInteract;
    }

    private void OnDisable()//disables actions for input
    {
        moveAction.Disable();
        lookAction.Disable();
        defaultPlayerActions.TopDown.Attack.Disable();
        defaultPlayerActions.TopDown.Throw.Disable();
        defaultPlayerActions.TopDown.Interact.Disable();
        defaultPlayerActions.TopDown.Attack.performed -= OnAttack;
        defaultPlayerActions.TopDown.Throw.performed -= OnThrow;
        defaultPlayerActions.TopDown.Interact.performed -= OnInteract;
    }
    // Start is called before the first frame update
    public void Start()
    {
        operatorComp.rotTarget = reticleTransform;
    }
    // Update is called once per frame
    void Update()
    {
        aimPoint = lookAction.ReadValue<Vector2>();//reads mouse point in screen coordinates
        Vector3 reticleWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(aimPoint.x, aimPoint.y, 0));//converts screen coordinates to world coordinate 
        reticleWorldPos.z += -1 * reticleWorldPos.z;//removes offset created by pixel perfect camera module
        reticleTransform.position = reticleWorldPos;//sets reticle to the mouse pos
    }
    void FixedUpdate()
    {
        movementInput = moveAction.ReadValue<Vector2>();//reads the movement inputs
        movementVector = movementInput * operatorComp.effectiveSpeed;//multiplies the input by the speed of the character
        operatorComp.rbComp.AddForce(movementVector);//adds said force to the objects rigidbody
        if (operatorComp.rbComp.velocity.magnitude > operatorComp.maxSpeed)//checks if the speed is exceeding the maximum speed of the character
        {
            operatorComp.rbComp.velocity = operatorComp.rbComp.velocity.normalized * operatorComp.maxSpeed;//if it is, set the speed to the max speed
        }
        operatorComp.motionVec = operatorComp.rbComp.velocity;
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (operatorComp.activeWeapon != null)
            {
                if (!operatorComp.activeWeapon.isAttacking)
                {
                    operatorComp.activeWeapon.Attack();
                }
            }
        }
    }
    public void OnThrow(InputAction.CallbackContext context)
    {
        operatorComp.WeaponThrow();
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (operatorComp.sensor.item == reticleSensor.item)
        {
            if (operatorComp.sensor.item.CompareTag("Weapon"))
            {
                Destroy(operatorComp.activeWeapon.gameObject);
                operatorComp.activeWeapon = null;
                operatorComp.PickupWeapon(operatorComp.sensor.item.GetComponent<Weapon>());
            }
        }
    }
}