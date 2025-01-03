using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[System.Serializable]
public class PersonCard : MonoBehaviour
{
    public RectTransform imageParent;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI factionText;
    public Person person;
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
        if (person == null)
        {
            Debug.LogWarning(gameObject.name + "'s _person is unset");
        }
        else
        {
            StartCoroutine(setElements());
        }
    }
    public IEnumerator setElements()
    {
         yield return new WaitUntil(() => person.emptyImage != null);
        person.GenerateFace(imageParent);
        nameText.text = person.givenName + " " + person.familyName;//set the name text
        factionText.text = person.faction.ToString();//set the faction text
    }
}
