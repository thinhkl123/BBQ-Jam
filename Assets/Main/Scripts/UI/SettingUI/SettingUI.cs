using Athena.Common.UI;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UIController
{
    public System.Action OnCloseBtn;
    public System.Action<float> OnMusicSlider;
    public System.Action<float> OnSFXSlider;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button closeBtn;

    public void OnCloseBtnClick()
    {
        OnCloseBtn?.Invoke();
    }

    public void SetSlider(float musicValue, float sfxValue)
    {
        Debug.Log(musicValue + " " + sfxValue);
        musicSlider.value = musicValue;
        sfxSlider.value = sfxValue;
    }

    public void OnMusicSliderChanged()
    {
        OnMusicSlider?.Invoke(musicSlider.value);
    }

    public void OnSFXSliderChanged()
    {
        OnSFXSlider?.Invoke(sfxSlider.value);
    }
}
