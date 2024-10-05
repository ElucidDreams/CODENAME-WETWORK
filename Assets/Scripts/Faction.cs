using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

public class Faction : MonoBehaviour
{
    public AvailableFactions factionID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IdCheck(AvailableFactions testId)
    {
        return factionID == testId;
    }
}
