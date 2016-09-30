using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelUpChoiceItem : MonoBehaviour {

    public class ItemEvent: UnityEvent<LevelUp.Boost> { }
    public Image icon;
    public Text amounText;
    public Text descriptionText;
    public GameObject descriptionObj;
    public PointerListener button;

    public ItemEvent onChoose = new ItemEvent();
    
    public void Init(LevelUp.Boost boost)
    {
        LevelUp.BoostType type = LevelUp.GetBoostTypeByType(boost.type);

        descriptionText.text = type.description;
        descriptionObj.SetActive(false);
        icon.sprite = type.sprite;

        float roundedAmount = ((int)boost.amount * 100) / 100;
        amounText.text = "+" + roundedAmount;

        button.onClick.AddListener(delegate () { onChoose.Invoke(boost); });
        button.onPointerEnter.AddListener(Enter);
        button.onPointerExit.AddListener(Exit);
    }

    void Enter()
    {
        descriptionObj.SetActive(true);
    }
    void Exit()
    {
        descriptionObj.SetActive(false);
    }
}
