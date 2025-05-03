using Athena.Common.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostUI : UIController
{
    public System.Action OnHomeBtn;
    public System.Action OnRetryBtn;

    public RectTransform lostText;
    public RectTransform homeButton;
    public RectTransform retryButton;

    private void OnEnable()
    {
        //ShowUI();
    }

    public void ShowUI()
    {
        // Reset scale & position
        lostText.localScale = Vector3.zero;
        homeButton.anchoredPosition = new Vector2(-820, homeButton.anchoredPosition.y);
        retryButton.anchoredPosition = new Vector2(820, retryButton.anchoredPosition.y);

        // Banner scale in
        lostText.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            // Buttons slide in
            homeButton.DOAnchorPosX(-237.8f, 0.4f).SetEase(Ease.OutBack).SetDelay(1.2f);
            retryButton.DOAnchorPosX(239.5f, 0.4f).SetEase(Ease.OutBack).SetDelay(1.3f);
        });
    }

    public void OnHomeBtnClick()
    {
        OnHomeBtn?.Invoke();
    }

    public void OnRetryBtnClick()
    {
        OnRetryBtn?.Invoke();
    }
}
