using LevelManager;
using UnityEngine;

public class DataManager : MonoSingleton<DataManager>
{
    public LevelSO LevelData;
    public FoodSO FoodData;
    public CustomerSO CustomerData;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);
    }
}
