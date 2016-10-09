using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class CapitalStat : MonoBehaviour
{
    public Ressource_Type type;
    public Text totalText;
    public Text profitText;
    Capitale capital;

    public Text statTextPrefab;
    public Vector2 offset;

    void Start()
    {
        capital = World.main.empire.capitale;
        Village.StatEvent ev = capital.GetStatEvent(type, false);
        ev.AddListener(OnValueChange);
        Village.StatEvent evAlt = capital.GetStatEvent(type, true);
        if(evAlt != ev) evAlt.AddListener(UpdateDisplay);
        UpdateDisplay(0);
    }
    void UpdateDisplay(int dummy)
    {
        if (totalText != null) totalText.text = "" + capital.GetTotal(type);
        if (profitText != null)
        {
            if(type == Ressource_Type.happiness)
            {
                totalText.text = ""+capital.bonheur + "/" + capital.bonheurMax;
            }
            else
            {
                int bilan = capital.GetBilan(type);
                profitText.text = "" + (bilan > 0 ? "+" : "") + bilan;
            }
        }
    }

    void OnValueChange(int amount)
    {
        if (amount == 0)
            return;

        UpdateDisplay(amount);
        Text text = Instantiate(statTextPrefab.gameObject).GetComponent<Text>();

        text.text = "" + (amount > 0 ? "+" : "") + amount;

        text.rectTransform.SetParent(this.GetComponent<RectTransform>(),true);
        text.transform.localScale = Vector3.one;
        text.rectTransform.anchoredPosition = offset;

        text.rectTransform.DOAnchorPosX(text.rectTransform.anchoredPosition.x + 75f, 1.75f).SetEase(Ease.OutQuint);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        text.DOFade(1, 0.1f);
        text.DOFade(0, 0.4f).SetDelay(1.75f - 0.4f).OnComplete(delegate () { Destroy(text.gameObject); });
    }
}
