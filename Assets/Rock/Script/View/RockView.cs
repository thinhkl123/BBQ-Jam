using DG.Tweening;
using FoodLevelData;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RockView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public List<Vector2Int> poses;
    public GameObject hightLight;
    private Vector3 firstPosition;

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        //ReturnFirstPosition();
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {

        firstPosition = this.transform.position;
        Touch touch = Input.GetTouch(0);
        MatrixController.Instance.firstPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0f));
        MatrixController.Instance.currentRockView = this;
        MatrixController.Instance.isPressing = false;
        ShowHightLight();
    }

    public void SetPosesAndMove(List<Vector2Int> poses)
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();

        //sequence.AppendInterval(time);

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

    public void ReturnFirstPosition()
    {
        this.transform.position = firstPosition;
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
