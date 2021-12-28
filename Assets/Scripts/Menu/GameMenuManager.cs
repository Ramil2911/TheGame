using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    public GameObject gameMenu;

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameMenu.activeSelf == false)
            {
                Time.timeScale = 0;
                gameMenu.SetActive(true);
                Cursor.visible = false;
            }
            else
            {
                Time.timeScale = 1;
                gameMenu.SetActive(false);
                Cursor.visible = true;
            }
        }
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ReturnToGame()
    {
        print("work");
        gameMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        
    }
}
