using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class PointerListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent onClick = new UnityEvent();
    public UnityEvent onPointerDown = new UnityEvent();
    public UnityEvent onPointerUp = new UnityEvent();
    public UnityEvent onPointerEnter = new UnityEvent();
    public UnityEvent onPointerExit = new UnityEvent();

    public void OnPointerClick(PointerEventData e)
    {
        if (!enabled) return;

        onClick.Invoke();
    }

    public void OnPointerDown(PointerEventData e)
    {
        if (!enabled) return;

        onPointerDown.Invoke();
    }

    public void OnPointerUp(PointerEventData e)
    {
        if (!enabled) return;
        
        onPointerUp.Invoke();
    }

    public void OnPointerEnter(PointerEventData e)
    {
        if (!enabled) return;

        onPointerEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData e)
    {
        if (!enabled) return;

        onPointerExit.Invoke();
    }
}