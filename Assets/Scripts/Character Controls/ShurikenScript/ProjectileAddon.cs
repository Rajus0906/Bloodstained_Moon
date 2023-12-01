using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAddon : MonoBehaviour
{
    public int damage;

    public float lifetime = 5f;

    private Rigidbody rb;

    private bool targetHit;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Destroy the projectile after the specified lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(targetHit)
        {
            return;
        } 
        else
        {
            targetHit = true;
        }

        //check if hit enemy
        if(collision.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            enemy.TakeDamage(damage);

            Destroy(gameObject);
        }

        // Makes sure projectile sticks to surface
        rb.isKinematic = true;

        //make sure projectile moves with target
        transform.SetParent(collision.transform);
    }

}
