using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DevPanelMap : MonoBehaviour
{
    public Button mainButton;
    Color highlightColor = new Color(197.0f / 255.0f, 255.0f / 255.0f, 187.0f / 255.0f);

    public void Show()
    {
        mainButton.image.color = highlightColor;
    }
    public void Hide()
    {
        mainButton.image.color = Color.white;
        gameObject.SetActive(false);
    }
}
