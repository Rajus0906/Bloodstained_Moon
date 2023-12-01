using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int pickupAmount = 2;

    private void OnTriggerEnter(Collider other)
    {
       
        Debug.Log("Trigger entered");

       
        Destroy(gameObject);

        
            
        
    }
}
    

