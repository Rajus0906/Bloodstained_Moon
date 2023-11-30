using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    

    private PlayerRespawn playerRespawn;
    // Start is called before the first frame update
    void Start()
    {
        playerRespawn = GameObject.Find("Player").GetComponent<PlayerRespawn>();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Checkpoint")
        {
            playerRespawn.respawnPoint = transform.position;
            Debug.Log("Respawn point set to: " + transform.position);
        }
    }
}
