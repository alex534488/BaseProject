using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapCity : MonoBehaviour {
    public Color criticalColor = Color.red;
    public string cityName;
    public Image terrain;
    public Image highlight;
    public Image resourceIcon;
    public Text resourceText;
    public Text resourceSecondaryText;

    private Village village;
    private Resource_Type type;
    private Resource_Type secondType;

    public void Init(Village village)
    {
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
}
