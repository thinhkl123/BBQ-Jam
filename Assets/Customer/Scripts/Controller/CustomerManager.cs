using DG.Tweening;
using LevelManager;
using SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoSingleton<CustomerManager>
{
    //public Customer CustomerPrefab;
    public Transform spawnTf;
    public Transform orderTf;
    public Transform outTf;

    public Customer customerNext = null;
    public Customer currentCus = null;
    public Order orderNext = null;
    private int currentId = 0;

    public bool isSwitching = false;

    private void Start()
    {
        isSwitching = true;
        currentId = -1;
        //Invoke(nameof(SpawnCustomer), 4f);
        SpawnCustomer();
    }

    private void SpawnCustomer()
    {
        currentId++;
        if (currentId == 0)
        {
            SpawnCustomer(DataManager.Instance.LevelData.Levels[GameManager.Instance.currentLevel-1].Orders[currentId], true);
        }
        else if (currentId < DataManager.Instance.LevelData.Levels[GameManager.Instance.currentLevel - 1].Orders.Count)
        {
            SpawnCustomer(DataManager.Instance.LevelData.Levels[GameManager.Instance.currentLevel - 1].Orders[currentId]);
        }
    }

    private void SpawnCustomer(Order order, bool isFirst = false)
    {
        //Customer customerGO = Instantiate(CustomerPrefab);
        Customer customerGO = Instantiate(DataManager.Instance.CustomerData.GetPrefab(order.Customer));
        customerGO.MaxTime = order.Time;
        customerGO.SetTransformAtFirst(spawnTf.position, new Vector3 (0, 90f, 0));

        List<Sprite> sprites = new List<Sprite>();
        for (int i = 0; i < order.Foods.Count; i++)
        {
            sprites.Add(DataManager.Instance.FoodData.GetIcon(order.Foods[i]));
        }

        customerGO.UpdateIcon(sprites);

        if (isFirst)
        {
            currentCus = customerGO;
            StartCoroutine(StartOrderCoroutine(customerGO, order));
        } 
        else
        {
            customerNext = customerGO;
            orderNext = order;
        }
    }


    private IEnumerator StartOrderCoroutine(Customer customerGO, Order order)
    {
        yield return new WaitForSeconds(1.5f);

        customerGO.currentTime = order.Time;
        customerGO.isOrdering = true;
        customerGO.MoveTo(orderTf.position);
    }

    private void StartOrder(Customer customerGO, Order order)
    {
        customerGO.MoveTo(orderTf.position);

        List<Sprite> sprites = new List<Sprite>();
        for (int i = 0; i < order.Foods.Count; i++)
        {
            sprites.Add(DataManager.Instance.FoodData.GetIcon(order.Foods[i]));
        }

        customerGO.UpdateIcon(sprites);
    }

    public void SpawnContinue()
    {
        customerNext = null;
        SpawnCustomer();
    }

    public void CompleteOrder()
    {
        isSwitching = true;

        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(1f); // chờ 1s
        sequence.AppendCallback(() =>
        {
            SoundsManager.Instance.PlaySFX(SoundType.OrderComplete);

            currentCus.MoveOut(outTf.position);
            currentCus = customerNext;

            if (customerNext != null && orderNext != null)
            {
                StartOrder(customerNext, orderNext);
            }
            else
            {
                GameManager.Instance.WinGame();
            }
        });
    }

    public List<FoodType> GetCurrentOrder()
    {
        return DataManager.Instance.LevelData.Levels[GameManager.Instance.currentLevel - 1].Orders[currentId-1].Foods;
    }
}
