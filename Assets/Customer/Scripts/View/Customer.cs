using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class Customer : MonoBehaviour
{
    [SerializeField] private ParticleSystem spawnVFX;

    [Header(" Component ")]
    public Animator animator;
    public GameObject Visual;
    
    [Header(" UI ")]
    [SerializeField] private GameObject OrderUI;
    [SerializeField] private GameObject TimerUI;
    [SerializeField] private List<Image> icons;
    [SerializeField] private Image timeBar;

    public bool isOrdering = false;
    public float MaxTime;
    private float currentTime;

    private void Awake()
    {

    }

    private void Start()
    {
        OrderUI.SetActive(false);
        TimerUI.SetActive(false);
        spawnVFX.Play();
    }

    private void Update()
    {
        if (!isOrdering)
        {
            return;
        }

        currentTime -= Time.deltaTime;
        timeBar.fillAmount = currentTime/MaxTime;

        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveOut(CustomerManager.Instance.outTf.position);
            CustomerManager.Instance.CompleteOrder();
        }
    }

    public void SetTransformAtFirst(Vector3 pos, Vector3 rot)
    {
        this.transform.position = pos;
        this.transform.rotation = Quaternion.Euler(rot);
        Invoke(nameof(ShowOrder), 1.5f);
    }

    public void ShowOrder()
    {
        OrderUI.SetActive(true);
    }

    public void MoveTo(Vector3 target, float time = 1f)
    {
        this.animator.SetBool("Walk", true);
        this.transform.DOMove(target, time).SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            this.animator.SetBool("Walk", false);
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0));
            TimerUI.SetActive(true);

            isOrdering = true;
            currentTime = MaxTime;

            CustomerManager.Instance.SpawnContinue();
        });
    }

    public void MoveOut(Vector3 target, float time = 1f)
    {
        OrderUI.SetActive(false);
        TimerUI.SetActive(false);

        this.animator.SetBool("Walk", true);
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 90f, 0));
        this.transform.DOMove(target, time).SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            this.animator.SetBool("Walk", false);
            Destroy(gameObject);
        });
    }

    public void UpdateIcon(List<Sprite> sprites)
    {
        for (int i = 0; i < sprites.Count; i++)
        {
            icons[i].enabled = true;
            icons[i].sprite = sprites[i];
        }
        for (int i = sprites.Count; i < icons.Count; i++)
        {
            icons[i].enabled = false;
        }
    }
}
