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

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);

        currentLevel = 1;
    }

    private void Start()
    {
        
    }

    public void WinGame()
    {
        Invoke(nameof(ShowUIWin), 1f);
    }

    private void ShowUIWin()
    {
        SoundsManager.Instance.PlaySFX(SoundType.Win);
        AppManager.Instance.PauseGame(false);
        WinManager.Instance.ShowUI();
    }

    public void LoseGame()
    {
        Invoke(nameof(ShowUILose), 1f);
    }

    private void ShowUILose()
    {
        SoundsManager.Instance.PlaySFX(SoundType.Lose);
        AppManager.Instance.PauseGame(false);
        LostManager.Instance.ShowUI();
    }

    public void PlayGame()
    {
        AppManager.Instance.PauseGame(true);
        LoadingManager.instance.LoadScene("Level " + DataManager.Instance.LevelData.Levels[currentLevel - 1].LevelId.ToString());

    }
}
