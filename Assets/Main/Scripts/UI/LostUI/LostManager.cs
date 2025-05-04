using Athena.Common.UI;
using Atom;
using SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostManager : MonoSingleton<LostManager>
{
    protected LostUI _lostUI;

    public LostUI LostUI { get { return _lostUI; } }

    public void Setup()
    {
        _lostUI = AppManager.Instance.ShowSafeTopUI<LostUI>("UI/LostUI");

        _lostUI.OnHomeBtn = BackHome;
        _lostUI.OnRetryBtn = RetryLevel;

        HideUI();
    }

    private void RetryLevel()
    {
        //LoadingManager.instance.LoadScene("Main");
    }

    private void BackHome()
    {
        //LoadingManager.instance.LoadScene("Home");
    }

    public void ShowResult()
    {
        Invoke(nameof(ShowUI), 0.5f);
    }

    public void ShowUI()
    {
        //SoundsManager.Instance.PlaySFX(SoundType.Lose);
        UIManager.Instance.ShowUI(_lostUI, true);
    }

    public void HideUI()
    {
        _lostUI.gameObject.SetActive(false);
    }
}
