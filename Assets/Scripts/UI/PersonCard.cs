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
    public Person _person;
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
        if (_person == null)
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
         yield return new WaitUntil(() => _person.emptyImage != null);
        _person.GenerateFace(imageParent);
        nameText.text = _person.givenName + " " + _person.familyName;//set the name text
        factionText.text = _person.faction.ToString();//set the faction text
    }
}
