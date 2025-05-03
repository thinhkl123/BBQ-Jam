using LevelManager;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public List<Vector2Int> poses;
    public bool isCooked;
    public Sprite sprite;
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
            Debug.Log(newPos);

            this.transform.position = new Vector3(newPos.x, this.transform.position.y, newPos.z);
        }
    }

    public void SetCook()
    {
        isCooked = true;
        this.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
    }
}
