using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour {

    private Vector3 direction;
    private int damage;
    public static float vitesse = 3; // Vitesse des balles
    public LayerMask mask;
    private static List<Bullet> inactiveBullets = new List<Bullet>();

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
        transform.position += vitesse * transform.forward * Time.deltaTime;
	}

    void OnTriggerEnter(Collider collider)
    {
        if ((mask.value & 1 << collider.gameObject.layer) != 0) { return; }

        Zombie zombie = collider.GetComponent<Zombie>();

        zombie.LoseHP(damage);

        gameObject.SetActive(false);
        inactiveBullets.Add(this);
    }
}
