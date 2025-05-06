using MatrixData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FoodLevelData
{
    [Serializable]
    public enum Direction
    {
        Left = 1,
        Right = 2,
        Up = 3,
        Down = 4,
        None = 0,
    }

    [Serializable]
    public class FoodLevel
    {
        public GameObject Prefab;
        public List<Direction> DirectionList;
        public List<Vector2Int> PoseList;
        public bool isCooked;
    }

    [Serializable]
    public class FoodsLevel
    {
        public int LevelId;
        public MatrixType MatrixType;
        public List<FoodLevel> FoodLevelList;
    }

    [CreateAssetMenu(menuName = "FoodLevelData", fileName = "FoodLevelData")]
    public class FoodLevelSO : ScriptableObject
    {
        public List<FoodsLevel> FoodsLevelList;
    }
}
