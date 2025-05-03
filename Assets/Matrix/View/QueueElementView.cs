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
        this.FoodType = foodType;
        this.SpriteRenderer.enabled = true;
        this.SpriteRenderer.sprite = DataManager.Instance.FoodData.GetIcon(foodType);
    }

}
