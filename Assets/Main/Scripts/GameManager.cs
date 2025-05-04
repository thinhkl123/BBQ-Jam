using Atom;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
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
        WinManager.Instance.ShowUI();
    }

    public void LoseGame()
    {
        Invoke(nameof(ShowUILose), 1f);
    }

    private void ShowUILose()
    {
        LostManager.Instance.ShowUI();
    }
}
