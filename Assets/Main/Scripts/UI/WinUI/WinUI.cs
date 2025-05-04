using Athena.Common.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : UIController
{
    public System.Action OnHomeBtn;
    public System.Action OnNextBtn;

    public RectTransform stageClearBanner;
    public RectTransform[] stars;
    public RectTransform timerPanel;
    public RectTransform homeButton;
    public RectTransform nextLevelButton;

    [SerializeField] private TextMeshProUGUI resultText;

    private void OnEnable()
    {
        //ShowUI();
    }

    public void SetResultText(float value)
    {
        int minutes = Mathf.FloorToInt(value / 60f);
        int seconds = Mathf.FloorToInt(value % 60f);
        string timeFormatted = string.Format("{0:00}:{1:00}", minutes, seconds);
        this.resultText.text = timeFormatted;
    }

    public void ShowUI()
    {
        // Reset scale & position
        stageClearBanner.localScale = Vector3.zero;
        foreach (var star in stars) star.localScale = Vector3.zero;
        timerPanel.localScale = Vector3.zero;
        homeButton.anchoredPosition = new Vector2(-820, homeButton.anchoredPosition.y);
        nextLevelButton.anchoredPosition = new Vector2(820, nextLevelButton.anchoredPosition.y);

        // Banner scale in
        stageClearBanner.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            // Stars show one by one
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack).SetDelay(i * 0.2f);
            }

            // Timer panel scale in
            timerPanel.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack).SetDelay(0.8f);

            // Buttons slide in
            homeButton.DOAnchorPosX(-237.8f, 0.4f).SetEase(Ease.OutBack).SetDelay(1.2f);
            nextLevelButton.DOAnchorPosX(239.5f, 0.4f).SetEase(Ease.OutBack).SetDelay(1.3f);
        });
    }

    public void OnHomeBtnClick()
    {
        OnHomeBtn?.Invoke();
    }

    public void OnNextBtnClick()
    { 
        OnNextBtn?.Invoke(); 
    }
}
