using LevelManager;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomerInfo
{
    public CustomerType CustomerType;
    public Customer CustomerPrefab;
}

[CreateAssetMenu(menuName = "CustomerData", fileName = "CustomerData")]
public class CustomerSO : ScriptableObject
{
    public List<CustomerInfo> CustomerList;

    public Customer GetPrefab(CustomerType customerType)
    {
        foreach (var item in CustomerList)
        {
            if (item.CustomerType == customerType)
            {
                return item.CustomerPrefab;
            }
        }

        return null;
    }
}
