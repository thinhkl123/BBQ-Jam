using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManager
{
    public enum CustomerType
    {
        WearShort = 0,
        WearClothes = 1,
    }

    public enum FoodType
    {
        Sausage = 0,
        Mushroom = 1,
        Bacon = 2,
    }

    [Serializable]
    public class Order
    {
        public CustomerType Customer;
        public List<FoodType> Foods;
        public float Time;
    }

    [Serializable]
    public class Level
    {
        public int LevelId;
        public List<Order> Orders;
    }

    [CreateAssetMenu(menuName = "LevelData", fileName = "LevelData")]
    public class LevelSO : ScriptableObject
    {
        public List<Level> Levels;
    }
}
