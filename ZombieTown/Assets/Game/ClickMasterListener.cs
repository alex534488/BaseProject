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

            UnSelect();
            if (col == null) return;

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
            if (col == null) return;
            Personnage personnage = currentlySelected.GetComponent<Personnage>();
            if (personnage == null) return;

            //MoveOrder
            if (col.tag == "Map")
            {
                //Set to Move to
                personnage.comportement.ChangeState<StatesMoveTo>();
                (personnage.comportement.currentStates as StatesMoveTo).Init(hit.point);
            }
            else //AttackOrder
            {
                //Set to Attack to
                currentlySelected.GetComponent<Personnage>().comportement.ChangeState<StatesAttack>();
                (personnage.comportement.currentStates as StatesAttack).SetTarget(personnage);
            }

            UnSelect();
        }
    }

    void UnSelect()
    {
        if(currentlySelected != null)
        {
            currentlySelected.UnSelect();
            currentlySelected = null;
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
