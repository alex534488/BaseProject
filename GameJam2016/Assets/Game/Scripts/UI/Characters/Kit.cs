using UnityEngine;
using CCC.Utility;
using System.Collections;

[System.Serializable]
public class Kit : IKit
{
    public int head = -1;
    public int body = -1;
    public int arms = -1;
    public int hair = -1;
    public SerializableColor skinColor;
    public SerializableColor bodyColor;
    public SerializableColor hairColor;

    public Sprite Head()
    {
        return SpriteBank.GetSprite(head);
    }

    public Sprite Body()
    {
        return SpriteBank.GetSprite(body);
    }

    public Sprite Arms()
    {
        return SpriteBank.GetSprite(arms);
    }

    public Sprite Hair()
    {
        return SpriteBank.GetSprite(hair);
    }

    public Color SkinColor()
    {
        return skinColor;
    }

    public Color HairColor()
    {
        return hairColor;
    }

    public Color BodyColor()
    {
        return bodyColor;
    }
}
