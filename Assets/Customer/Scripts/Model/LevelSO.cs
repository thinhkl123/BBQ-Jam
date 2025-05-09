using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManager
{
    public enum CustomerType
    {
        M19 = 0,
        M33 = 1,
        M2 = 2, 
        M27 = 3,
        F5 = 4,
        F7 = 5,
    }

    public enum FoodType
    {
        None = 0,
        Mushroom = 1,
        Bacon = 2,
        Sausage = 3,
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
        public int TutorialId;
        public int IceId;
        public List<Order> Orders;
    }

    [CreateAssetMenu(menuName = "LevelData", fileName = "LevelData")]
    public class LevelSO : ScriptableObject
    {
        public List<Level> Levels;
    }
}
