using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{

    public PointerListener button;
    public float speed;

    void Awake()
    {
        RectTransform tr = GetComponent<RectTransform>();
        Vector2 sizeDelta = tr.sizeDelta;
        tr.sizeDelta = new Vector2(0, sizeDelta.y);
        tr.DOSizeDelta(sizeDelta, 1 / speed).SetEase(Ease.OutQuad);
    }

    public void Exit()
    {
        RectTransform tr = GetComponent<RectTransform>();
        tr.DOKill();
        tr.DOSizeDelta(new Vector2(0, tr.sizeDelta.y), 1 / speed).SetEase(Ease.InQuad).OnComplete(OnCompleteDestroy);
    }

    void OnCompleteDestroy()
    {
        transform.DOKill();
        Destroy(gameObject);
    }
}
