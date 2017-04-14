using UnityEngine;
using System.Collections;
using CCC.Utility;
using System.Collections.Generic;
using FullInspector;

public abstract class Bank<T> : Singleton<Bank<T>>
{
    [InspectorDisabled(), InspectorOrder(0)]
    public Dictionary<string, T> bank = new Dictionary<string, T>(); // Liste des items disponible

    [fiInspectorOnly, InspectorOrder(1), InspectorHeader("Editing")]
    public List<T> addList = new List<T>(); // Liste des items a ajouter
    [fiInspectorOnly, InspectorOrder(2.5f), InspectorMargin(5)]
    public List<T> removeList = new List<T>(); // Liste des items a ajouter
    [fiInspectorOnly, InspectorOrder(3)]
    public string removeElement;

    [InspectorButton(), InspectorOrder(2), InspectorHideIf("HidePushList")]
    private void PushList()
    {
        if (!Application.isEditor)
            return;

        foreach (T item in addList)
        {
            LocalAdd(item);
        }
        addList.Clear();
    }
    [InspectorButton(), InspectorOrder(4), InspectorHideIf("HideRemoveFromList")]
    private void RemoveFromList()
    {
        if (!Application.isEditor)
            return;
        if (LocalRemove(removeElement))
            removeElement = "";
        foreach (T item in removeList)
        {
            LocalRemove(item);
        }
        removeList.Clear();
    }
    [InspectorButton(), InspectorOrder(5)]
    private void RebuildBankIds()
    {
        if (!Application.isEditor)
            return;
        Dictionary<string, T> newBank = new Dictionary<string, T>(bank.Count);
        foreach (T item in bank.Values)
        {
            string id = Convert(item);
            if (string.IsNullOrEmpty(id))
                Debug.LogWarning("Id is empty. Target item: " + item.ToString());
            else if (newBank.ContainsKey(id))
                Debug.LogWarning("Id is already taken. Target item: " + item.ToString());
            else
                newBank.Add(id, item);
        }
        bank = newBank;
    }

    private bool HidePushList()
    {
        return addList.Count <= 0;
    }
    private bool HideRemoveFromList()
    {
        return removeElement == "" && removeList.Count <= 0;
    }

    private bool LocalRemove(string id)
    {
        if (string.IsNullOrEmpty(id))
            return false;
        return bank.Remove(id);
    }
    private bool LocalRemove(T obj)
    {
        return bank.Remove(Convert(obj));
    }
    private void LocalAdd(T item)
    {
        string id = Convert(item);
        if (string.IsNullOrEmpty(id))
            Debug.LogWarning("Id is empty. Target item: " + item.ToString());
        else if (bank.ContainsKey(id))
            Debug.LogWarning("Id is already taken. Target item: " + item.ToString());
        else
            bank.Add(id, item);
    }

    private bool LocalHas(string id)
    {
        return bank.ContainsKey(id);
    }
    private bool LocalHas(T obj)
    {
        return bank.ContainsValue(obj);
    }
    private T LocalGet(string id)
    {
        if (LocalHas(id))
            return bank[id];
        else
            return default(T);
    }
    private T LocalGetRandom()
    {
        int index = Random.Range(0, bank.Count);
        Dictionary<string, T>.ValueCollection.Enumerator enumerator = bank.Values.GetEnumerator();

        int i = 0;
        while(enumerator.MoveNext())
        {
            if (i == index)
                return enumerator.Current;
            i++;
        }

        return default(T);
    }

    abstract protected string Convert(T obj);

    static public T Get(string id) { return instance.LocalGet(id); }
    static public void Add(T item) { instance.LocalAdd(item); }
    static public bool Remove(string id) { return instance.LocalRemove(id); }
    static public bool Remove(T item) { return instance.LocalRemove(item); }
    static public bool Has(string id) { return instance.LocalHas(id); }
    static public bool Has(T item) { return instance.LocalHas(item); }
    static public T GetRandom() { return instance.LocalGetRandom(); }
}
