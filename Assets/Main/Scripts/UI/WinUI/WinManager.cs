using Athena.Common.UI;
using Atom;
using SoundManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinManager : MonoSingleton<WinManager>
{
    protected WinUI _winUI;

    public WinUI WinUI {  get { return _winUI; } }

    public void Setup()
    {
        _winUI = AppManager.Instance.ShowSafeTopUI<WinUI>("UI/WinUI");

        _winUI.OnHomeBtn = BackHome;
        _winUI.OnNextBtn = NextLevel;

        HideUI();
    }

    private void NextLevel()
    {
        HideUI();
        if (GameManager.Instance.currentLevel+ 1 <= GameManager.Instance.MaxLevel)
        {
            GameManager.Instance.currentLevel++;
            GameManager.Instance.PlayGame();
        }
    }

    private void BackHome()
    {
        HideUI();
        LoadingManager.instance.LoadScene("Home");
    }



    public void ShowResult(float value)
    {
        _winUI.SetResultText(value);
        Invoke(nameof(ShowUI), 1f);
    }

    public void ShowUI()
    {
        //SoundsManager.Instance.PlaySFX(SoundType.Win);
        UIManager.Instance.ShowUI(_winUI, true);
    }

    public void HideUI()
    {
        _winUI.gameObject.SetActive(false);
    }
}
