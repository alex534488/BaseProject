using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectiles : MonoBehaviour {

    //À retravailler pour rendre encore plus générique et facile d'utilisation

    private Vector3 direction;
    public float vitesse;
    public LayerMask mask; // Le projectile ne doit pas frapper ceci
    private static List<Projectiles> inactiveProjectiles = new List<Projectiles>();
    private float timer = 0;
    public float timeAlive = 4; // Cooldown

    // Fonction appeler par des classes externes pour tirer
    public static void Shoot(Vector3 direction, Vector3 position, GameObject projectileprefab)
    {
        Projectiles launchedProjectiles;
        if (inactiveProjectiles.Count == 0)
        {
            launchedProjectiles = Instantiate(projectileprefab.gameObject).GetComponent<Projectiles>();
        }
        else
        {
            launchedProjectiles = inactiveProjectiles[0];
            inactiveProjectiles.RemoveAt(0);
        }
        launchedProjectiles.Init(direction, position);
    }

    public void Init(Vector3 direction, Vector3 position)
    {
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
            inactiveProjectiles.Add(this);
            timer = 0;
        } else
        {
            timer += Time.deltaTime;
        }
	}

    void Hit(Collider col)
    {
        // Le projectile a frapper quelques choses...

        gameObject.SetActive(false);
        inactiveProjectiles.Add(this);
    }
}
