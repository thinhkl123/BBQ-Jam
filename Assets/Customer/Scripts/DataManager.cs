using LevelManager;
using UnityEngine;
using FoodLevelData;
using MatrixData;

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
}
