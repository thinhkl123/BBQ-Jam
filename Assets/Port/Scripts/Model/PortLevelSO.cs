using LevelManager;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PortLevel
{
    public PortType PortType;
    public Vector2Int Pos;
}

[Serializable]
public class PortLevelConfig
{
    public int PortLevel;
    public List<PortLevel> Ports;
}

[CreateAssetMenu(menuName = "PortLevelData", fileName = "PortLevelData")]
public class PortLevelSO : ScriptableObject
{
    public List<PortLevelConfig> PortLevels;
}
