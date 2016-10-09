using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(PointerListener))]
public class Tooltip : MonoBehaviour {

    public string text;
    public GameObject prefab;
    public Vector3 offset;
    public RectTransform container;

    Transform currentTooltip;
    
	void Awake () {
        PointerListener pointerListener = GetComponent<PointerListener>();

        pointerListener.onPointerEnter.AddListener(OnPointerEnter);
        pointerListener.onPointerExit.AddListener(OnPointerExit);
    }

    void OnPointerEnter()
    {
        currentTooltip = Instantiate(prefab.gameObject).transform;
        currentTooltip.SetParent(container);
        currentTooltip.localScale = Vector3.one;
        currentTooltip.position = transform.position + offset;

        Text text = currentTooltip.GetComponentInChildren<Text>();
        text.text = this.text;
    }

    void OnPointerExit()
    {
        if (currentTooltip != null) Destroy(currentTooltip.gameObject);
    }
}
