using LevelManager;
using UnityEngine;
using FoodLevelData;
using MatrixData;
using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using System;

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
        int curMaxLevel = PlayerPrefs.GetInt("MaxLevel", 1);
        //Debug.Log(curMaxLevel);

        return curMaxLevel;
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
        }, error =>
        {
            Debug.LogError("Failed to load user data: " + error.GenerateErrorReport());
        });
    }
}
