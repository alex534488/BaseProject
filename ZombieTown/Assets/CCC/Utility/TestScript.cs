using UnityEngine;
using System.Collections;
using System;
using CCC.Manager;
using CCC.Utility;

public class TestScript : MonoBehaviour
{
    public GameObject bullet;
    public int damage;
    public float vitesse;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Bullet.Shoot(damage, transform.forward, transform.position, bullet);
        }

    }
}
