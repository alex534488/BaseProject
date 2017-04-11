using UnityEngine;
using System.Collections;
using CCC.Utility;
using System.Collections.Generic;

// Stock tous les items disponible dans le jeu
public class Bank<T> : Singleton<Bank<T>>
{
    private List<T> bank = new List<T>(); // Liste des items disponible

    static public T GetItem(T item)
    {
        for (int i = 0; i < instance.bank.Count; i++)
        {
            if (instance.bank[i].Equals(item))
            {
                return instance.bank[i];
            }
        }
        return default(T);
    }

    static public T GetItemByIndex(int index)
    {
        return instance.bank[index];
    }

    static public T GetRandomItem()
    {
        return new RandomList<T>(instance.bank).Pick();
    }

    static public T GetLastItem()
    {
        return instance.bank[instance.bank.Count - 1];
    }

    static public void AddItem(T item)
    {
        instance.bank.Add(item);
    }

    static public void DeleteItem(T item)
    {
        instance.bank.Remove(item);
    }

    static public void DeleteRandomItem()
    {
        instance.bank.Remove(GetRandomItem());
    }

    static public void DeleteItemByIndex(int index)
    {
        instance.bank.Remove(instance.bank[index]);
    }

    static public bool Has(T item)
    {
        for (int i = 0; i < instance.bank.Count; i++)
        {
            if (instance.bank[i].Equals(item))
            {
                return true;
            }
        }
        return false;
    }

    static public List<T> GetAllBuildings()
    {
        return instance.bank;
    }
}
