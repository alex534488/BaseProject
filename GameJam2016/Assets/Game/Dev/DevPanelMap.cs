using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DevPanelMap : MonoBehaviour
{
    public Button mainButton;
    public Text text;
    Color highlightColor = new Color(197.0f / 255.0f, 255.0f / 255.0f, 187.0f / 255.0f);

    public void Show()
    {
        mainButton.image.color = highlightColor;
        gameObject.SetActive(true);
        text.text = GetInfo();
    }
    public void Hide()
    {
        mainButton.image.color = Color.white;
        gameObject.SetActive(false);
    }

    string GetInfo()
    {
        string result = "";
        Map map = Universe.Map;
        for(int i = 0; i < map.nbTerritory; i++)
        {
            result += "- Territoire #" + i + " : ";
            if(map.GetTeamMap()[i] == 1)
            {
                result += "Team: Romain | ";
            } else if (map.GetTeamMap()[i] == 2)
            {
                result += "Team: Barbares | ";
            }
            result += "Territoire Adjacent: ";
            for(int j = 0; j < map.GetMap()[i].Length; j++)
            {
                if(map.GetMap()[i][j]) result += j + " , ";
            }
            result += "\n\n";
        }
        return result;
    }
}
