using DG.Tweening;
using LevelManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueElementView : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public FoodType FoodType;

    public void Init()
    {
        this.FoodType = FoodType.None;
        this.SpriteRenderer.enabled = false;
    }

    public void SetInfor(FoodType foodType)
    {
        SpriteRenderer.transform.localScale = Vector3.zero;

        this.FoodType = foodType;
        this.SpriteRenderer.enabled = true;
        this.SpriteRenderer.sprite = DataManager.Instance.FoodData.GetIcon(foodType);

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(1f); // chờ 1s
        sequence.Append(SpriteRenderer.transform.DOScale(Vector3.one, 0.2f)); // trở về kích thước gốc nếu muốn
    }

    public void SetNull()
    {
        this.FoodType = FoodType.None;

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(1.2f); // chờ 1s
        sequence.Append(SpriteRenderer.transform.DOScale(Vector3.zero, 0.2f)); // trở về kích thước gốc nếu muốn
        sequence.AppendCallback(() =>
        {
            this.SpriteRenderer.enabled = false;
        });
    }
}
