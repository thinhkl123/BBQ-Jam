using LevelManager;
using UnityEngine;
using FoodLevelData;
using MatrixData;
using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using System;
using System.Collections;

public class DataManager : MonoSingleton<DataManager>
{
    public LevelSO LevelData;
    public FoodSO FoodData;
    public CustomerSO CustomerData;
    public FoodLevelSO FoodLevelData;
    public MatrixSO MatrixData;
    public IceSO IceData;
    public PortLevelSO PortLevelData;
    public PortSO PortData;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);
    }

    public int GetCurMaxLevel()
    {
        GetUserData();
        int curMaxLevel = PlayerPrefs.GetInt("MaxLevel");
        Debug.Log(curMaxLevel);

        return curMaxLevel;
    }

    public void GetCurMaxLevel(Action<int> callback)
    {
        StartCoroutine(WaitAndGetMaxLevel(callback));
    }

    private IEnumerator WaitAndGetMaxLevel(Action<int> callback)
    {
        GetUserData();
        yield return new WaitForSeconds(2f);

        int curMaxLevel = PlayerPrefs.GetInt("MaxLevel");
        Debug.Log(curMaxLevel);

        callback?.Invoke(curMaxLevel);
    }


    public void UpdateUserData(int level)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
        {
            { "Level", level.ToString() }
        }
        };

        PlayFabClientAPI.UpdateUserData(request, result =>
        {
            Debug.Log("User data saved successfully!");
        }, error =>
        {
            Debug.LogError("Failed to save user data: " + error.GenerateErrorReport());
        });
    }

    public void GetUserData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("Level"))
            {
                string level = result.Data["Level"].Value;
                PlayerPrefs.SetInt("MaxLevel", Int32.Parse(level));
            }
            else
            {
                Debug.Log("New Player Data: Level 1");
                PlayerPrefs.SetInt("MaxLevel", 1);
            }
        }, error =>
        {
            Debug.LogError("Failed to load user data: " + error.GenerateErrorReport());
            PlayerPrefs.SetInt("MaxLevel", 1);
        });
    }
}
