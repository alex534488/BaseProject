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
    public float timelapse = 60;

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

        if(timer >= timelapse)
        {
            gameObject.SetActive(false);
            inactiveBullets.Add(this);
            timer = 0;
        } else
        {
            timer++;
        }
	}

    void Hit(Collider col)
    {
        //if ((mask.value & 1 << collider.gameObject.layer) != 0) { return; }

        if(col != null)
        {
            Zombie zombie = col.GetComponent<Zombie>();

            if (zombie != null) { zombie.LoseHP(damage); }
        }

        gameObject.SetActive(false);
        inactiveBullets.Add(this);
    }
}
