using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NextDayButton : MonoBehaviour {

    Button button;
    void Start()
    {
        button = GetComponent<Button>();
        DayManager.OnNewDay.AddListener(OnNewDay);
        DayManager.OnNewDayTransition.AddListener(OnNewDayTransition);
        DayManager.OnArrival.AddListener(OnArrival);
        RequestManager.OnBeginRequests.AddListener(OnBeginRequests);
        RequestManager.OnCompleteRequests.AddListener(OnCompleteRequests);
    }

    void OnBeginRequests()
    {
        button.interactable = false;
    }

    void OnCompleteRequests()
    {
        button.interactable = true;
    }

    void OnNewDayTransition()
    {
        button.interactable = false;
    }

    void OnArrival()
    {
        button.interactable = !RequestManager.IsRequesting;
    }

    void OnNewDay()
    {
        button.interactable = !RequestManager.IsRequesting;
    }
}
