using FoodLevelData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public bool isDone = false;
    public bool isPopUp;

    [Header(" Popup ")]
    public GameObject PopUpOb;

    [Header(" Step by step ")]
    public List<GameObject> StepObList;
    public List<TutorialStep> StepList;
    private int currentStep;

    private void Start()
    {
        isDone = false;

        if (!isPopUp)
        {
            currentStep = 0;
            StepObList[0].SetActive(true);
            for (int i = 1; i < StepObList.Count; i++)
            {
                StepObList[i].SetActive(false);
            }
        }
    }

    public bool CheckStep(string Obname, Direction direction)
    {
        if (StepList[currentStep].ObjectName == Obname && StepList[currentStep].Direction == direction)
        {
            NextStep();

            return true;
        }

        return false;
    }

    public string GetCurrentObjectStep()
    {
        return StepList[currentStep].ObjectName;
    }

    public void NextStep()
    {
        currentStep++;
        if (currentStep >= StepObList.Count)
        {
            isDone = true;
            StepObList[currentStep - 1].SetActive(false);
        }
        else
        {
            StepObList[currentStep - 1].SetActive(false);
            StepObList[currentStep].SetActive(true);
        }
    }
}

[Serializable]
public class TutorialStep
{
    public string ObjectName;
    public Direction Direction;
}
