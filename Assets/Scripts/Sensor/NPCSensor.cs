using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCSensor : MonoBehaviour
{
    public TestNPC parentNPC;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            parentNPC.inAttackRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            parentNPC.inAttackRange = false;
        }
    }
}
