using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CCC.Utility;
#if UNITY_EDITOR
using UnityEditor;
using CCC.EditorUtil;
#endif

namespace Game.Characters
{
    [CreateAssetMenu(menuName = "Character/CharacterGenerator")]
    public class CharacterGenerator : ScriptableObject, IKitMaker
    {

        [System.Serializable]
        public class ColorRange
        {
            public Color min = Color.white;
            public Color max = Color.white;
            public Color GetColor()
            {
                float minH = 0;
                float maxH = 0;
                float minS = 0;
                float maxS = 0;
                float minV = 0;
                float maxV = 0;
                Color.RGBToHSV(min, out minH, out minS, out minV);
                Color.RGBToHSV(max, out maxH, out maxS, out maxV);

                return Random.ColorHSV(minH, maxH, minS, maxS, minV, maxV);
            }
        }
        [System.Serializable]
        public class ColoredSprite
        {
            public Sprite sprite;
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
        public Sprite[] cheveux;
        public Sprite[] tetes;
        public ColoredSprite[] corps;
        public Sprite[] bras;
        public ColorRange skinRange;
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
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            CharacterGenerator bank = target as CharacterGenerator;

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
            bank.tempBankObj = EditorGUILayout.ObjectField(bank.tempBankObj, typeof(CharacterGenerator));
            bank.tempCustomKitObj = EditorGUILayout.ObjectField(bank.tempCustomKitObj, typeof(CustomKit));

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
                    EditorUtility.SetDirty(bank);
                }
            }
        }
    }
#endif

}