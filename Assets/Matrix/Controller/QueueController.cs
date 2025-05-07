using DG.Tweening;
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

                if (!CustomerManager.Instance.isSwitching)
                {

                    //CheckOrder();
                    if (!CheckOrder())
                    {
                        for (int j = 0; j < queueElements.Count; j++)
                        {
                            if (queueElements[j].FoodType == FoodType.None)
                            {
                                return;
                            }
                        }

                        GameManager.Instance.LoseGame();
                    }

                }

                //Debug.Log("Queue is full");

                return;
            }
        }

        

    }

    public bool CheckOrder()
    {
        List<FoodType> foodTypes2 = new List<FoodType>();
        for (int i = 0; i < queueElements.Count; i++)
        {
            if (queueElements[i].FoodType != FoodType.None)
            {
                foodTypes2.Add(queueElements[i].FoodType);
            }
        }

        //foreach (var foodType in foodTypes2) Debug.Log(foodType.ToString()); Debug.Log("End");

        List<FoodType> orders = new List<FoodType>(CustomerManager.Instance.GetCurrentOrder());

        for (int i = 0; i < orders.Count; i++)
        {
            if (foodTypes2.Contains(orders[i]))
            {
                foodTypes2.Remove(orders[i]);
            }
            else
            {
                return false;
            }
        }

        Debug.Log("Order is Passed");

        for (int i = 0; i < queueElements.Count; i++)
        {
            if (orders.Contains(queueElements[i].FoodType))
            {
                orders.Remove(queueElements[i].FoodType);
                queueElements[i].SetNull();
            }
        }

        CustomerManager.Instance.CompleteOrder();

        return true;
    } 

    public Vector3 GetAvailablePos()
    {
        for (int i = 0; i < queueElements.Count; i++)
        {
            if (queueElements[i].FoodType == FoodType.None)
            {
                return queueElements[i].transform.position;
            }
        }

        return Vector3.zero;
    }
}
