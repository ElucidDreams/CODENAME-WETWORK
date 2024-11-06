using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Operator _test;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        Save();
        _test.givenName = "Lol";
        _test.level = 2;
        _test.faction = GameConstants.Faction.Army;
        Load();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public async void Save()
    {
        string data = JsonUtility.ToJson(_test,true);
        Debug.Log(data);
        await File.WriteAllTextAsync( Application.persistentDataPath + "/data.json", data);
        Debug.Log("Save Done");
    }
    public async void Load()
    {
        string data = await File.ReadAllTextAsync(Application.persistentDataPath + "/data.json");
        JsonUtility.FromJsonOverwrite(data, _test);
        Debug.Log("Load Done");
    }
}
