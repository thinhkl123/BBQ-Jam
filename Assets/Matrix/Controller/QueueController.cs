using LevelManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueController : MonoSingleton<QueueController>
{
    public List<QueueElementView> queueElements = new List<QueueElementView>();
    public int queueElemntCount = 3;

    protected override void Awake()
    {
        Init();
    }

    public void Init()
    {
        for (int i = 0; i < queueElements.Count; i++)
        {
            queueElements[i].Init();
        }
    }

    public void AddQueue(FoodType foodType)
    {
        for (int i = 0; i < queueElements.Count; i++)
        {
            if (queueElements[i].FoodType == FoodType.None)
            {
                queueElements[i].SetInfor(foodType);
                return;
            }
        }
        Debug.Log("Queue is full");

    }
}
