using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class LoseTimer : MonoBehaviour {

    static List<LoseTimer> timers = new List<LoseTimer>();

    public static LoseTimer NewTimer(LoseTimer prefab, float timeLeft, UnityAction finishedCallback = null)
    {
        LoseTimer timer = Instantiate(prefab.gameObject).GetComponent<LoseTimer>();

        timer.transform.SetParent(CanvasMain.main.transform);

        timer.Init(timeLeft, finishedCallback);

        return timer;
    }

    public Text timeText;
    private float timeLeft;
    private bool finished = false;
    UnityAction finishedCallback = null;

    public void Init(float timeLeft, UnityAction finishedCallback = null)
    {
        this.finishedCallback = finishedCallback;
        this.timeLeft = timeLeft;
        float y = timers.Count * (GetComponent<RectTransform>().sizeDelta.y + 20);
        GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -y, 0);

        timers.Add(this);
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            Finish();
        }
        else UpdateLayout(timeLeft);
    }

    void UpdateLayout(float value)
    {
        timeText.text = ((int)(value*10))/10 + " s";
    }

    void Finish()
    {
        if (finished) return;

        if(finishedCallback != null)finishedCallback.Invoke();
        UpdateLayout(0);
        finished = true;
    }

    public void Kill()
    {
        timers.Remove(this);
        Destroy(gameObject);
    }
}
