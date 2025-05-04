using Athena.Common.UI;
using Atom;
using UnityEngine;

public class PauseManager : MonoSingleton<PauseManager>
{
    protected PauseUI _pauseUI;

    public PauseUI PauseUI { get { return _pauseUI; } }

    public void Setup()
    {
        _pauseUI = AppManager.Instance.ShowSafeTopUI<PauseUI>("UI/PauseUI", false);

        _pauseUI.OnHomeBtn = BackHome;
        _pauseUI.OnContinueBtn = ContinueGame;
        _pauseUI.OnSettingBtn = SettingManager.Instance.ShowUI;

        HideUI();
    }

    private void ContinueGame()
    {
        HideUI();
        AppManager.Instance.PauseGame(false);
    }

    private void BackHome()
    {
        HideUI();
        LoadingManager.instance.LoadScene("Home");
    }

    public void ShowUI()
    {
        UIManager.Instance.ShowUI(_pauseUI, true);
    }

    public void HideUI()
    {
        _pauseUI.gameObject.SetActive(false);
    }
}
