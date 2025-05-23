using FoodLevelData;
using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "RockLevelData", fileName = "RockLevelData")]
public class RockSO : ScriptableObject
{
    public List<RockLevel> RockLevelList;
}

[Serializable]
public class Rock
{
    public RockView RockPrefab;
    public List<Direction> DirectionList;
    public Vector2Int Pos;
}

[Serializable]
public class RockLevel
{
    public int RockId;
    public List<Rock> RockList;
}