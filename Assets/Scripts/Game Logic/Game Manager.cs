using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameConstants;


public class GameManager : MonoBehaviour
{
    private GameManager singleton;
    public string playerGivenName = "";
    public string playerFamilyName = "";
    public Faction playerFaction = Faction.Army;
    public TextMeshProUGUI givenNameInput;
    public TextMeshProUGUI familyNameInput;
    public TMP_Dropdown factionDropdown;


    private void Awake()
    {
        if(singleton && singleton != this)
        {
            Destroy(gameObject);
        }
        singleton = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
    }

    public async void SaveGM()
    {
        playerGivenName = givenNameInput.text;
        playerFamilyName = familyNameInput.text;
        playerFaction = (Faction)factionDropdown.value + 1;
        string data = JsonUtility.ToJson(this, true);
        Debug.Log(data);
        await File.WriteAllTextAsync(Application.persistentDataPath + "/data.json", data);
        Debug.Log("Save Done");
    }
    public async void LoadGM()
    {
        string data = await File.ReadAllTextAsync(Application.persistentDataPath + "/data.json");
        JsonUtility.FromJsonOverwrite(data, this);
        Debug.Log("Load Done");
    }
/*
    public static IEnumerator LoadLevel(string sceneName)
    {
        SaveGM();
        var asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, load)
    }
    */
    public void LoadScene(int sceneInt)
    {
        SaveGM();
        SceneManager.LoadScene(sceneInt);
    }

    public void WinTransition()
    {
        StartCoroutine(WinLevel());
    }
    public void LoseTransition()
    {
        StartCoroutine(LoseLevel());
    }
    public IEnumerator WinLevel()
    {
        SceneManager.LoadScene("WinScene");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainMenu");
    }
    public IEnumerator LoseLevel()
    {
        SceneManager.LoadScene("LoseScene");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainMenu");
    }
}
