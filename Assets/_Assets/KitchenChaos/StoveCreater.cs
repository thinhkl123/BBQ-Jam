using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCreater : MonoBehaviour
{
    public Vector2Int Size;
    public GameObject stovePrefab;

    public Vector2 Distance;
    public Vector2 second;

    private Vector3[,] arr;

    private void Start()
    {
        Create(Size);
    }

    private void Create(Vector2Int size)
    {
        arr = new Vector3[size.x, size.y];

        for (int i = 0; i<size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector3 posInit = Vector3.zero;
                Vector3 rot = new Vector3(0, -90f, 0);
                if (j == 0)
                {
                    posInit = new Vector3(0, 0, -i * Distance.y);
                    rot = new Vector3(0, 90f, 0);
                }
                else if (j == 1)
                {
                    posInit = new Vector3(arr[i, 0].x + second.x, 0, arr[i, 0].z + second.y);
                }
                else
                {
                    posInit = new Vector3(arr[i, j-1].x + Distance.x, 0, arr[i, j-1].z);
                }

                arr[i, j] = posInit;
                Instantiate(stovePrefab, posInit, Quaternion.Euler(rot), this.transform);
            }
        }
    }
}
