using DG.Tweening;
using SoundManager;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IceView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;

    public void SetHealth(int health)
    {
        healthText.text = health.ToString();
    }

    public void DecreaseHealth(int health, float time = 0f, float timeMove = 0.5f)
    {
        StartCoroutine(SetHealthCoroutine(health, time, timeMove));
    }

    IEnumerator SetHealthCoroutine(int health, float time = 0f, float timeMove = 0.5f)
    {
        yield return new WaitForSeconds(timeMove + time);

        healthText.text = health.ToString();
    }

    public void Melt(float time = 0f, float timeMove = 0.5f)
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(/*0.8f*/ timeMove + 0.3f + time);
        sequence.AppendCallback(() =>
        {
            SoundsManager.Instance.PlaySFX(SoundType.IceMelt);
        });
        sequence.Append(this.transform.DOScale(Vector3.zero, 0.2f));
        sequence.AppendCallback(() =>
        {
            Destroy(gameObject);
        });
    }
}
