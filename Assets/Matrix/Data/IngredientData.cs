using System;
using UnityEngine;

public enum IngredientType
{
    None = 0,
    Sausage = 1,
    Bacon = 2,
    Mushroom = 3,
    Bacon1 = 4,
}

[Serializable]
public class IngredientDetail
{
    public Vector3 ScaleDefault;
    public Vector3 Scale;
}

[CreateAssetMenu(menuName = "IngredientData")]
public class IngredientData : ScriptableObject
{
    [SerializeField] IngredientDetail[] ingredients;

    public IngredientDetail GetIngredientDetail(IngredientType ingredientType)
    {
        return ingredients[(int)ingredientType];
    }
}
