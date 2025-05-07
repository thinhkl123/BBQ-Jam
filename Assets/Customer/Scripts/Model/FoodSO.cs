using LevelManager;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FoodInfo
{
    public FoodType FoodType;
    public Sprite Icon;
    public Vector3 Scale;
}

[CreateAssetMenu(menuName = "FoodData", fileName = "FoodData")]
public class FoodSO : ScriptableObject
{
    public List<FoodInfo> FoodList;

    public Sprite GetIcon(FoodType foodType)
    {
        foreach (FoodInfo item in FoodList)
        {
            if (item.FoodType == foodType)
            {
                return item.Icon;
            }
        }

        return null;
    }

    public Vector3 GetScale(FoodType foodType)
    {
        foreach (FoodInfo item in FoodList)
        {
            if (item.FoodType == foodType)
            {
                return item.Scale;
            }
        }

        return Vector3.one;
    }
}
