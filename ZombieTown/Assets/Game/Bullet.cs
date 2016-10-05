using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour {

    private Vector3 direction;
    private int damage;
    public float vitesse; // Vitesse des balles
    public LayerMask mask;
    private static List<Bullet> inactiveBullets = new List<Bullet>();
    private float timer = 0;
    public float timeAlive = 4;

    public static void Shoot(int damage, Vector3 direction, Vector3 position, GameObject bulletprefab)
    {
        Bullet launchedBullet;
        if (inactiveBullets.Count == 0)
        {
            launchedBullet = Instantiate(bulletprefab.gameObject).GetComponent<Bullet>();
        }
        else
        {
            launchedBullet = inactiveBullets[0];
            inactiveBullets.RemoveAt(0);
        }
        launchedBullet.Init(damage, direction, position);
    }

    public void Init(int damage, Vector3 direction, Vector3 position)
    {
        this.damage = damage;
        this.direction = direction;


        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.position = position;
        gameObject.SetActive(true);
    }
	
	void Update () {
        float distance = vitesse * Time.deltaTime;
        Ray ray = new Ray(transform.position,transform.forward);
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(ray, out hit, distance, mask);
        if(hit.collider != null)
        {
            Hit(hit.collider);
        }
        transform.position += transform.forward * distance;

        if(timer >= timeAlive)
        {
            gameObject.SetActive(false);
            inactiveBullets.Add(this);
            timer = 0;
        } else
        {
            timer+=Time.deltaTime;
        }
	}

    void Hit(Collider col)
    {
        //if ((mask.value & 1 << collider.gameObject.layer) != 0) { return; }

        if(col != null)
        {
            Personnage personnage = col.GetComponent<Personnage>();

            if (personnage != null) { personnage.LoseHP(damage); }
        }

        gameObject.SetActive(false);
        inactiveBullets.Add(this);
    }
}
