using Athena.Common.UI;
using Atom;
using SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoSingleton<SettingManager>
{
    protected SettingUI _settingUI;

    public SettingUI SettingUI { get { return _settingUI; } }


    public void Setup()
    {
        _settingUI = AppManager.Instance.ShowSafeTopUI<SettingUI>("UI/SettingUI");

        _settingUI.OnCloseBtn = HideUI;
        _settingUI.SetSlider(SoundsManager.Instance.GetMusicVolume(), SoundsManager.Instance.GetSFXVolume());
        _settingUI.OnMusicSlider = ChangeMusicVolume;
        _settingUI.OnSFXSlider = ChangeSFXVolume;

        HideUI();
    }

    public void ChangeMusicVolume(float value)
    {
        SoundsManager.Instance.SetMusicVolume(value);
    }

    public void ChangeSFXVolume(float value)
    {
        SoundsManager.Instance.SetSFXVolume(value);
    }

    public void ShowUI()
    {
        UIManager.Instance.ShowUI(_settingUI, true);
    }

    public void HideUI()
    {
        _settingUI.gameObject.SetActive(false);
    }
}
