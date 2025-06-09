using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class ItemAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private bool flipValueOnEnd;
    [SerializeField] private bool playOnce;

    [SerializeField] private UnityEvent onAnimationStart;
    [SerializeField] private UnityEvent onAnimationEnd;

    private bool isAlreadyPlayed = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveItem(Vector3 target)
    {
        if (playOnce && isAlreadyPlayed) return;
        DOTween.Init();
        transform.position = startPosition;
        onAnimationStart?.Invoke();
        transform.DOLocalMove(target, 0.1f).SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                if (flipValueOnEnd) FlipPositionValue();
                if (playOnce) isAlreadyPlayed = true;
                onAnimationEnd?.Invoke();
            });
    }

    public void MoveItem()
    {
        if (playOnce && isAlreadyPlayed) return;
        DOTween.Init();
        transform.position = startPosition;
        onAnimationStart?.Invoke();
        transform.DOLocalMove(endPosition, 0.1f).SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                if (flipValueOnEnd) FlipPositionValue();
                if (playOnce) isAlreadyPlayed = true;
                onAnimationEnd?.Invoke();
            });
    }

    private void FlipPositionValue() {
        Vector3 temp = startPosition;
        startPosition = endPosition;
        endPosition = temp;
    }

}
