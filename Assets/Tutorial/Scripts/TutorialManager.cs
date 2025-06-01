using DG.Tweening;
using FoodLevelData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
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
        else
        {
            ShowPopUp();
        }
    }

    private void Update()
    {
        if (isDone) return;

        if (isPopUp)
        {    
            if (Input.touchCount > 0)
            {
                HidePopUp();
            }
        }
    }

    private void ShowPopUp()
    {
        //float animationDuration = 0.3f;
        //Ease easeType = Ease.OutBack;

        //RectTransform popupTransform = PopUpOb.GetComponentInChildren<RectTransform>();

        //popupTransform.localScale = Vector3.zero;
        //popupTransform.DOScale(Vector3.one, animationDuration)
        //              .SetEase(easeType);

        //PopUpOb.GetComponent<CanvasGroup>().DOFade(1f, animationDuration);

        PopUpOb.SetActive(true);
    }

    private void HidePopUp()
    {
        //PopUpOb.SetActive(false);
        isDone = true;

        float animationDuration = 0.3f;
        Ease easeType = Ease.OutBack;

        RectTransform popupTransform = PopUpOb.GetComponentInChildren<RectTransform>();

        popupTransform.localScale = Vector3.one;
        popupTransform.DOScale(Vector3.zero, animationDuration)
                      .SetEase(easeType);

        PopUpOb.GetComponent<CanvasGroup>().DOFade(0f, animationDuration).
            OnComplete(() =>
            {
                PopUpOb.SetActive(false);
            });
    }

    public bool CheckStep(string Obname, Direction direction)
    {
        if (isPopUp)
        {
            return false;
        }

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
