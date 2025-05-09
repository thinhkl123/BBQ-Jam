using DG.Tweening;
using LevelManager;
using SoundManager;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public List<Vector2Int> poses;
    public bool isCooked;
    public Sprite cookedSprite;
    public Sprite unCookedSprite;
    public GameObject hightLight;
    //public GameObject cookedPrefab;
    public FoodType FoodType;

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        //Debug.Log(MatrixController.Instance.Dir);
        //poses = MatrixController.Instance.Move(MatrixController.Instance.Dir, poses);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (CustomerManager.Instance.isSwitching) return;

        MatrixController.Instance.currentView = this;
        MatrixController.Instance.isPressing = false;

        if (TutorialManager.Instance != null)
        {
            if (!TutorialManager.Instance.isDone)
            {
                if (TutorialManager.Instance.GetCurrentObjectStep() == this.name)
                {
                    ShowHightLight();
                }
            }
            else
            {
                ShowHightLight();
            }
        }
        else
        {
            ShowHightLight();
        }
    }

    public void ShowHightLight()
    {
        hightLight.SetActive(true);
    }

    public void HideHightLight()
    {
        hightLight.SetActive(false);
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
        else
        {
            //Debug.Log("Shake");
            transform.DOShakeRotation(
                duration: 0.5f,                 // Thời gian lắc
                strength: new Vector3(0, 0, 20f), // Chỉ lắc theo trục Z
                vibrato: 10,                  // Số lần rung
                randomness: 90f,              // Độ ngẫu nhiên
                fadeOut: true                 // Giảm dần độ rung về sau
            );
        }
    }

    public void SetInitCook()
    {
        isCooked = true;

        if (cookedSprite != null)
        {
            this.GetComponentInChildren<SpriteRenderer>().sprite = cookedSprite;
            this.hightLight.GetComponent<SpriteRenderer>().sprite = cookedSprite;
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
            SoundsManager.Instance.PlaySFX(SoundType.IceMelt);

            MatrixController.Instance.SetFire(poses);
        });
        sequence.Append(this.transform.DOScale(Vector3.zero, shrinkDuration).SetEase(Ease.InBack)); // thu nhỏ và biến mất
        sequence.AppendCallback(() =>
        {
            this.transform.localScale = Vector3.zero; // đảm bảo đang invisible
            // Có thể tắt gameObject tại đây nếu cần
            if (cookedSprite != null)
            {
                this.GetComponentInChildren<SpriteRenderer>().sprite = cookedSprite;
                this.hightLight.GetComponent<SpriteRenderer>().sprite = cookedSprite;
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

    public void SetUnCook()
    {
        isCooked = false;

        float delay = 0.5f;          // thời gian chờ trước khi bắt đầu
        float shrinkDuration = 0.2f;
        float growDuration = 0.2f;
        Vector3 spawnScale = Vector3.one * 0.4f; // scale khi xuất hiện lại

        Vector3 originalScale = this.transform.localScale;

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(delay); 
        sequence.AppendCallback(() =>
        {
            SoundsManager.Instance.PlaySFX(SoundType.Pop);
        });
        sequence.Append(this.transform.DOScale(Vector3.zero, shrinkDuration).SetEase(Ease.InBack)); // thu nhỏ và biến mất
        sequence.AppendCallback(() =>
        {
            this.transform.localScale = Vector3.zero; // đảm bảo đang invisible
            // Có thể tắt gameObject tại đây nếu cần
            if (cookedSprite != null)
            {
                this.GetComponentInChildren<SpriteRenderer>().sprite = unCookedSprite;
                this.hightLight.GetComponent<SpriteRenderer>().sprite = unCookedSprite;
            }
        });

        // Hiện lại và scale to hơn
        sequence.Append(this.transform.DOScale(spawnScale, growDuration).SetEase(Ease.OutBack));
        sequence.Append(this.transform.DOScale(originalScale, 0.2f)); // trở về kích thước gốc nếu muốn
        sequence.AppendInterval(0.4f);
    }

    public void MoveOut(Vector3 t1, Vector3 t2)
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        if (Vector3.Distance(t1, this.transform.position) >= 0.1f)
        {
            sequence.Append(this.transform.DOMove(new Vector3(t1.x, this.transform.position.y, t1.z), 0.3f).SetEase(Ease.Linear));
        }
        sequence.AppendCallback(() =>
        {
            SoundsManager.Instance.PlaySFX(SoundType.Woosh);
        });
        sequence.Append(this.transform.DOMove(new Vector3(t2.x, this.transform.position.y, t2.z), 0.5f).SetEase(Ease.Linear));
        sequence.AppendCallback(() =>
        {
            Destroy(this.gameObject);
        });
    }
}
