using Athena.Common.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : UIController
{
    public System.Action OnPlayBtn;
    public System.Action OnSettingBtn;
    public System.Action OnRankingBtn;

    public Sprite activeImg;
    public Sprite noActiveImg;

    [SerializeField] private Button playBtn;
    [SerializeField] private Button settingBtn;
    [SerializeField] private GameObject levelSelect;
    [SerializeField] private Button rankBtn;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform levelContainer;

    [SerializeField] private List<LevelDetail> levelDetails;

    private void Start()
    {
        playBtn.gameObject.SetActive(false);
        levelSelect.gameObject.SetActive(false);
        rankBtn.gameObject.SetActive(false);
    }

    public void HideObject()
    {
        playBtn.gameObject.SetActive(false);
        levelSelect.gameObject.SetActive(false);
        rankBtn.gameObject.SetActive(false);
    }
    public void OnPlayBtnClick()
    {
        OnPlayBtn?.Invoke();
    }

    public void OnSettingBtnClick()
    {
        OnSettingBtn?.Invoke();
    }

    public void OnRankingBtnClick()
    {
        OnRankingBtn?.Invoke();
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

    public void SetupLevel(int maxLevelActive)
    {
        GameManager.Instance.currentLevel = maxLevelActive;

        for (int i = 0; i < levelDetails.Count; i++)
        {
            levelDetails[i].gameObject.SetActive(true);

            levelDetails[i].levelId = i + 1;

            if (i < maxLevelActive - 1)
            {
                levelDetails[i].select.SetActive(false);
                levelDetails[i].active.sprite = activeImg;
                levelDetails[i].button.interactable = true;
            }
            else if (i == maxLevelActive - 1)
            {
                levelDetails[i].select.SetActive(true);
                levelDetails[i].active.sprite = activeImg;
                levelDetails[i].button.interactable = true;
            }
            else
            {
                levelDetails[i].select.SetActive(false);
                levelDetails[i].active.sprite = noActiveImg;
                levelDetails[i].button.interactable = false;
            }

            levelDetails[i].OnClick = SwapToLevel;
        }

        playBtn.gameObject.SetActive(true);
        levelSelect.gameObject.SetActive(true);
        rankBtn.gameObject.SetActive(true);

        if (maxLevelActive > 2)
        {
            SnapTo(levelDetails[maxLevelActive - 2].GetComponent<RectTransform>());
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

    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        levelContainer.anchoredPosition =
                (Vector2)scrollRect.transform.InverseTransformPoint(levelContainer.position)
                - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    }
}
