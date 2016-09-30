using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelUp : MonoBehaviour
{
    public static LevelUp master;

    public enum Types { Hp, Speed, Dmg, CarryAmount, HealOnKill }
    [System.Serializable]
    public class BoostType
    {
        public Types type;
        public float min;
        public float max = 10;
        public string description;
        public int weight = 1;
        public Sprite sprite;
        public bool integersOnly = true;
    }
    public class Boost
    {
        public Types type;
        public float amount;
    }

    public bool isManager = false;

    [Header("For Manager")]
    public List<BoostType> boosts = new List<BoostType>();
    public LevelUpChoice levelUpChoicePrefab;

    [Header("For Non-Manager")]
    public Zombie zombie;
    public bool waitingForBoost = false;

    void Awake()
    {
        if (master == null || isManager) master = this;
        else zombie = GetComponent<Zombie>();

        if(zombie != null)
        {
            zombie.onLevelUp.AddListener(CheckZombie);
        }
    }

    void CheckZombie()
    {
        if (zombie == null) return;
        if(zombie.unclaimedLevelUps > 0 && !waitingForBoost)
        {
            waitingForBoost = true;
            AskBoost(this);
        }
    }

    public void ApplyBoost(Boost boost)
    {
        waitingForBoost = false;
        zombie.ClaimLevelUp(boost);
        CheckZombie();
    }

    public static Boost GenerateBoost(BoostType exclude = null)
    {
        Boost boost = new Boost();

        List<BoostType> newList = new List<BoostType>(master.boosts);
        if (exclude != null) newList.Remove(exclude);

        BoostType pickedType = Drawtype(newList);
        boost.type = pickedType.type;
        boost.amount = Random.Range(pickedType.min, pickedType.max);
        if (pickedType.integersOnly) boost.amount = Mathf.RoundToInt(boost.amount);

        return boost;
    }
    public static BoostType Drawtype (List<BoostType> poll)
    {
        int totalWeight = 0;

        foreach (BoostType type in poll) totalWeight += type.weight;

        int pickedTicket = Random.Range(0, totalWeight);


        int currentWeight = 0;

        foreach (BoostType type in poll)
        {
            if (currentWeight < type.weight + currentWeight) return type;
            currentWeight += type.weight;
        }

        return poll[poll.Count-1];
    }

    public static BoostType GetBoostTypeByType(Types type)
    {
        foreach(BoostType boostType in master.boosts)
        {
            if (boostType.type == type) return boostType;
        }
        return null;
    }

    public static void AskBoost(LevelUp from)
    {
        Boost boostA = GenerateBoost();
        BoostType exclude = GetBoostTypeByType(boostA.type);
        Boost boostB = GenerateBoost(exclude);


        LevelUpChoice.Ask(boostA, boostB, from, master.levelUpChoicePrefab, master.transform);
    }


}