using UnityEngine;
using UnityEngine.UI;

public class LevelDetail : MonoBehaviour
{
    public GameObject select;
    public Image active;
    public int levelId;

    public Button button;

    public System.Action<int> OnClick;

    public void OnClickBtn()
    {
        OnClick?.Invoke(levelId);
    }
}
