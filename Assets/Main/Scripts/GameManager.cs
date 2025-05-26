using Atom;
using OneID;
using SoundManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public int MaxLevel = 15;
    public int currentLevel;
    public bool isLost;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void WinGame()
    {
        Debug.Log("Win");
        int curMaxLevel = PlayerPrefs.GetInt("MaxLevel", 1);
        if (currentLevel >= curMaxLevel)
        {
            LeaderboardManager.Instance.UpdateLeaderboard(currentLevel);
            if (currentLevel + 1 <= MaxLevel)
            {
                DataManager.Instance.UpdateUserData(currentLevel + 1);
                PlayerPrefs.SetInt("MaxLevel", currentLevel+1);
            }
        }
        Invoke(nameof(ShowUIWin), 1f);
    }

    private void ShowUIWin()
    {
        AppManager.Instance.PauseGame(true);
        SoundsManager.Instance.PlaySFX(SoundType.Win);
        WinManager.Instance.ShowUI();
    }

    public void LoseGame()
    {
        isLost = true;
        Invoke(nameof(ShowUILose), 0.5f);
    }

    private void ShowUILose()
    {
        Debug.Log("Lose");
        AppManager.Instance.PauseGame(true);
        SoundsManager.Instance.PlaySFX(SoundType.Lose);
        LostManager.Instance.ShowUI();
    }

    public void PlayGame()
    {
        AppManager.Instance.PauseGame(false);
        LoginManager.Instance.HideSDK();
        //isLost = false;
        //LoadingManager.instance.LoadScene("Level " + DataManager.Instance.LevelData.Levels[currentLevel - 1].LevelId.ToString());
        LoadingManager.instance.LoadScene("Play");
    }
}
