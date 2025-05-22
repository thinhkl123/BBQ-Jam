using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManager
{
    public enum PortType
    {
        Type1 = 1,
        Type2 = 2,
        Type3 = 3,
        Type4 = 4,
    }

    [Serializable]
    public class PortConfig
    {
        public PortType PortType;
        public PortView PortPrefab;
    }

    [CreateAssetMenu(menuName = "PortData", fileName = "PortData")]
    public class PortSO : ScriptableObject
    {
        public List<PortConfig> PortConfigs;

        public PortView GetPrefab(PortType portType)
        {
            foreach (var config in PortConfigs)
            {
                if (config.PortType == portType)
                {
                    return config.PortPrefab;
                }
            }

            return null;
        }
    }
}

