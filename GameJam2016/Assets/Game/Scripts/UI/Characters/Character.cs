using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Events;

public class Character : MonoBehaviour
{

    [Header("Display")]
    public SpriteRenderer cheveux;
    public SpriteRenderer tete;
    public SpriteRenderer bras;
    public SpriteRenderer corps;
    public SpriteRenderer coup;
    [Header("Stuff")]
    public Animator anim;
    public float moveSpeed = 1;

    List<Transform> dest = new List<Transform>();
    UnityAction onComplete;

    bool isMoving = false;

    public void Init(Transform spawnPoint, IKit kit = null)
    {
        if (kit == null) ApplyKit(CharacterBank.GetRandomKit());
        else ApplyKit(kit);

        transform.position = spawnPoint.position;
        transform.localScale = spawnPoint.localScale;
    }

    void ApplyKit(IKit kit)
    {
        tete.sprite = kit.Head();
        tete.color = kit.SkinColor();

        cheveux.sprite = kit.Hair();
        cheveux.color = kit.HairColor();

        coup.color = kit.SkinColor();

        corps.sprite = kit.Body();
        corps.color = kit.BodyColor();

        bras.sprite = kit.Arms();
        bras.color = kit.SkinColor();
    }

    public void QueueMoveTo(Transform destination, UnityAction onComplete = null)
    {
        this.onComplete = onComplete;
        dest.Add(destination);
        if (!isMoving) NextMoveTo();
    }

    void NextMoveTo()
    {
        float duration = (dest[0].position - transform.position).magnitude / moveSpeed;
        float scaleDelta = Mathf.Abs((transform.localScale - dest[0].localScale).magnitude);
        duration += (scaleDelta * 5 * 1.5f / moveSpeed);

        transform.DOScale(dest[0].localScale, duration).SetEase(dest[0].localScale.magnitude > transform.localScale.magnitude ? Ease.InSine : Ease.OutSine);
        transform.DOMove(dest[0].position, duration).SetEase(Ease.Linear).OnComplete(EndItem);
        isMoving = true;
        anim.SetBool("walking", true);
    }

    void EndItem()
    {
        dest.RemoveAt(0);
        if (dest.Count > 0) NextMoveTo();
        else
        {
            isMoving = false;
            anim.SetBool("walking", false);
            if (onComplete != null) onComplete.Invoke();
        }
    }
}
