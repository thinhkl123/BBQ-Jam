using Atom;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{
    public Button pauseBtn;
    public TextMeshProUGUI levelText;

    private void Start()
    {
        pauseBtn.onClick.AddListener(() =>
        {
            AppManager.Instance.PauseGame(true);
            PauseManager.Instance.ShowUI();
        });

        levelText.text = "Level " + GameManager.Instance.currentLevel;
        if (!DataManager.Instance.LevelData.Levels[GameManager.Instance.currentLevel-1].isHard)
        {
            levelText.color = Color.white;
        }

        GameManager.Instance.isLost = false;
    }
}
