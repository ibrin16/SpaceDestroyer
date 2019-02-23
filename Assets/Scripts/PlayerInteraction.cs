using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    public int health;
    public static PlayerInteraction instance;
    public bool[] gunsOn;
    public GameObject equipedGun;



    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;

    }

    void Start()
    {
        gunsOn = new bool[] { false, false, false, false, false, false };

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // need to check if gun 0 is equipped
        // then check if gun 1 is equipped
        // then check if gun 2 is equipped

        if (collision.CompareTag("Pistol"))
        {
            if (Gun.instance.equiped)
            {
                gunsOn[0] = true;
                Destroy(collision.gameObject);
                Gun.instance.myGuns[0] = 0;
            }
            else
            {
                gunsOn[0] = true;
                equipedGun = collision.gameObject;
                equipedGun.transform.SetParent(transform);
                Gun.instance.equiped = true;
                Gun.instance.myGuns[0] = 0;
            }

        }
        if (collision.CompareTag("Shotgun"))
        {
            gunsOn[1] = true;
            Destroy(collision.gameObject);
            Gun.instance.myGuns[0] = 1;
        }
        if (collision.CompareTag("AR"))
        {
            gunsOn[1] = true;
            Destroy(collision.gameObject);
            Gun.instance.myGuns[0] = 2;
        }
        if (collision.CompareTag("Better AR"))
        {
            gunsOn[1] = true;
            Destroy(collision.gameObject);
            Gun.instance.myGuns[0] = 3;
        }
        if (collision.CompareTag("RPG"))
        {
            gunsOn[1] = true;
            Destroy(collision.gameObject);
            Gun.instance.myGuns[0] = 4;
        }

    }
    private int SlotFinder()
    {
        if (!Gun.instance.first)
        {
            return 0;
        }
        else if (!Gun.instance.second)
        {
            return 1;
        }
        else if(!Gun.instance.third)
        {
            return 2;
        }
        else
        {
            // then it should go to the equiped one
            return Gun.instance.equipedGun;
        }
    }
}
