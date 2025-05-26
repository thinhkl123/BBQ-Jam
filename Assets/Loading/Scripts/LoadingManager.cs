using SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;

    [SerializeField] protected LoadingUI _loadingUI;

    public LoadingUI LoadingUI { get { return _loadingUI; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //_loadingUI.gameObject.SetActive(false);
        LoadScene("Home");
    }

    public void LoadScene(string sceneName)
    {
        //SoundsManager.Instance.StopMusic();

        _loadingUI.gameObject.SetActive(true);
        _loadingUI.LoadLevel(sceneName);
    }
}
