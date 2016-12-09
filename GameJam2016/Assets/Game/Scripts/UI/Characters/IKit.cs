using UnityEngine;

namespace Game.Characters
{
    public interface IKit
    {
        Sprite Head();
        Sprite Body();
        Sprite Arms();
        Sprite Hair();
        Color SkinColor();
        Color HairColor();
        Color BodyColor();
    }
}
