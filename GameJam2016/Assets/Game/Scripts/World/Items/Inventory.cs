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
        if(!CanAddItem)
        {
            Debug.LogWarning("Inventory full!");
            return;
        }
        Item item = ItemBank.GetItem(name);
        if (item == null)
        {
            Debug.LogWarning("Cannot give " + name + " to empire. The item does not exist.");
            return;
        }

        item.SetEmpire(myEmpire);

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
    public int CountOf(string name)
    {
        int count = 0;
        for(int i = 0; i < itemsName.Count; i++)
        {
            if (itemsName[i] == name)
                count++;
        }
        return count;
    }

    public int ItemCount { get { return itemsName.Count; } }

    public bool CanAddItem { get { return ItemCount < maxItemSlot; } }

    /// <summary>
    /// Supprimer un item
    /// </summary>
    public void DeleteItem(int index)
    {
        behaviors[index].OnDelete();

        inventory.RemoveAt(index);
        itemsName.RemoveAt(index);
        behaviors.RemoveAt(index);
    }

    /// <summary>
    /// Utilise un item
    /// </summary>
    public void UseItem(int index, bool delete = true)
    {
        //Applique les effets de l'item
        inventory[index].Apply();

        //Fait l'event sur la behavior (s'il y a lieu)
        if (behaviors[index] != null)
            behaviors[index].OnUse();

        //Delete l'item
        if(delete)
            DeleteItem(index);
    }

    /// <summary>
    /// Remplace un item par un autre grace a leur nom
    /// </summary>
    public void ReplaceItem(int index, string newItemName)
    {
        DeleteItem(index);
        AddItem(newItemName);
    }

    /// <summary>
    /// Regénère les liens avec les buildings (seulement appelé lors du OnLoad())
    /// </summary>
    private void ReAdd(string name)
    {
        Item newItem = ItemBank.GetItem(name); // Trouver le building
        newItem.SetEmpire(myEmpire);
        inventory.Add(newItem); // L'ajouter a la liste de building construit
    }
}