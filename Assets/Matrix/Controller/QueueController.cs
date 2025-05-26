using DG.Tweening;
using LevelManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueController : MonoBehaviour
{
    public static QueueController Instance;

    public List<QueueElementView> queueElements = new List<QueueElementView>();
    public int queueElemntCount = 3;
    public bool isCheckingQueue = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Debug.Log("Init");
        Init();
        isCheckingQueue = false;
    }

    private void Update()
    {
        CheckQueue();
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
                queueElements[i].SetInfor(foodType, MatrixController.Instance.TimeWait);

                //if (!CustomerManager.Instance.isSwitching)
                //{

                //    CheckOrder();
                //    if (!CheckOrder())
                //    {
                //        for (int j = 0; j < queueElements.Count; j++)
                //        {
                //            if (queueElements[j].FoodType == FoodType.None)
                //            {
                //                return;
                //            }
                //        }

                //        GameManager.Instance.LoseGame();
                //    }

                //}

                //Debug.Log("Queue is full");

                return;
            }
        }
    }

    public void CheckQueue()
    {
        if (!CustomerManager.Instance.isSwitching && !GameManager.Instance.isLost && !isCheckingQueue)
        {
            //Debug.Log(isCheckingQueue);

            //Debug.Log("Check");
            if (!CheckOrder())
            {
                for (int j = 0; j < queueElements.Count; j++)
                {
                    if (queueElements[j].FoodType == FoodType.None || 
                        (queueElements[j].FoodType != FoodType.None) && (queueElements[j].isSpawning))
                    {
                        return;
                    }
                }
                Debug.Log("Full Queue");
                isCheckingQueue = true;

                GameManager.Instance.LoseGame();
            }

        }
        isCheckingQueue = false;
    }

    public bool CheckOrder()
    {
        isCheckingQueue = true;

        List<FoodType> foodTypes2 = new List<FoodType>();
        for (int i = 0; i < queueElements.Count; i++)
        {
            if (queueElements[i].FoodType != FoodType.None && !queueElements[i].isSpawning)
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

    public bool IsAvailablePos()
    {
        for (int i = 0; i < queueElements.Count; i++)
        {
            if (queueElements[i].FoodType == FoodType.None)
            {
                return true;
            }
        }

        return false;
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
