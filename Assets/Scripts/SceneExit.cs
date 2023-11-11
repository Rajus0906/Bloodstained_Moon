using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public string scenetoLoad;
    

    private void OnTriggerEnter(Collider other)
    {
        
        SceneManager.LoadScene(scenetoLoad);
    }
}
