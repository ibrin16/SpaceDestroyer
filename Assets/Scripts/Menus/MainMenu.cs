using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script to manage the main menu. Loads scenes and quits the game.
/// </summary>
public class MainMenu : MonoBehaviour
{
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

