using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientModel
{
    public int index;
    public List<MatrixController.Direction> directions;

    public void Init()
    {
        this.index = 0;
        this.directions = new List<MatrixController.Direction>();
    }
}

