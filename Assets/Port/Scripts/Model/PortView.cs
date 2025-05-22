using FoodLevelData;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PortView : MonoBehaviour
{
    public List<DirectionCombo> DirectionCombos;
}

[Serializable]
public class DirectionCombo
{
    public Direction directionIn;
    public Direction directionOut;
}
