using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class MapCity : MonoBehaviour {
    public Color criticalColor = Color.red;
    public string cityName;
    public Image terrain;
    public Image highlight;
    public Image resourceIcon;
    public Text resourceText;
    public Text resourceSecondaryText;
    public Text cityText;
    public Button button;

    public class MapCityEvent : UnityEvent<MapCity> { }
    public MapCityEvent onClick = new MapCityEvent();

    private Village village;
    private Resource_Type type;
    private Resource_Type secondType;

    private Tweener highlightAnim;
    private bool animating = false;

    public void Init(Village village)
    {
        button.onClick.AddListener(OnClick);
        this.village = village;
    }
    
    public void DestroyAnim()
    {

    }

    public void Display(Resource_Type type, Resource_Type secondType)
    {
        this.type = type;
        this.secondType = secondType;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        resourceIcon.sprite = GameResources.GetIcon(type);
        int value = village.GetResource(type);
        int secondValue = village.GetResource(secondType);

        //First value
        resourceText.color = (value < 0) ? criticalColor : Color.black;
        resourceText.text = value.ToString();

        if (secondType == Resource_Type.armyProd && value == 0) resourceSecondaryText.gameObject.SetActive(false);
        else resourceSecondaryText.gameObject.SetActive(true);

        //Second value
        resourceSecondaryText.color = (secondValue < 0) ?criticalColor: Color.black;
        resourceSecondaryText.text = (secondType == Resource_Type.reputationCap ? "/" : (secondValue >=0 ? "+" : "")) + secondValue;
    }

    void OnClick()
    {
        onClick.Invoke(this);
    }

    public void Highlight()
    {
        animating = true;

        highlight.DOColor(new Color(1, 1, 1, highlight.color.a), 0.25f).SetEase(Ease.InOutSine).OnComplete(delegate()
        {
            if (animating)
            {
                highlightAnim = highlight.DOColor(new Color(0, 0, 0, highlight.color.a), 0.75f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            }
        });
    }

    public void StopHighlight()
    {
        animating = false;

        highlightAnim.Kill();
        highlight.DOKill();
        highlightAnim = highlight.DOColor(new Color(0, 0, 0, highlight.color.a), 0.75f).SetEase(Ease.InOutSine);
    }
}
