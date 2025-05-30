using DG.Tweening;
using FoodLevelData;
using LevelManager;
using SoundManager;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class IngredientView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public List<Vector2Int> poses;
    public List<Direction> directions;
    public bool isCooked;
    public Sprite cookedSprite;
    public Sprite unCookedSprite;
    public GameObject hightLight;
    public IngredientView convertObj;
    //public GameObject cookedPrefab;
    public FoodType FoodType;
    public bool isAnim = false;

    private Direction curDir = Direction.None;
    private Vector3 firstPosition;

    private Vector3 tarScale;

    private void Awake()
    {
        tarScale = this.transform.localScale;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        //Debug.Log(MatrixController.Instance.Dir);
        //poses = MatrixController.Instance.Move(MatrixController.Instance.Dir, poses);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Debug.Log(isAnim);

        MatrixController.Instance.TouchTime = 0f;

        if (/*CustomerManager.Instance.isSwitching ||*/ isAnim) return;

        //Debug.Log(DOTween.TotalPlayingTweens());

        if (DOTween.TotalPlayingTweens() != 0) return;

        firstPosition = this.transform.position;
        Touch touch = Input.GetTouch(0); 
        MatrixController.Instance.firstPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0f));

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

    public void Hide(float time = 0f)
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(time);
        sequence.Append(this.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack));
        sequence.AppendCallback(() =>
        {
            Destroy(this.gameObject);
        });
    }

    public void Spawn(float time = 0f)
    {
        isAnim = true;
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(time);
        sequence.Append(this.transform.DOScale(tarScale, 0.2f).SetEase(Ease.OutBack));
        sequence.AppendCallback(() =>
        {
            isAnim = false;
        });
    }

    public void ReturnFirstPosition()
    {
        //Debug.Log(firstPosition);
        this.transform.position = firstPosition;
        //Debug.Log(this.transform.position);
    }

    public void SetPoses(List<Vector2Int> poses)
    {
        this.poses = poses;

        Vector3 newPos = MatrixController.Instance.GetPosition(poses);

        this.transform.DOMove(new Vector3(newPos.x, this.transform.position.y, newPos.z), 0.1f).SetEase(Ease.Linear);
    }

    public void SetPosesAndMove(List<Vector2Int> poses)
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
            ReturnFirstPosition();

            transform.DOShakeRotation(
                duration: 0.5f,                 // Thời gian lắc
                strength: new Vector3(0, 0, 20f), // Chỉ lắc theo trục Z
                vibrato: 10,                  // Số lần rung
                randomness: 90f,              // Độ ngẫu nhiên
                fadeOut: true                 // Giảm dần độ rung về sau
            );
        }
    }

    public void SetPosesAndMove(List<Vector2Int> poses, float time)
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(time);

        if (this.poses != poses)
        {
            this.poses = poses;

            Vector3 newPos = MatrixController.Instance.GetPosition(poses);
            //Debug.Log(newPos);

            //this.transform.position = new Vector3(newPos.x, this.transform.position.y, newPos.z);
            sequence.Append(
                this.transform.DOMove(new Vector3(newPos.x, this.transform.position.y, newPos.z), 0.5f).SetEase(Ease.InQuint)
            );
        }
        else
        {
            sequence.AppendCallback(() =>
            {
                ReturnFirstPosition();
            });

            sequence.Append(
                transform.DOShakeRotation(
                    duration: 0.5f,                 // Thời gian lắc
                    strength: new Vector3(0, 0, 20f), // Chỉ lắc theo trục Z
                    vibrato: 10,                  // Số lần rung
                    randomness: 90f,              // Độ ngẫu nhiên
                    fadeOut: true                 // Giảm dần độ rung về sau
                )
            );
        }
    }

    public void ShakeWrongChoice()
    {
        Vector3 originalPos = this.transform.localPosition;
        DG.Tweening.Sequence seq = DOTween.Sequence();

        float strength = 0.2f;
        float duration = 0.05f;

        if (directions.Contains(Direction.Right))
        {
            seq.Append(this.transform.DOLocalMoveX(originalPos.x - strength, duration));
            seq.Append(this.transform.DOLocalMoveX(originalPos.x + strength, duration));
            seq.Append(this.transform.DOLocalMoveX(originalPos.x - strength * 0.6f, duration));
            seq.Append(this.transform.DOLocalMoveX(originalPos.x + strength * 0.6f, duration));
            seq.Append(this.transform.DOLocalMoveX(originalPos.x, duration));
            seq.SetEase(Ease.OutQuad);
        }
        else
        {
            seq.Append(this.transform.DOLocalMoveZ(originalPos.z + strength, duration));
            seq.Append(this.transform.DOLocalMoveZ(originalPos.z - strength, duration));
            seq.Append(this.transform.DOLocalMoveZ(originalPos.z + strength * 0.6f, duration));
            seq.Append(this.transform.DOLocalMoveZ(originalPos.z - strength * 0.6f, duration));
            seq.Append(this.transform.DOLocalMoveZ(originalPos.z, duration));
            seq.SetEase(Ease.OutQuad);
        }
    }


    public void Shake(float time = 0f)
    {
        isAnim = true;

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(time);

        sequence.Append(
            transform.DOShakeRotation(
                duration: 0.5f,                 // Thời gian lắc
                strength: new Vector3(0, 0, 20f), // Chỉ lắc theo trục Z
                vibrato: 10,                  // Số lần rung
                randomness: 90f,              // Độ ngẫu nhiên
                fadeOut: true                 // Giảm dần độ rung về sau
            )
        );

        sequence.AppendCallback(() =>
        {
            isAnim = false;
        });
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

    public void SetCook(float time = 0f)
    {
        isAnim = true;

        isCooked = true;

        float delay = 0.5f;          // thời gian chờ trước khi bắt đầu
        float shrinkDuration = 0.2f;
        float growDuration = 0.2f;
        Vector3 spawnScale = Vector3.one * 0.4f; // scale khi xuất hiện lại

        Vector3 originalScale = this.transform.localScale;

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(delay + time);
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
        sequence.Join(this.transform.DOScale(tarScale * 1.5f, 0.3f).SetEase(Ease.OutQuint));
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
        sequence.Join(this.transform.DOScale(tarScale, 0.3f).SetEase(Ease.InExpo));
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

    public void SetUnCook(float time = 0f)
    {
        isAnim = true;

        isCooked = false;

        float delay = 0.5f;          // thời gian chờ trước khi bắt đầu
        float shrinkDuration = 0.2f;
        float growDuration = 0.2f;
        Vector3 spawnScale = Vector3.one * 0.4f; // scale khi xuất hiện lại

        //Vector3 originalScale = this.transform.localScale;

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(delay + time); 
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
        sequence.Append(this.transform.DOScale(tarScale, 0.2f)); // trở về kích thước gốc nếu muốn
        sequence.AppendCallback(() =>
        {
            isAnim = false;
        });
    }

    public void MoveOut(Vector3 t1, Vector3 t2, float time = 0f)
    {
        isAnim = true;

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(time);

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

    public void Nudge(Vector3 fpos, Vector3 scPos)
    {
        //Debug.Log("Nudge");

        Vector3 offset = Vector3.zero;
        Direction dir;

        float x = scPos.x - fpos.x;
        float y = scPos.z - fpos.z;

        if (Mathf.Abs(x) <= 0.05f && Mathf.Abs(y) <= 0.05f)
        {
            dir = Direction.None;
        }
        else if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            dir = x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            dir = y > 0 ? Direction.Up : Direction.Down;
        }

        if (dir == Direction.None || !MatrixController.Instance.ingredientGrid[poses[0].x, poses[0].y].directions.Contains(dir)) 
        {
            return;
        }

        x = Mathf.Min(Mathf.Abs(x), 0.35f);
        y = Mathf.Min(Mathf.Abs(y), 0.35f);

        switch (dir)
        {
            case Direction.Left:
                offset = new Vector3(-1, 0, 0);
                break;
            case Direction.Right:
                offset = new Vector3(1, 0, 0);
                break;
            case Direction.Up:
                offset = new Vector3(0, 0, 1);
                break;
            case Direction.Down:
                offset = new Vector3(0, 0, -1);
                break;
        }

        offset = new Vector3(offset.x * Mathf.Abs(x), 0f, offset.z * Mathf.Abs(y));

        this.transform.position = firstPosition + offset;
    }
}
