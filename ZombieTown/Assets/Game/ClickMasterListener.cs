using UnityEngine;
using System.Collections;

public class ClickMasterListener : MonoBehaviour {

    public float maxDist = 15;
    public LayerMask zombieMask;
    public LayerMask copAndTerrainMask;
    public static OnEntityClick currentlySelected = null;

    void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Collider col = CheckHit(zombieMask).collider;

            currentlySelected.UnSelect();
            currentlySelected = null;
            OnEntityClick entity = col.GetComponent<OnEntityClick>();
            if (entity != null && entity.clickable)
            {
                currentlySelected = entity;
                currentlySelected.Select();
            }
        }
        else if (currentlySelected != null && Input.GetMouseButtonDown(1))
        {
            RaycastHit hit = CheckHit(copAndTerrainMask);
            Collider col = hit.collider;

            //MoveOrder
            if(col.tag == "Map")
            {
                print("move");
                //Set to Move to
                //currentlySelected.GetComponent<Personnage>().comportement.ChangeState();
            }
            else //AttackOrder
            {
                //Set to Attack to
            }
        }
    }

    RaycastHit CheckHit(LayerMask mask)
    {
        Vector3 v3 = Input.mousePosition;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        Ray ray = new Ray(v3, Camera.main.transform.forward);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, maxDist, mask);
        return hit;
    }
}
