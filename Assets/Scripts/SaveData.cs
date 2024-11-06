using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string type;
    public string[] keys;
    public string[] values;
    
    public SaveData(string classType, string[] keyArr, string[] valueArr)
    {
        type = classType;
        keys = keyArr;
        values = valueArr;
    }
    public string GetValue(string key)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i] == key)
            {
                return values[i];
            }
        }
        Debug.LogWarning($"Value matching provided key |{key}| was not found, ensure that the object loading this is of type |{type}|, returning NULL.");
        return "NULL";
    }
}
