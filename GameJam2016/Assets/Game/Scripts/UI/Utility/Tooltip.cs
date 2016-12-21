using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(PointerListener))]
public class Tooltip : MonoBehaviour {

    static Vector2 screenRef = new Vector2(1052, 592);
    static Vector2 modif;

    public string text;
    public GameObject prefab;
    public Vector3 offset;
    public RectTransform container;

    Transform currentTooltip;
    
	void Awake () {
        if (modif == Vector2.zero) modif = new Vector2(Screen.width / screenRef.x, Screen.height / screenRef.y);
        PointerListener pointerListener = GetComponent<PointerListener>();

        pointerListener.onPointerEnter.AddListener(OnPointerEnter);
        pointerListener.onPointerExit.AddListener(OnPointerExit);
    }

    void OnPointerEnter()
    {
        if(prefab == null)
        {
            Debug.LogWarning("Tooltip prefab is null");
            return;
        }
        currentTooltip = Instantiate(prefab.gameObject).transform;
        currentTooltip.SetParent(container);
        currentTooltip.localScale = Vector3.one;
        currentTooltip.position = transform.position + new Vector3(offset.x*modif.x, offset.y*modif.y, 0) ;

        Text text = currentTooltip.GetComponentInChildren<Text>();
        text.text = this.text;
    }

    void OnPointerExit()
    {
        if (currentTooltip != null) Destroy(currentTooltip.gameObject);
    }
}
