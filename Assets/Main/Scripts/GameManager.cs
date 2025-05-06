using Atom;
using SoundManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public int MaxLevel = 10;
    public int currentLevel;
    public bool isLost;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);

        currentLevel = 1;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void WinGame()
    {
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
        Invoke(nameof(ShowUILose), 1f);
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
        isLost = false;
        //LoadingManager.instance.LoadScene("Level " + DataManager.Instance.LevelData.Levels[currentLevel - 1].LevelId.ToString());
        LoadingManager.instance.LoadScene("Play");
    }
}
