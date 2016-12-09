using UnityEngine;
using System.Collections;
using Game.Characters;

[CreateAssetMenu(menuName = "Character/Custom Kit")]
public class CustomKit : ScriptableObject, IKit, IKitMaker
{
    public string tag;
    public Kit kit;

    public Sprite Head()
    {
        return kit.Head();
    }

    public Sprite Body()
    {
        return kit.Body();
    }

    public Sprite Arms()
    {
        return kit.Arms();
    }

    public Sprite Hair()
    {
        return kit.Hair();
    }

    public Color SkinColor()
    {
        return kit.SkinColor();
    }

    public Color HairColor()
    {
        return kit.HairColor();
    }

    public Color BodyColor()
    {
        return kit.BodyColor();
    }

    public IKit MakeKit()
    {
        return kit;
    }

    public string Tag()
    {
        return tag;
    }

    public int Weight()
    {
        return 3; //1 pour la tete, 1 pour le corps, 1 pour les cheveux
    }
}
