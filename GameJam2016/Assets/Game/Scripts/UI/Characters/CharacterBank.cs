using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CCC.Utility;

public class CharacterBank : Singleton<CharacterBank>
{
    public class StandardTags
    {
        private StandardTags(string value) { Value = value; }

        public string Value { get; set; }

        public static StandardTags Soldier { get { return new StandardTags("soldier"); } }
        public static StandardTags Beggar { get { return new StandardTags("beggar"); } }
        public static StandardTags Philosopher { get { return new StandardTags("philosopher"); } }
        public static StandardTags Citizen { get { return new StandardTags("citizen"); } }
        public static StandardTags NotSoldier { get { return new StandardTags("not soldier"); } }
    }
    [Tooltip("Ces kits peuvent être choisient lors de GetRandomKit()")]
    public List<CharacterGenerator> baseKits;
    [Tooltip("Ces kits ne peuvent pas être choisient lors de GetRandomKit()")]
    public List<CharacterGenerator> otherKits;

    static public IKit GetRandomKit()
    {
        if (instance.baseKits.Count <= 0)
        {
            Debug.LogError("Insert at least one standard kit in the generator");
            return null;
        }

        Lottery lottery = new Lottery();
        foreach (IKitMaker kitMaker in instance.baseKits)
        {
            lottery.Add(kitMaker);
        }
        return (lottery.Pick() as IKitMaker).MakeKit();
    }

    static public IKit GetKit(string tag)
    {
        foreach (IKitMaker kit in instance.baseKits)
        {
            if (kit.Tag() == tag)
                return kit.MakeKit();
        }
        foreach (IKitMaker kit in instance.otherKits)
        {
            if (kit.Tag() == tag)
                return kit.MakeKit();
        }
        return null;
    }

    static public IKit GetKit(StandardTags tag)
    {
        return GetKit(tag.Value);
    }
}
