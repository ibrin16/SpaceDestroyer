/// <summary>
/// Purpose: Generate the main menu and provide the interactive feature 
/// associated with a menu item
/// Written by: Isacc Brinkman and Daya Shrestha
/// Game Development: Programming and Practice B
/// DIS, Spring 2019
/// Some parts borrowed from Benno Lüders' code
/// </summary>

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

