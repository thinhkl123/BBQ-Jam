using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IceLevelData", fileName = "IceLevelData")]
public class IceSO : ScriptableObject
{
    public IceView IcePrefab;
    public List<IceLevel> IceLevelList; 
}

[Serializable]
public class Ice
{
    public Vector2Int Pos;
    public int Health;
}

[Serializable]
public class IceLevel
{
    public int LevelId;
    public List<Ice> IceList;
}
