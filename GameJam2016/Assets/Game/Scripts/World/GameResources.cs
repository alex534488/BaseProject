using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public enum ResourceType
{
    science = 1,
    scienceProd = 2,
    gold = 3,
    goldProd = 4,
    material = 5,
    materialProd = 6,
    citizenProgress = 7,
    food = 8,
    happiness = 9,
    reputation = 10,
    armyPower = 11,
    armyCost = 12,
    citizenProgressMax = 13,
    custom = 14
}

public enum Village_ResourceType
{
    scienceProd = 1,
    goldProd = 2,
    materialProd = 3,
    food = 4,
    armyPower = 5,
    armyCost = 6,
    custom = 7
}

public enum Empire_ResourceType
{
    science = 1,
    gold = 2,
    material = 3,
    citizenProgress = 4,
    happiness = 5,
    reputation = 6,
    citizenProgressMax = 7,
    custom = 8
}

public class GameResources : MonoBehaviour
{
    public class ResourceEvent : UnityEvent<ResourceType> { }
    public Sprite scienceIcon;
    public Sprite scienceProdIcon;
    public Sprite goldIcon;
    public Sprite goldProdIcon;
    public Sprite materialIcon;
    public Sprite materialProdIcon;
    public Sprite citizenProgressIcon;
    public Sprite foodIcon;
    public Sprite happinessIcon;
    public Sprite reputationIcon;
    public Sprite armyPowerIcon;
    public Sprite armyCostIcon;
    public Sprite citizenProgressMax;

    static GameResources instance;

    void Awake()
    {
        instance = this;
    }

    public static Sprite GetIcon(ResourceType type)
    {
        if (instance == null) { Debug.LogError("Error: instance is null."); return null; }
        switch (type)
        {
            default:
            case ResourceType.science:
                return instance.scienceIcon;
            case ResourceType.scienceProd:
                return instance.scienceProdIcon;
            case ResourceType.gold:
                return instance.goldIcon;
            case ResourceType.goldProd:
                return instance.goldProdIcon;
            case ResourceType.material:
                return instance.materialIcon;
            case ResourceType.materialProd:
                return instance.materialProdIcon;
            case ResourceType.citizenProgress:
                return instance.citizenProgressIcon;
            case ResourceType.food:
                return instance.foodIcon;
            case ResourceType.happiness:
                return instance.happinessIcon;
            case ResourceType.reputation:
                return instance.reputationIcon;
            case ResourceType.armyPower:
                return instance.armyPowerIcon;
            case ResourceType.armyCost:
                return instance.armyCostIcon;
            case ResourceType.citizenProgressMax:
                return instance.citizenProgressMax;
        }
    }

    public static string ToString(ResourceType stdType)
    {
        switch (stdType)
        {
            default: return stdType.ToString();
            case ResourceType.science:
                return "Science";
            case ResourceType.scienceProd:
                return "Science Production";
            case ResourceType.gold:
                return "Gold";
            case ResourceType.goldProd:
                return "Gold Production";
            case ResourceType.material:
                return "Material";
            case ResourceType.materialProd:
                return "Material Production";
            case ResourceType.citizenProgress:
                return "Citizen Progress";
            case ResourceType.food:
                return "Food";
            case ResourceType.happiness:
                return "Happiness";
            case ResourceType.reputation:
                return "Reputation";
            case ResourceType.armyPower:
                return "Army Power";
            case ResourceType.armyCost:
                return "Army Cost";
            case ResourceType.citizenProgressMax:
                return "Citizen Progress Max";
        }
    }
}
