using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RoomManager : Singleton<RoomManager>
{
    public List<Canvas> listView;

    public Canvas currentView;

    static public bool DoesViewExist(int i)
    {
        if (i < 0 || (i > (instance.listView.Count - 1))) return false;
        return true;
    }

    // Trouve l'index d'un Canvas dans la liste
    static public int FindView(Canvas currentView)
    {
        for (int i = 0; i < instance.listView.Count ; i++)
        {
            if (instance.listView[i].name == currentView.name)
            {
                return i;
            }
        }
        return -1;
    }

    // Trouve l'index du prochain Canvas dans la liste (direction:gauche/droite)
    static public int FindNextView(Canvas currentView, int direction)
    {
        int current = FindView(currentView);
        current += direction; 
        if (!DoesViewExist(current)) return -1;
        return current;
    }

    // Active un Canvas dans la liste grace a son index
    static public void ActivateView(int i)
    {
        if (!DoesViewExist(i)) return;

        // Load Canvas dans la liste a l'index i via le SceneManager
        instance.listView[i].gameObject.SetActive(true);
    }

    static public void DeactivateView(int i)
    {
        if (!DoesViewExist(i)) return;

        // Load Canvas dans la liste a l'index i via le SceneManager
        instance.listView[i].gameObject.SetActive(false);
    }

    static public List<Canvas> GetListView()
    {
        return instance.listView;
    }

    static public Canvas GetView()
    {
        return instance.currentView;
    }

    static public void SetView(Canvas newView)
    {
        instance.currentView = newView;
    }
}
