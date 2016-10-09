using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "CharacterGenerator")]
public class CharacterGenerator : ScriptableObject {

    [System.Serializable]
    public class ColorRange
    {
        public Color from;
        public Color to;
    }
    [System.Serializable]
    public class ColoredSprite
    {
        public Sprite sprite;
        public ColorRange range;
    }
    public Sprite[] cheveux;
    public Sprite[] tetes;
    public ColoredSprite[] corps;
    public Sprite[] bras;
    public ColorRange skinRange;
    public ColorRange cheveuxRange;

    public class Kit
    {
        public Sprite tete;
        public Sprite corps;
        public Sprite bras;
        public Sprite cheveux;
        public Color skinColor;
        public Color corpsColor;
        public Color cheveuxColor;
    }

    public Kit Generate()
    {
        Kit kit = new Kit();

        kit.cheveux = cheveux[Random.Range(0, cheveux.Length)];
        kit.tete = tetes[Random.Range(0, tetes.Length)];

        int corpsIndex = Random.Range(0, this.corps.Length);
        ColoredSprite corps = this.corps[corpsIndex];
        kit.corps = corps.sprite;
        kit.bras = bras[corpsIndex];

        kit.skinColor = Color.Lerp(skinRange.from, skinRange.to, Random.Range(0f, 1f));
        kit.cheveuxColor = Color.Lerp(cheveuxRange.from, cheveuxRange.to, Random.Range(0f, 1f));
        kit.corpsColor = Color.Lerp(corps.range.from, corps.range.to, Random.Range(0f, 1f));

        return kit;
    }
}
