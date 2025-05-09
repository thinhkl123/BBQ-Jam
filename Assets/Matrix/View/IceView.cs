using DG.Tweening;
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

    public void DecreaseHealth(int health)
    {
        StartCoroutine(SetHealthCoroutine(health));
    }

    IEnumerator SetHealthCoroutine(int health)
    {
        yield return new WaitForSeconds(0.5f);

        healthText.text = health.ToString();
    }

    public void Melt()
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(0.8f);
        sequence.Append(this.transform.DOScale(Vector3.zero, 0.2f));
        sequence.AppendCallback(() =>
        {
            Destroy(gameObject);
        });
    }
}
