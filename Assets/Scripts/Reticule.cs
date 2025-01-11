using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

public class Reticule : MonoBehaviour
{
    public RuntimeAnimatorController backupAnimation;
    public Animator anim;
    public AYellowpaper.SerializedCollections.SerializedDictionary<Faction, RuntimeAnimatorController> ReticuleAnimations;
    // Start is called before the first frame update
    void Start()
    {
        anim.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetFaction(Faction faction)
    {
        try
        {
            anim.runtimeAnimatorController = ReticuleAnimations[faction];
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error setting reticule sprite: {e}");
            anim.runtimeAnimatorController = backupAnimation;
        }
    }
}
