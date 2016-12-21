using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CCC.Manager
{
    public class RoomManager : BaseManager {

        new static RoomManager instance;

        public static List<Canvas> listView = new List<Canvas>();

        protected override void Awake()
        {
            base.Awake();
            instance = this;
        }

        // Trouve l'index d'un Canvas dans la liste
        public static int FindView(Scene currentView) 
        {
            for (int i = 0; i < listView.Count; i++)
            {
                if(listView[i].name == currentView.name)
                {
                    return i;
                }
            }
            return -1;
        }

        // Trouve l'index du prochain Canvas dans la liste (direction:gauche/droite)
        public static int FindNextView(Scene currentView, int direction)
        {
            int current = FindView(currentView);
            current += direction;
            if (current < 0) return ((listView.Count-1) - ((-1 * current) - 1));
            if (current > listView.Count - 1) return (current - (listView.Count - 1));

            return current;
        }

        // Active un Canvas dans la liste grace a son index
        public static void ActivateView(int i)
        {
            if (i < 0 || i >= listView.Count) return;

            // Load Canvas dans la liste a l'index i via le SceneManager
            listView[i].gameObject.SetActive(true);
        } 
	}
}
