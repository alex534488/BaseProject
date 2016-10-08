using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CapitalStat : MonoBehaviour {
    public Ressource_Type type;
    public Text totalText;
    public Text profitText;
    Capitale capital;

    void Start()
    {
        capital = World.main.empire.capitale;
        UpdateDisplay();
    }
    void UpdateDisplay()
    {
        if (profitText != null) totalText.text = "" + capital.GetTotal(type);
        if(profitText != null) profitText.text = "" + capital.GetBilan(type);
    }
}
