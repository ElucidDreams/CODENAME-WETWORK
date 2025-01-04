using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameConstants;

public class Reticule : MonoBehaviour
{
    public AnimationClip backupAnimation;
    public Animation anim;
    public AYellowpaper.SerializedCollections.SerializedDictionary<Faction, AnimationClip> ReticuleAnimations;
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
            anim.clip = ReticuleAnimations[faction];
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error setting reticule sprite: {e}");
            anim.clip = backupAnimation;
        }
        anim.Play();
    }
}
