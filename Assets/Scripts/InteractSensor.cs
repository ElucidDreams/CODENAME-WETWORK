using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TestTools;

public class InteractSensor : MonoBehaviour
{
    CircleCollider2D pickupCollider;
    public GameObject item = null;
    // Start is called before the first frame update
    void Start()
    {
        pickupCollider = GetComponent<CircleCollider2D>();
        pickupCollider.isTrigger = true;
        StartCoroutine(ItemDebug());
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Weapon"))
        {
            item = collider.gameObject;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Weapon"))
        {
            item = null;
        }
    }
    public IEnumerator ItemDebug()
    {
        yield return new WaitUntil(() => item != null);
        Debug.Log(item.name);
        yield return new WaitForSeconds(1f);
        StartCoroutine(ItemDebug());
    }
}
