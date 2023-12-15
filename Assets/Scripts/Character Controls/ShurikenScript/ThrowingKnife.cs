using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;

public class ThrowingKnife : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    public TMP_Text totalThrowsLeft;
    

    [Header("Settings")]
    public int totalThrows = 10;
    public float throwCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public float throwForce;
    public float throwUpWardsForce;

    bool readyToThrow;

    [Header("Sound Effects")]
    public AudioSource throwSound;

    private void Start()
    {
        readyToThrow = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
        {
            Throw();

            throwSound.Play();
        }

        // Update throws UI
        UpdateThrowsUI();
    }

    private void Throw()
    {
        readyToThrow = false;

        //instantiate Object to Throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        //get rigidbody componet
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        //calculate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if(Physics.Raycast(cam.position,cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }


        // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpWardsForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        

        //implement throw cool down
        Invoke(nameof(ResetThrow), throwCooldown);
        
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    private void UpdateThrowsUI()
    {
        totalThrowsLeft.text = totalThrows.ToString();
    }



    public void RefillAmmo(int amount)
    {
        totalThrows += amount;
        // Update UI or perform any other actions needed
        UpdateThrowsUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        AmmoPickup ammoPickup = other.GetComponent<AmmoPickup>();

        if (ammoPickup != null)
        {
            RefillAmmo(ammoPickup.pickupAmount);
            Destroy(ammoPickup.gameObject); // Destroy the ammo pickup object
        }
    }
 }

