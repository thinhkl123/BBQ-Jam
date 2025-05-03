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
}
