using LevelManager;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FoodInfo
{
    public FoodType FoodType;
    public Sprite Icon;
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
}
