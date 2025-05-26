using Athena.Common.UI;
using Atom;
using OneID;
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
        Debug.Log("Home Start");

        if (!hasLoaded)
        {
            hasLoaded = true;
            DontDestroyOnLoad(CreateObject("PortraitLoginManager", "LoginManager"));
            DontDestroyOnLoad(CreateObject("Controllers/GameManager", "GameManager"));
            //CreateObject("LoadingManager", "LoadingManager");
            CreateObject("Controllers/UIManager", "UIManager");
            CreateObject("Controllers/DataManager", "DataManager");
            CreateObject("SoundsManager", "SoundsManager");
            DontDestroyOnLoad(CreateObject("Controllers/WinManager", "WinManaer"));
            DontDestroyOnLoad(CreateObject("Controllers/LostManager", "LostManager"));
            DontDestroyOnLoad(CreateObject("Controllers/PauseManager", "PauseManager"));
            DontDestroyOnLoad(CreateObject("Controllers/SettingManager", "SettingManager"));
            DontDestroyOnLoad(CreateObject("LeaderBoardManager", "LeaderBoardManager"));

            SettingManager.Instance.Setup();
            WinManager.Instance.Setup();
            LostManager.Instance.Setup();
            PauseManager.Instance.Setup();
            LeaderboardManager.Instance.Setup();

            SoundsManager.Instance.PlayMusic(SoundType.GameMusic);

            Setup();
        }
        else
        {
            LoginManager.Instance.ShowSDK();

            _homeUI = AppManager.Instance.ShowSafeTopUI<HomeUI>("UI/HomeUI");
            SetupLevel();
        }

        GameModeContainer.Instance.InitGame();

        CreateObjects();

        //Setup();
    }

    private void CreateObjects()
    {
        //CreateModule("LeaderBoardManager", "LeaderBoardManager");
        CreateModule("Controllers/SettingManager", "SettingManager");

        //LeaderboardManager.Instance.Setup();
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
        _homeUI.OnRankingBtn = LeaderboardManager.Instance.ShowUI;

        //_homeUI.SetupLevel();
        StartCoroutine(SetupLevelCoroutine());
    }

    private IEnumerator SetupLevelCoroutine()
    {
        yield return new WaitForSeconds(2f);

        Debug.Log("Setup Level");

        _homeUI.SetupLevel(DataManager.Instance.GetCurMaxLevel());
    }

    private void SetupLevel()
    {
        Debug.Log("Setup Level");
        _homeUI.SetupLevel(PlayerPrefs.GetInt("MaxLevel", 1));
    }

    private void PlayGame()
    {
        HideUI();
        GameManager.Instance.PlayGame();
        //LoadingManager.instance.LoadScene("Level " + DataManager.Instance.LevelData.Levels[GameManager.Instance.currentLevel-1].LevelId.ToString());
    }

    public void ShowUI()
    {
        _homeUI.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        //_homeUI.HideObject();
        _homeUI.gameObject.SetActive(false);
    }
}
