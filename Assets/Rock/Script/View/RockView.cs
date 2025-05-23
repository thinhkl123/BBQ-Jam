using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RockView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public List<Vector2Int> poses;

    public void OnPointerUp(PointerEventData pointerEventData)
    {
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        MatrixController.Instance.currentRockView = this;
        MatrixController.Instance.isPressing = false;
    }

    public void SetPosesAndMove(List<Vector2Int> poses)
    {
        if (this.poses != poses)
        {
            this.poses = poses;

            Vector3 newPos = MatrixController.Instance.GetPosition(poses);
            this.transform.DOMove(new Vector3(newPos.x, this.transform.position.y, newPos.z), 0.5f).SetEase(Ease.InQuint);
        }
    }
}
