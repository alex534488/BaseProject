using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class DialogText : MonoBehaviour {

    public class DialogTextEvent: UnityEvent<DialogText> { }
    [Header("Component")]
    public Text text;
    public PointerListener button;
    [Header("Variables")]
    public Color normalColor = Color.black;
    public Color highlightColor = Color.gray;
    public float characterSpeed = 0.1f;
    public bool highlightOnHover = false;
    public DialogTextEvent onChoose = new DialogTextEvent();

    bool hasInit = false;
    bool isAnimating = false;
    Tweener tweener = null;

    string message;

    void Awake()
    {
        if (!hasInit) gameObject.SetActive(false);

        button.onPointerEnter.AddListener(OnPointerEnter);
        button.onPointerExit.AddListener(OnPointerExit);
        button.onClick.AddListener(OnClick);
    }

    public void Init(string message, bool isAnimated)
    {
        this.message = message;

        if (isAnimated)
        {
            text.text = "";
            isAnimating = true;
            int characterCount = message.Length;
            tweener = text.DOText(message, characterCount / characterSpeed, false, ScrambleMode.None).SetEase(Ease.Linear).OnComplete(OnCompletAnim);
        }
        else
        {
            text.text = message;
        }

        hasInit = true;
        gameObject.SetActive(true);
    }
    
    void Highlight(bool state)
    {
        text.color = state ? highlightColor : normalColor;
    }

    public void SpeedUp()
    {
        if (tweener == null || !isAnimating) return;
        tweener.Kill();
        OnCompletAnim();
        text.text = message;
    }

    public void Exit()
    {
        Destroy(gameObject);
    }

    //Events

    void OnPointerEnter()
    {
        if (highlightOnHover) Highlight(true);
    }

    void OnPointerExit()
    {
        Highlight(false);
    }

    void OnClick()
    {
        if (isAnimating)
        {
            SpeedUp();
        }
        else
        {
            if (onChoose != null)
            {
                onChoose.Invoke(this);
            }
        }
    }

    void OnCompletAnim()
    {
        isAnimating = false;
    }
}
