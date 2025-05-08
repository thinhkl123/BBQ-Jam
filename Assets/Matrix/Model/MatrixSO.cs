using System;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixData
{
    [Serializable]
    public enum MatrixType
    {
        M4x4 = 0,
        M5x5 = 1,
        M5x6 = 2,
        M6x6 = 3,
        M6x8 = 4,
    }

    [Serializable]
    public class MatrixConfig
    {
        public MatrixType MatrixType;
        public Vector2Int MatrixSize;
        public GameObject Prefab;
        public Vector3 Position;
    }

    [CreateAssetMenu(menuName = "MatrixData", fileName = "MatrixData")]
    public class MatrixSO : ScriptableObject
    {
        public List<MatrixConfig> MatrixConfigs;

        public GameObject GetMatrixPrefab(MatrixType type)
        {
            foreach (var mat in MatrixConfigs)
            {
                if (mat.MatrixType == type)
                {
                    return mat.Prefab;
                }
            }

            return null;
        }

        public Vector3 GetMatrixPosition(MatrixType type)
        {
            foreach (var mat in MatrixConfigs)
            {
                if (mat.MatrixType == type)
                {
                    return mat.Position;
                }
            }

            return Vector3.zero;
        }

        public Vector2Int GetMatrixSize(MatrixType type)
        {
            foreach (var mat in MatrixConfigs)
            {
                if (mat.MatrixType == type)
                {
                    return mat.MatrixSize;
                }
            }

            return Vector2Int.zero;
        }
    }
}
