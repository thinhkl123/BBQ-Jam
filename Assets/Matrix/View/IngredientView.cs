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
    public bool isAnim = false;

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        //Debug.Log(MatrixController.Instance.Dir);
        //poses = MatrixController.Instance.Move(MatrixController.Instance.Dir, poses);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Debug.Log(isAnim);

        if (/*CustomerManager.Instance.isSwitching ||*/ isAnim) return;

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
            this.transform.DOMove(new Vector3(newPos.x, this.transform.position.y, newPos.z), 0.5f).SetEase(Ease.InQuint);
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

    public void Shake()
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
        isAnim = true;

        isCooked = true;

        float delay = 0.5f;          // thời gian chờ trước khi bắt đầu
        float shrinkDuration = 0.2f;
        float growDuration = 0.2f;
        Vector3 spawnScale = Vector3.one * 0.4f; // scale khi xuất hiện lại

        Vector3 originalScale = this.transform.localScale;

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        //sequence.AppendInterval(delay); // chờ 1s
        //sequence.AppendCallback(() =>
        //{
        //    SoundsManager.Instance.PlaySFX(SoundType.Cooked);

        //    MatrixController.Instance.SetFire(poses);
        //});
        //sequence.Append(this.transform.DOScale(Vector3.zero, shrinkDuration).SetEase(Ease.InBack)); // thu nhỏ và biến mất
        //sequence.AppendCallback(() =>
        //{
        //    this.transform.localScale = Vector3.zero; // đảm bảo đang invisible
        //    // Có thể tắt gameObject tại đây nếu cần
        //    if (cookedSprite != null)
        //    {
        //        this.GetComponentInChildren<SpriteRenderer>().sprite = cookedSprite;
        //        this.hightLight.GetComponent<SpriteRenderer>().sprite = cookedSprite;
        //    }
        //});

        //// Hiện lại và scale to hơn
        //sequence.Append(this.transform.DOScale(spawnScale, growDuration).SetEase(Ease.OutBack));
        //sequence.Append(this.transform.DOScale(originalScale, 0.2f)); // trở về kích thước gốc nếu muốn
        //sequence.AppendInterval(0.4f);
        //sequence.AppendCallback(() =>
        //{
        //    MatrixController.Instance.UnFire(poses);
        //});
        //sequence.AppendCallback(() =>
        //{
        //    isAnim = false;
        //});

        //transform.DORotate(new Vector3(0, 90f, 0), 0.15f, RotateMode.Fast)
        //         .SetEase(Ease.InOutSine)
        //         .OnComplete(() =>
        //         {
        //             // Giai đoạn 2: đổi sprite khi ở nửa vòng
        //             spriteRenderer.sprite = cookedSprite;

        //             // Giai đoạn 3: hoàn tất xoay còn lại
        //             transform.DORotate(new Vector3(0, 180f, 0), 0.15f, RotateMode.Fast)
        //                      .SetEase(Ease.InOutSine);
        //         });

        sequence.AppendInterval(delay);
        sequence.AppendCallback(() =>
        {
            SoundsManager.Instance.PlaySFX(SoundType.Cooked);

            MatrixController.Instance.SetFire(poses);
        });

        //Debug.Log(new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y + 90f, this.transform.localEulerAngles.z));
        sequence.Append(this.transform.DORotate(new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y + 90f, this.transform.localEulerAngles.z), 0.3f, RotateMode.Fast)
                 .SetEase(Ease.InOutSine));
        if (this.name.Contains("Hon"))
        {
            sequence.Join(this.transform.DOMoveZ(this.transform.position.z + 1f, 0.3f).SetEase(Ease.OutQuint));
        }
        else
        {
            sequence.Join(this.transform.DOMoveX(this.transform.position.x + 0.5f, 0.3f).SetEase(Ease.OutQuint));
        }
        sequence.Join(this.transform.DOScale(this.transform.localScale * 1.5f, 0.3f).SetEase(Ease.OutQuint));
        sequence.AppendCallback(() =>
        {
            if (cookedSprite != null)
            {
                this.GetComponentInChildren<SpriteRenderer>().sprite = cookedSprite;
                this.hightLight.GetComponent<SpriteRenderer>().sprite = cookedSprite;
            }
        });

        //sequence.Append(transform.DORotate(new Vector3(90f, 180f, 0), 0.3f, RotateMode.Fast).SetEase(Ease.InOutSine));
        sequence.Append(transform.DORotate(new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y + 180f, this.transform.localEulerAngles.z), 0.3f, RotateMode.Fast)
                              .SetEase(Ease.InOutSine));
        if (this.name.Contains("Hon"))
        {
            sequence.Join(this.transform.DOMoveZ(this.transform.position.z , 0.3f).SetEase(Ease.InExpo));
        }
        else
        {
            sequence.Join(this.transform.DOMoveX(this.transform.position.x, 0.3f).SetEase(Ease.InExpo));
        }
        sequence.Join(this.transform.DOScale(this.transform.localScale, 0.3f).SetEase(Ease.InExpo));
        sequence.AppendInterval(0.4f);
        sequence.AppendCallback(() =>
        {
            MatrixController.Instance.UnFire(poses);
        });
        sequence.AppendCallback(() =>
        {
            isAnim = false;
        });
    }

    public void SetUnCook()
    {
        isAnim = true;

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
            SoundsManager.Instance.PlaySFX(SoundType.UnCooked);
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
        sequence.AppendCallback(() =>
        {
            isAnim = false;
        });
    }

    public void MoveOut(Vector3 t1, Vector3 t2)
    {
        isAnim = true;

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
