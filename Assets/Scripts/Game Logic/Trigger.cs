using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Trigger : MonoBehaviour
{
    public UnityEvent onEnter;
    public UnityEvent onStay;
    public UnityEvent onExit;
    public UnityEvent onInteract;
    public UnityEvent onAttack;
    private enum TriggerType
    {
        OnEnter,
        OnStay,
        OnExit,
        OnInteract,
        OnAttack
    }

    public Collider2D triggerCol;
    [SerializeField] private TriggerType triggerType;
    float timer = 0f;
    public float stayTime = 5f;
    public string detectTag = "Player";
    // Start is called before the first frame update
    void Start()
    {
        if (triggerCol != null)
        {
            triggerCol.isTrigger = true;
        }
        else
        {
            Debug.LogError($"Trigger collider on {gameObject.name} is not set.");
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(detectTag))
        {
            timer = Time.time;
            if (triggerType == TriggerType.OnEnter)
            {
                onEnter.Invoke();
            }
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag(detectTag))
        {
            if (triggerType == TriggerType.OnStay && (Time.time - timer) >= stayTime)
            {
                onStay.Invoke();
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag(detectTag))
        {
            timer = 0f;
            if (triggerType == TriggerType.OnExit)
            {
                onExit.Invoke();
            }
        }
    }
}
