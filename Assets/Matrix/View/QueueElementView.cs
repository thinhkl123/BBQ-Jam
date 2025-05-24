using DG.Tweening;
using LevelManager;
using SoundManager;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QueueElementView : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    public FoodType FoodType;

    public bool isSpawning = false;
    private bool isDisapearing = false;

    public void Init()
    {
        this.FoodType = FoodType.None;
        this.SpriteRenderer.enabled = false;

        this.isSpawning = false;
        this.isDisapearing = false;
    }

    public void SetInfor(FoodType foodType, float time = 0f)
    {
        isSpawning = true;  

        Vector3 initScale = DataManager.Instance.FoodData.GetScale(foodType);
        //Debug.Log(foodType.ToString() + " " + this.name);

        SpriteRenderer.transform.localScale = Vector3.zero;

        this.FoodType = foodType;
        this.SpriteRenderer.enabled = true;
        this.SpriteRenderer.sprite = DataManager.Instance.FoodData.GetIcon(foodType);
        //this.SpriteRenderer.transform.localScale = DataManager.Instance.FoodData.GetScale(FoodType);

        //StartCoroutine(SetInforCoroutine());

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(0.8f + time); 
        sequence.AppendCallback(() =>
        {
            SoundsManager.Instance.PlaySFX(SoundType.Pop);
        });
        sequence.Append(SpriteRenderer.transform.DOScale(initScale, 0.2f)); // trở về kích thước gốc nếu muốn
        sequence.AppendCallback(() =>
        {
            isSpawning = false;
        });
    }



    IEnumerator SetInforCoroutine()
    {
        float startTime = Time.time;

        // Chờ đến khi biến bool trở thành true
        yield return new WaitUntil(() => !isDisapearing);

        float elapsed = Time.time - startTime;

        // Nếu thời gian chờ nhỏ hơn 1s, chờ tiếp cho đủ 1s
        if (elapsed < 1f)
        {
            yield return new WaitForSeconds(1f - elapsed);
        }

        isSpawning = true;

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            SoundsManager.Instance.PlaySFX(SoundType.Pop);
        });
        sequence.Append(SpriteRenderer.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f)); // trở về kích thước gốc nếu muốn
        sequence.AppendCallback(() =>
        {
            isSpawning = false;
        });
    }

    public void SetNull(float time = 0f)
    {
        //Debug.Log("Null");

        //this.FoodType = FoodType.None;

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        isDisapearing = true;

        sequence.AppendInterval(0f + time); 
        sequence.Append(SpriteRenderer.transform.DOScale(Vector3.zero, 0.2f)); 
        sequence.AppendCallback(() =>
        {
            this.SpriteRenderer.enabled = false;

            this.FoodType = FoodType.None;

            isDisapearing = false;
        });
    }

    //public void SetNull()
    //{
    //    Debug.Log("Null " + this.name);

    //    this.FoodType = FoodType.None;

    //    StartCoroutine(SetNullCoroutine());
    //}

    IEnumerator SetNullCoroutine()
    {
        float startTime = Time.time;

        // Chờ đến khi biến bool trở thành true
        yield return new WaitUntil(() => !isSpawning);

        float elapsed = Time.time - startTime;

        // Nếu thời gian chờ nhỏ hơn 1s, chờ tiếp cho đủ 1s
        if (elapsed < 1.2f)
        {
            yield return new WaitForSeconds(1.2f - elapsed);
        }

        this.isDisapearing = true;

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.Append(SpriteRenderer.transform.DOScale(Vector3.zero, 0.2f));
        sequence.AppendCallback(() =>
        {
            this.SpriteRenderer.enabled = false;

            this.isDisapearing = false;
        });
    }
}
