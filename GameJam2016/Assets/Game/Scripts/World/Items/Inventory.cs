using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class Inventory : INewDay
{
    private Empire myEmpire;
    private int maxItemSlot;

    // Holy trinity
    [System.NonSerialized]
    private List<Item> inventory = new List<Item>(); // Liste de buildings construit dans le village
    private List<string> itemsName = new List<string>(); // Liste des NOMS de buildings construit dans le village
    private List<ItemBehavior> behaviors = new List<ItemBehavior>();// Dictionnaire contenant tout les behavior particuliere de building

    //Appelé lorsqu'on load une partie 
    [OnDeserialized]
    public void OnLoad(StreamingContext context)
    {
        inventory = new List<Item>();
        foreach (string name in itemsName)
            ReAdd(name);
    }

    public Inventory(Empire myEmpire, int maxItemSlot)
    {
        this.myEmpire = myEmpire;
        this.maxItemSlot = maxItemSlot;
    }

    /// <summary>
    /// Faire passer l'inventaire a une nouvelle journee
    /// </summary>
    public void NewDay()
    {
        for(int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].HasBehavior())
            {
                inventory[i].OnNewDay();
                behaviors[i].OnNewDay();
            }
        }
    }

    /// <summary>
    /// Ajout d'un item dans l'inventaire
    /// </summary>
    public void AddItem(string name)
    {
        Item item = ItemBank.GetItem(name);
        if (item == null)
        {
            Debug.LogWarning("Cannot give " + name + " to empire. The item does not exist.");
            return;
        }

        inventory.Add(item);
        itemsName.Add(name);

        if (item.HasBehavior())
        {
            ItemBehavior behavior = item.CreateBehavior();
            behaviors.Add(behavior);
            behavior.OnGet();
        } else
        {
            behaviors.Add(null);
        }
    }

    /// <summary>
    /// Es ce que l'item existe dans l'inventaire
    /// </summary>
    public bool Has(string name)
    {
        for(int i = 0; i < itemsName.Count; i++)
        {
            if (itemsName[i] == name) return true;
        }
        return false;
    }

    /// <summary>
    /// Supprimer un item
    /// </summary>
    public void DeleteItem(string name)
    {
        inventory.Remove(GetItemInInventoryByName(name));
        itemsName.Remove(name);
        behaviors.Remove(GetItemBehaviorByName(name));
        GetItemBehaviorByName(name).OnNewDay();
    }


    /// <summary>
    /// Utilise un item
    /// </summary>
    public void UseItem(string name, bool delete = true)
    {
        Item item = GetItemInInventoryByName(name);
        if (item == null) return;

        item.Apply(myEmpire);

        if (item.HasBehavior()) GetItemBehaviorByName(name).OnUse();

        if(delete) DeleteItem(name);
    }

    /// <summary>
    /// Remplace un item par un autre grace a leur nom
    /// </summary>
    public void ReplaceItem(string name, string newItem)
    {
        Item item = ItemBank.GetItem(name);
        if (item == null)
        {
            Debug.LogWarning("Cannot give " + name + " to empire. The item does not exist.");
            return;
        }

        bool foundIt = false;
        int index = 0;
        // Searching for the item to replace
        for (int i = 0; i < itemsName.Count; i++)
        {
            if (itemsName[i] == name)
            {
                foundIt = true;
                itemsName[i] = newItem;
                inventory[i] = item;
                index = i;
                break;
            }
        }
        if (foundIt)
        {
            if (item.HasBehavior())
            {
                ItemBehavior behavior = item.CreateBehavior();
                behaviors[index] = behavior;
                if (inventory[index].HasBehavior()) GetItemBehaviorByName(name).OnDelete();
                behavior.OnGet();
            }
            else
            {
                behaviors[index] = null;
            }
        }
    }

    /// <summary>
    /// Retourne l'item grace a son nom
    /// </summary>
    public Item GetItemInInventoryByName(string name)
    {
        for (int i = 0; i < itemsName.Count; i++)
        {
            if (itemsName[i] == name) return inventory[i];
        }
        return null;
    }

    /// <summary>
    /// Retourne un itemBehavior grace au nom de l'item
    /// </summary>
    public ItemBehavior GetItemBehaviorByName(string name)
    {
        for (int i = 0; i < itemsName.Count; i++)
        {
            if (itemsName[i] == name) return behaviors[i];
        }
        return null;
    }

    /// <summary>
    /// Regénère les liens avec les buildings (seulement appelé lors du OnLoad())
    /// </summary>
    private void ReAdd(string name)
    {
        Item newItem = ItemBank.GetItem(name); // Trouver le building
        inventory.Add(newItem); // L'ajouter a la liste de building construit
    }
}
