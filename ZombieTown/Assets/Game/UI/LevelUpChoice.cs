using UnityEngine;
using System.Collections;

public class LevelUpChoice : MonoBehaviour {

    public LevelUpChoiceItem prefab;
    public float verticalOffset = 75;

    LevelUp from;
    public static void Ask(LevelUp.Boost boostA, LevelUp.Boost boostB, LevelUp from, LevelUpChoice prefab, Transform parent)
    {
        LevelUpChoice choice = Instantiate(prefab.gameObject).GetComponent<LevelUpChoice>();
        choice.gameObject.transform.SetParent(parent);
        choice.Init(boostA, boostB, from);
    }

    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(from.transform.position) + Vector3.up * verticalOffset;
    }

    void Init(LevelUp.Boost boostA, LevelUp.Boost boostB, LevelUp from)
    {
        this.from = from;
        Spawn(boostA);
        Spawn(boostB);
    }

    void Spawn(LevelUp.Boost boost)
    {
        LevelUpChoiceItem item = Instantiate(prefab.gameObject).GetComponent<LevelUpChoiceItem>();
        item.Init(boost);
        item.onChoose.AddListener(OnChoose);
        item.gameObject.transform.SetParent(transform, true);
    }

    void OnChoose(LevelUp.Boost item)
    {
        from.ApplyBoost(item);
        Destroy(gameObject);
    }
}
