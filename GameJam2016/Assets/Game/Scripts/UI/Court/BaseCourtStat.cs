using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class BaseCourtStat : MonoBehaviour
{

    public string prefix;
    public Text text;
    public Color positiveColor = new Color(0.22f, 0.43f, 0);
    public Color negativeColor = new Color(0.43f, 0, 0);
    [Header("Gain anim")]
    public Text gainTextPrefab;
    public bool animateGain;
    public Vector3 gainAnimOffset;
    public Color positiveGainColor = new Color(0.75f, 1, 0.5f);
    public Color negativeGainColor = new Color(1, 0.5f, 0.5f);

    protected bool stopUpdating;
    protected bool skipGainAnimation;
    protected int value;
    [System.NonSerialized]
    protected Empire empire;

    protected virtual void Start()
    {
        DayManager.OnNewDay.AddListener(OnNewDay);
        DayManager.OnNewDayTransition.AddListener(OnTransitionToNewDay);
        DayManager.OnArrival.AddListener(OnNewDay);
        DayManager.SyncToUniverseInit(Init);
    }

    private void Init()
    {
        Universe.OnSetNewWorld.AddListener(LinkToEmpire);
        LinkToEmpire();
    }

    protected virtual void LinkToEmpire()
    {
        empire = Universe.Empire;

        if (empire == null)
        {
            Debug.LogWarning("null empire");
            return;
        }
    }

    //Update la 'value'
    protected virtual void OnStatSet(int newValue)
    {
        if (stopUpdating)
            return;

        if (animateGain)
        {
            if (skipGainAnimation)
                skipGainAnimation = false;
            else
            {
                int delta = newValue - value;
                if (delta != 0)
                    GainAnimation(delta);
            }
        }

        value = newValue;

        UpdateDisplay();
    }

    protected virtual void OnTransitionToNewDay()
    {
        stopUpdating = true;
    }

    protected virtual void GainAnimation(int amount)
    {
        if (gainTextPrefab == null)
            return;

        Text gainText = Instantiate(gainTextPrefab.gameObject).GetComponent<Text>();

        gainText.text = "" + (amount > 0 ? "+" : "") + amount;
        gainText.color = amount < 0 ? negativeGainColor : positiveGainColor;

        gainText.rectTransform.SetParent(text.GetComponent<RectTransform>(), true);
        gainText.transform.localScale = Vector3.one;
        gainText.rectTransform.anchoredPosition = gainAnimOffset;

        gainText.rectTransform.DOAnchorPosX(gainText.rectTransform.anchoredPosition.x + 75f, 1.75f).SetEase(Ease.OutQuint);
        gainText.color = new Color(gainText.color.r, gainText.color.g, gainText.color.b, 0);
        gainText.DOFade(1, 0.1f);
        gainText.DOFade(0, 0.4f).SetDelay(1.75f - 0.4f).OnComplete(delegate () { Destroy(gainText.gameObject); });
    }

    protected virtual void UpdateDisplay()
    {
        if (text != null)
            text.text = prefix + value.ToString();
        text.color = value < 0 ? negativeColor : positiveColor;
    }

    protected virtual void OnNewDay()
    {
        stopUpdating = false;
    }
}
