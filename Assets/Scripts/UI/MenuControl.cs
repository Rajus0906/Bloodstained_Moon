using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); //this will have the name of your main game scene
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene("TutorialScene"); //this will have the name of your main game scene
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Quit()
    {
        SceneManager.LoadScene("StartScreen");
    }


    public void ExitGame()
    {
        Application.Quit();
    }

}
