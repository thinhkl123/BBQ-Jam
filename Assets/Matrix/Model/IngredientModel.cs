using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodLevelData;

public class IngredientModel
{
    public int index;
    public List<Direction> directions;

    public void Init()
    {
        this.index = 0;
        this.directions = new List<Direction>();
    }
}

