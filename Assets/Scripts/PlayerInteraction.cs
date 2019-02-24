using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    public int health;
    public static PlayerInteraction instance;
    //public GameObject equipedGun;



    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Hit(int damage)
    {
        // need to also implement the health bar changing
        health -= damage;
        UIHealthPanel.instance.UpdateHealth();

        if (health <= 0)
        {
            StartCoroutine(RestartTheGameAfterSeconds(1));
            print("death");
            // reload current scene
        }

    }
    IEnumerator RestartTheGameAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
