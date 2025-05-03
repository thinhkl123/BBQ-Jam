using Athena.Common.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : UIController
{
    public System.Action OnPlayBtn;
    public System.Action OnSettingBtn;

    [SerializeField] private Button playBtn;
    [SerializeField] private Button settingBtn;

    [SerializeField] private List<LevelDetail> levelDetails;

    public void OnPlayBtnClick()
    {
        OnPlayBtn?.Invoke();
    }

    public void OnSettingBtnClick()
    {
        Debug.Log("Click");
        OnSettingBtn?.Invoke();
    }

    public void SetupLevel()
    {
        levelDetails[0].levelId = 1;
        levelDetails[0].select.SetActive(true);
        levelDetails[0].OnClick = SwapToLevel;

        for (int i = 1; i < levelDetails.Count; i++)
        {
            levelDetails[i].levelId = i + 1;
            levelDetails[i].select.SetActive(false);
            levelDetails[i].OnClick = SwapToLevel;
        }
    }

    public void SwapToLevel(int level)
    {
        SwapLevel(GameManager.Instance.currentLevel, level);
    }

    public void SwapLevel(int from, int to)
    {
        levelDetails[from-1].select.SetActive(false);
        levelDetails[to - 1].select.SetActive(true);

        GameManager.Instance.currentLevel = to;
    }
}
