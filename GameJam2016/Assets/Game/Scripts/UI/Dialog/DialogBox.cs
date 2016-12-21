using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    public PointerListener button;
    public float speed;

    Vector2 sizeDelta;
    Vector2 bigSizeDelta;
    RectTransform tr;

    void Awake()
    {
        tr = GetComponent<RectTransform>();
        sizeDelta = tr.sizeDelta;
        bigSizeDelta = new Vector2(sizeDelta.x, sizeDelta.y * 1.33333f);
        tr.sizeDelta = new Vector2(0, tr.sizeDelta.y);
        SetSmall();
    }

    public void SetBig(TweenCallback onComplete = null)
    {
        tr.DOKill();
        tr.DOSizeDelta(bigSizeDelta, 1 / speed).SetEase(Ease.OutQuad).OnComplete(onComplete);
    }

    public void SetSmall(TweenCallback onComplete = null)
    {
        tr.DOKill();
        tr.DOSizeDelta(sizeDelta, 1 / speed).SetEase(Ease.OutQuad).OnComplete(onComplete);
    }

    public void Exit()
    {
        tr.DOKill();
        tr.DOSizeDelta(new Vector2(0, tr.sizeDelta.y), 1 / speed).SetEase(Ease.InQuad).OnComplete(OnCompleteDestroy);
    }

    void OnCompleteDestroy()
    {
        transform.DOKill();
        Destroy(gameObject);
    }
}
