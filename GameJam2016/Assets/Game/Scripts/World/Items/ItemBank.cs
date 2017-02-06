using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBank : Singleton<ItemBank> {

    public List<Item> Items = new List<Item>();

    static public Item GetItem(string name)
    {
        for (int i = 0; i < instance.Items.Count; i++)
        {
            if (instance.Items[i].GetName() == name)
            {
                return Instantiate(instance.Items[i]) as Item;
            }
        }
        return null;
    }
}
