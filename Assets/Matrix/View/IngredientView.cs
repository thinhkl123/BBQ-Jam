using DG.Tweening;
using LevelManager;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public List<Vector2Int> poses;
    public bool isCooked;
    public Sprite sprite;
    public GameObject cookedPrefab;
    public FoodType FoodType;

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        //Debug.Log(MatrixController.Instance.Dir);
        //poses = MatrixController.Instance.Move(MatrixController.Instance.Dir, poses);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        MatrixController.Instance.currentView = this;
    }

    public void SetPoses(List<Vector2Int> poses)
    {
        if (this.poses != poses)
        {
            this.poses = poses;

            Vector3 newPos = MatrixController.Instance.GetPosition(poses);
            //Debug.Log(newPos);

            //this.transform.position = new Vector3(newPos.x, this.transform.position.y, newPos.z);
            this.transform.DOMove(new Vector3(newPos.x, this.transform.position.y, newPos.z), 0.5f).SetEase(Ease.OutBounce);
        }
    }

    public void SetCook()
    {
        isCooked = true;

        float delay = 0.5f;          // thời gian chờ trước khi bắt đầu
        float shrinkDuration = 0.2f;
        float growDuration = 0.2f;
        Vector3 spawnScale = Vector3.one * 0.4f; // scale khi xuất hiện lại

        Vector3 originalScale = this.transform.localScale;

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(delay); // chờ 1s
        sequence.AppendCallback(() =>
        {
            MatrixController.Instance.SetFire(poses);
        });
        sequence.Append(this.transform.DOScale(Vector3.zero, shrinkDuration).SetEase(Ease.InBack)); // thu nhỏ và biến mất
        sequence.AppendCallback(() =>
        {
            this.transform.localScale = Vector3.zero; // đảm bảo đang invisible
            // Có thể tắt gameObject tại đây nếu cần
            if (sprite != null)
            {
                this.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
            }
        });

        // Hiện lại và scale to hơn
        sequence.Append(this.transform.DOScale(spawnScale, growDuration).SetEase(Ease.OutBack));
        sequence.Append(this.transform.DOScale(originalScale, 0.2f)); // trở về kích thước gốc nếu muốn
        sequence.AppendInterval(0.4f); 
        sequence.AppendCallback(() =>
        {
            MatrixController.Instance.UnFire(poses);
        });
    }

    public void MoveOut(Vector3 t1, Vector3 t2)
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(this.transform.DOMove(new Vector3(t1.x, this.transform.position.y, t1.z), 0.5f).SetEase(Ease.OutBack));
        sequence.Append(this.transform.DOMove(new Vector3(t2.x, this.transform.position.y, t2.z), 0.5f).SetEase(Ease.Linear));
        sequence.AppendCallback(() =>
        {
            Destroy(this.gameObject);
        });
    }
}
