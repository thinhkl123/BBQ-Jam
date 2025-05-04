using Atom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{
    public Button pauseBtn;

    private void Start()
    {
        pauseBtn.onClick.AddListener(() =>
        {
            AppManager.Instance.PauseGame(true);
            PauseManager.Instance.ShowUI();
        });
    }
}
