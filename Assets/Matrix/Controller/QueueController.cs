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
                CheckOrder();
                
                return;
            }
        }


        Debug.Log("Queue is full");

    }

    private void CheckOrder()
    {
        List<FoodType> foodTypes = new List<FoodType>();
        for (int i = 0; i < queueElements.Count; i++)
        {
            if (queueElements[i].FoodType != FoodType.None)
            {
                foodTypes.Add(queueElements[i].FoodType);
            }
        }
        List<FoodType> foodTypes2 = new List<FoodType>(foodTypes);

        List<FoodType> orders = CustomerManager.Instance.GetCurrentOrder();

        for (int i = 0; i < orders.Count; i++)
        {
            if (foodTypes2.Contains(orders[i]))
            {
                foodTypes2.Remove(orders[i]);
            }
            else
            {
                return;
            }
        }

        Debug.Log("Order is Passed");

        for (int i = 0; i < queueElements.Count; i++)
        {
            if (orders.Contains(queueElements[i].FoodType))
            {
                queueElements[i].SetNull();
            }
        }

        CustomerManager.Instance.CompleteOrder();
    } 
}
