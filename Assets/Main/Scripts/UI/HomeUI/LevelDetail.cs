using UnityEngine;
using UnityEngine.UI;

public class LevelDetail : MonoBehaviour
{
    public GameObject select;
    public int levelId;

    public System.Action<int> OnClick;

    public void OnClickBtn()
    {
        OnClick?.Invoke(levelId);
    }
}
