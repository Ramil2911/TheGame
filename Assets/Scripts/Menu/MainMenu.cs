using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    public void LoadScene(int idScene)
    {
        SceneManager.LoadScene(idScene);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
