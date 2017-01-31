using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using CCC.EditorUtil;
#endif

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

#if UNITY_EDITOR
[CustomEditor(typeof(CustomKit))]
public class CustomKitEditor : AdvEditor
{
    Sprite head;
    Sprite hair;
    Sprite body;
    Sprite arms;

    CustomKit kit;

    public void OnEnable()
    {
        kit = target as CustomKit;
        hair = GetSpriteFrom(kit.kit.hair);
        head = GetSpriteFrom(kit.kit.head);
        body = GetSpriteFrom(kit.kit.body);
        arms = GetSpriteFrom(kit.kit.arms);
    }

    public override void OnInspectorGUI()
    {
        kit = target as CustomKit;

        kit.tag = EditorGUILayout.TextField("Tag", kit.tag);

        EditorGUILayout.LabelField("Kit", bold);

        Sprite temp = hair;
        hair = DisplaySprite("Hair", hair);
        if(hair != temp)
            ApplySpriteTo(ref hair, ref kit.kit.hair);

        temp = head;
        head = DisplaySprite("Head", head);
        if (head != temp)
            ApplySpriteTo(ref head, ref kit.kit.head);

        temp = body;
        body = DisplaySprite("Body", body);
        if (body != temp)
            ApplySpriteTo(ref body, ref kit.kit.body);

        temp = arms;
        arms = DisplaySprite("Arms", arms);
        if (arms != temp)
            ApplySpriteTo(ref arms, ref kit.kit.arms);

        kit.kit.skinColor = new SerializableColor(EditorGUILayout.ColorField("Skin Color", kit.kit.skinColor));
        kit.kit.bodyColor = new SerializableColor(EditorGUILayout.ColorField("Body Color", kit.kit.bodyColor));
        kit.kit.hairColor = new SerializableColor(EditorGUILayout.ColorField("Hair Color", kit.kit.hairColor));
        EditorUtility.SetDirty(kit);
    }

    void ApplySpriteTo(ref Sprite sprite, ref int index)
    {
        if (sprite != null)
        {
            try
            {
                index = SpriteBank.GetIndex(sprite);
            }
            catch (System.Exception error)
            {
                Debug.LogError(error.Message);
                sprite = null;
            }
        }
    }

    Sprite GetSpriteFrom(int index)
    {
        if (index < 0)
            return null;
        return SpriteBank.GetSprite(index);
    }

    Sprite DisplaySprite(string label, Sprite sprite)
    {
        return EditorGUILayout.ObjectField(label + "(" + (sprite != null ? sprite.name : "null") + ")", sprite, typeof(Sprite), false) as Sprite;
    }
}
#endif