using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using CCC.Utility;

public class CapitalStat : MonoBehaviour
{
    public Ressource_Type type;
    public Ressource_Type secondeType;
    public Text totalText;
    public Text profitText;
    Capitale capital;

    public Text statTextPrefab;
    public Vector2 offset;

    int currentValue;

    void Start()
    {
        capital = World.main.empire.capitale;

        Stat<int>.StatEvent ev = capital.GetStatEvent(type);
        if(ev != null)ev.AddListener(OnValueChange);

        Stat<int>.StatEvent evAlt = capital.GetStatEvent(secondeType);
        if(evAlt != ev && evAlt != null) evAlt.AddListener(UpdateDisplay);

        UpdateDisplay(0);
    }
    void UpdateDisplay(int dummy)
    {
        currentValue = capital.GetResource(type);
        int altValue = capital.GetResource(secondeType);

        if (totalText != null) totalText.text = "" + currentValue;
        if (profitText != null)
        {
            if(type == Ressource_Type.happiness)
            {
                totalText.text = ""+ currentValue + "/" + altValue;
            }
            else
            {
                profitText.text = "" + (altValue >= 0 ? "+" : "") + altValue;
            }
        }
    }

    void OnValueChange(int newValue)
    {
        int change = newValue - currentValue;
        if (change == 0) return;

        UpdateDisplay(-1);
        Text text = Instantiate(statTextPrefab.gameObject).GetComponent<Text>();

        text.text = "" + (change > 0 ? "+" : "") + change;

        text.rectTransform.SetParent(this.GetComponent<RectTransform>(),true);
        text.transform.localScale = Vector3.one;
        text.rectTransform.anchoredPosition = offset;

        text.rectTransform.DOAnchorPosX(text.rectTransform.anchoredPosition.x + 75f, 1.75f).SetEase(Ease.OutQuint);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        text.DOFade(1, 0.1f);
        text.DOFade(0, 0.4f).SetDelay(1.75f - 0.4f).OnComplete(delegate () { Destroy(text.gameObject); });
    }
}
