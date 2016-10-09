using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CapitalStat : MonoBehaviour
{
    public Ressource_Type type;
    public Text totalText;
    public Text profitText;
    Capitale capital;

    void Start()
    {
        capital = World.main.empire.capitale;
        Village.StatEvent ev = capital.GetStatEvent(type, false);
        ev.AddListener(UpdateDisplay);
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

            }
            else
            {
                int bilan = capital.GetBilan(type);
                profitText.text = "" + (bilan > 0 ? "+" : "") + bilan;
            }
        }
    }
}
