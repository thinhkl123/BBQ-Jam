using Athena.Common.UI;
using Atom;
using SoundManager;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoSingleton<HomeManager> 
{
    protected HomeUI _homeUI;

    private static bool hasLoaded = false;

    private void Start()
    {
        if (!hasLoaded)
        {
            hasLoaded = true;
            CreateObject("LoadingManager", "LoadingManager");
            CreateObject("Controllers/UIManager", "UIManager");
            CreateObject("Controllers/DataManager", "DataManager");
        }

        GameModeContainer.Instance.InitGame();

        CreateObjects();

        Setup();
    }

    private void CreateObjects()
    {
        CreateModule("Controllers/SettingManager", "SettingManager");


        SettingManager.Instance.Setup();
    }

    private GameObject CreateObject(string module, string nameModule)
    {
        GameObject loginObject = GameObject.Instantiate(Resources.Load<GameObject>(module));
        loginObject.name = nameModule;

        return loginObject;
    }

    private GameObject CreateModule(string module, string nameModule)
    {
        GameObject loginObject = GameObject.Instantiate(Resources.Load<GameObject>(module), GameModeContainer.Instance.Manager.transform);
        loginObject.name = nameModule;

        return loginObject;
    }

    public void Setup()
    {
        _homeUI = AppManager.Instance.ShowSafeTopUI<HomeUI>("UI/HomeUI");

        _homeUI.OnSettingBtn = SettingManager.Instance.ShowUI;
        _homeUI.OnPlayBtn = PlayGame;

        _homeUI.SetupLevel();
    }

    private void PlayGame()
    {
        HideUI();
        LoadingManager.instance.LoadScene("Level " + DataManager.Instance.LevelData.Levels[GameManager.Instance.currentLevel].LevelId.ToString());
    }

    public void ShowUI()
    {
        _homeUI.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        _homeUI.gameObject.SetActive(false);
    }
}
