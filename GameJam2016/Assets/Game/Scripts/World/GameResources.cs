using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public enum Resource_Type
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
    armyCost = 12
}

public class GameResources : MonoBehaviour
{
    public class ResourceEvent : UnityEvent<Resource_Type> { }
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

    static GameResources instance;

    void Awake()
    {
        instance = this;
    }

    public static Sprite GetIcon(Resource_Type type)
    {
        if (instance == null) { Debug.LogError("Error: instance is null."); return null; }
        switch (type)
        {
            default:
            case Resource_Type.science:
                return instance.scienceIcon;
            case Resource_Type.scienceProd:
                return instance.scienceProdIcon;
            case Resource_Type.gold:
                return instance.goldIcon;
            case Resource_Type.goldProd:
                return instance.goldProdIcon;
            case Resource_Type.material:
                return instance.materialIcon;
            case Resource_Type.materialProd:
                return instance.materialProdIcon;
            case Resource_Type.citizenProgress:
                return instance.citizenProgressIcon;
            case Resource_Type.food:
                return instance.foodIcon;
            case Resource_Type.happiness:
                return instance.happinessIcon;
            case Resource_Type.reputation:
                return instance.reputationIcon;
            case Resource_Type.armyPower:
                return instance.armyPowerIcon;
            case Resource_Type.armyCost:
                return instance.armyCostIcon;
        }
    }

    public static string ToString(Resource_Type stdType)
    {
        switch (stdType)
        {
            default: return stdType.ToString();
            case Resource_Type.science:
                return "Science";
            case Resource_Type.scienceProd:
                return "Science Production";
            case Resource_Type.gold:
                return "Gold";
            case Resource_Type.goldProd:
                return "Gold Production";
            case Resource_Type.material:
                return "Material";
            case Resource_Type.materialProd:
                return "Material Production";
            case Resource_Type.citizenProgress:
                return "Citizen Progress";
            case Resource_Type.food:
                return "Food";
            case Resource_Type.happiness:
                return "Happiness";
            case Resource_Type.reputation:
                return "Reputation";
            case Resource_Type.armyPower:
                return "Army Power";
            case Resource_Type.armyCost:
                return "Army Cost";
        }
    }
}
