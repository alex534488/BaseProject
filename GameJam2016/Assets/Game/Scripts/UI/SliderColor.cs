using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderColor : MonoBehaviour {

    public Color[] separations;
    public Image fill;

    float min;
    float max;
    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        min = slider.minValue;
        max = slider.maxValue;
    }

    public void UpdateSlider(float amount)
    {
        slider.value = amount;
        int i = Mathf.RoundToInt((amount - min) / (max - min) * (separations.Length-1));
        fill.color = separations[i];
    }
}
