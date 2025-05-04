using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI loadingText;

    public void LoadLevel(string nameScene)
    {
        StartCoroutine(LoadAsyncchronously(nameScene));
    }

    IEnumerator LoadAsyncchronously(string nameScene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(nameScene);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            //Debug.Log(progress);
            loadingSlider.value = progress * 100f;
            loadingText.text = $"Loading...{progress*100}%";
            yield return null;
        }

        this.gameObject.SetActive(false);
    }
}
