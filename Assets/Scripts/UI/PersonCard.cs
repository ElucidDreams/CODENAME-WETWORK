using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PersonCard : MonoBehaviour
{
    public RectTransform imageParent;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI factionText;

    void Start()
    {
        if (imageParent == null)
        {
            Debug.LogWarning(gameObject.name + "'s imageParent RectTransform is unset");
        }
        if (nameText == null)
        {
            Debug.LogWarning(gameObject.name + "'s nameText TMP is unset");
        }
        if (factionText == null)
        {
            Debug.LogWarning(gameObject.name + "'s factionText TMP is unset");
        }
    }
}
