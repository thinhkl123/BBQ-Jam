using Athena.Common.UI;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : UIController
{
    public System.Action OnContinueBtn;
    public System.Action OnHomeBtn;
    public System.Action OnSettingBtn;
    public System.Action OnReStartBtn;

    [SerializeField] private Button continueBtn;
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button homeBtn;

    public void OnContinueBtnClick()
    {
        OnContinueBtn?.Invoke();
    }

    public void OnHomeBtnClick()
    {
        OnHomeBtn.Invoke();
    }

    public void OnSettingBtnClick()
    {
        OnSettingBtn?.Invoke();
    }

    public void OnRestartBtnClick()
    {
        OnReStartBtn?.Invoke();
    }
}
