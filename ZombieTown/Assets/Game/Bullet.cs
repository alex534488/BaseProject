using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public Vector3 direction;
    public int damage;
    public float vitesse;
    public LayerMask mask;

    public void Init(int damage, Vector3 direction, float vitesse)
    {
        this.damage = damage;
        this.direction = direction;
        this.vitesse = vitesse;

        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    void Start ()
    {
	    
	}
	
	void Update () {
        transform.position += vitesse * transform.forward * Time.deltaTime;
	}

    void OnTriggerEnter(Collider collider)
    {

        if (mask != (mask | (1 << collider.gameObject.layer))) { return; }

        Zombie zombie = collider.GetComponent<Zombie>();

        zombie.LoseHP(damage);

        Destroy(this);
    }
}
