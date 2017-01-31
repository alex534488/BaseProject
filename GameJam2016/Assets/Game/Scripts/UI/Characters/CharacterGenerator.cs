using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CCC.Utility;
#if UNITY_EDITOR
using UnityEditor;
using CCC.EditorUtil;
#endif

[CreateAssetMenu(menuName = "Character/CharacterGenerator")]
public class CharacterGenerator : ScriptableObject, IKitMaker
{
    [System.Serializable]
    public class ColorRange
    {
        public Color min = Color.white;
        public Color max = Color.white;
        public SerializableColor GetColor()
        {
            float minH = 0;
            float maxH = 0;
            float minS = 0;
            float maxS = 0;
            float minV = 0;
            float maxV = 0;
            Color.RGBToHSV(min, out minH, out minS, out minV);
            Color.RGBToHSV(max, out maxH, out maxS, out maxV);

            return new SerializableColor(Random.ColorHSV(minH, maxH, minS, maxS, minV, maxV));
        }
    }
    [System.Serializable]
    public class ColoredSprite
    {
        public int sprite;
        public ColorRange range;
    }


    public string tag;

    [HideInInspector]
    public List<ScriptableObject> subBanks = new List<ScriptableObject>();
    [HideInInspector]
    public Object tempBankObj;
    [HideInInspector]
    public Object tempCustomKitObj;

    [Header("Content")]
    public int[] cheveux;
    public int[] tetes;
    public ColoredSprite[] corps;//C38989FF
    public int[] bras;
    public ColorRange skinRange;//F14F51FF
    public ColorRange cheveuxRange;

    public IKit MakeKit()
    {
        Lottery lottery = new Lottery();
        foreach (IKitMaker kitMaker in subBanks)
        {
            lottery.Add(kitMaker);
        }
        lottery.Add(this, LocalWeight());

        IKitMaker result = lottery.Pick() as IKitMaker;
        if (result as Object == this)
            return MakeLocalKit();
        else
            return result.MakeKit();
    }

    IKit MakeLocalKit()
    {
        Kit kit = new Kit();

        int corpsIndex = 0;
        if (corps.Length != 1)
            corpsIndex = Random.Range(0, corps.Length);

        int cheveuxIndex = 0;
        if (cheveux.Length != 1)
            cheveuxIndex = Random.Range(0, cheveux.Length);

        int teteIndex = 0;
        if (tetes.Length != 1)
            teteIndex = Random.Range(0, tetes.Length);

        kit.hair = cheveux[cheveuxIndex];
        kit.head = tetes[teteIndex];

        ColoredSprite leCorps = this.corps[corpsIndex];
        kit.body = leCorps.sprite;
        kit.arms = bras[corpsIndex];

        kit.skinColor = skinRange.GetColor();
        kit.hairColor = cheveuxRange.GetColor();
        kit.bodyColor = leCorps.range.GetColor();

        return kit;
    }

    public string Tag()
    {
        return tag;
    }

    public int Weight()
    {
        int weight = LocalWeight();

        if (subBanks != null && subBanks.Count >= 1)
            foreach (IKitMaker kitMaker in subBanks)
                weight += kitMaker.Weight();

        return weight;
    }

    int LocalWeight()
    {
        int weight = cheveux.Length;
        weight += tetes.Length;
        weight += corps.Length;
        return weight;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CharacterGenerator))]
public class CharacterBankEditor : AdvEditor
{
    bool hairFold = false;
    Sprite[] hair;
    bool headsFold = false;
    Sprite[] heads;
    bool bodiesFold = false;
    Sprite[] bodies;
    bool armsFold = false;
    Sprite[] arms;
    CharacterGenerator bank;

    public void OnEnable()
    {
        bank = target as CharacterGenerator;

        hair = new Sprite[bank.cheveux.Length];
        for (int i = 0; i < hair.Length; i++)
        {
            hair[i] = GetSpriteFrom(bank.cheveux[i]);
        }
        heads = new Sprite[bank.tetes.Length];
        for (int i = 0; i < heads.Length; i++)
        {
            heads[i] = GetSpriteFrom(bank.tetes[i]);
        }
        bodies = new Sprite[bank.corps.Length];
        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i] = GetSpriteFrom(bank.corps[i].sprite);
        }
        arms = new Sprite[bank.bras.Length];
        for (int i = 0; i < arms.Length; i++)
        {
            arms[i] = GetSpriteFrom(bank.bras[i]);
        }
    }

    public override void OnInspectorGUI()
    {
        bank = target as CharacterGenerator;

        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        DisplaySpritedVersion();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Sub-banks", bold);
        if (bank.subBanks == null)
            bank.subBanks = new List<ScriptableObject>();
        foreach (IKitMaker kitMaker in bank.subBanks)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("X", GUILayout.Width(18), GUILayout.Height(18)))
            {
                bank.subBanks.Remove(kitMaker as ScriptableObject);
                break;
            }
            EditorGUILayout.LabelField("     " + kitMaker.Tag());
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Add", bold);
        bank.tempBankObj = EditorGUILayout.ObjectField(bank.tempBankObj, typeof(CharacterGenerator), false);
        bank.tempCustomKitObj = EditorGUILayout.ObjectField(bank.tempCustomKitObj, typeof(CustomKit), false);

        if (bank.tempBankObj != null || bank.tempCustomKitObj != null)
        {
            if (GUILayout.Button("Add", GUILayout.Width(75)))
            {
                if (bank.tempBankObj != null)
                {
                    bank.subBanks.Add(bank.tempBankObj as ScriptableObject);
                    bank.tempBankObj = null;
                }
                if (bank.tempCustomKitObj != null)
                {
                    bank.subBanks.Add(bank.tempCustomKitObj as ScriptableObject);
                    bank.tempCustomKitObj = null;
                }
            }
        }
        EditorUtility.SetDirty(bank);
    }

    void DisplaySpritedVersion()
    {
        EditorGUILayout.LabelField("Content w/ Sprites", bold);

        Sprite temp = null;

        if (hairFold = EditorGUILayout.Foldout(hairFold, "Cheveux"))
            for (int i = 0; i < hair.Length; i++)
            {
                temp = hair[i];
                hair[i] = DisplaySprite("Hair", hair[i]);
                if (hair[i] != temp)
                    ApplySpriteTo(ref hair[i], ref bank.cheveux[i]);
            }
        if (headsFold = EditorGUILayout.Foldout(headsFold, "Têtes"))
            for (int i = 0; i < heads.Length; i++)
            {
                temp = heads[i];
                heads[i] = DisplaySprite("Head", heads[i]);
                if (heads[i] != temp)
                    ApplySpriteTo(ref heads[i], ref bank.tetes[i]);
            }
        if (bodiesFold = EditorGUILayout.Foldout(bodiesFold, "Corps"))
            for (int i = 0; i < bodies.Length; i++)
            {
                temp = bodies[i];
                bodies[i] = DisplaySprite("Body", bodies[i]);
                if (bodies[i] != temp)
                    ApplySpriteTo(ref bodies[i], ref bank.corps[i].sprite);
            }
        if (armsFold = EditorGUILayout.Foldout(armsFold, "Bras"))
            for (int i = 0; i < arms.Length; i++)
            {
                temp = arms[i];
                arms[i] = DisplaySprite("Arm", arms[i]);
                if (arms[i] != temp)
                    ApplySpriteTo(ref arms[i], ref bank.bras[i]);
            }
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