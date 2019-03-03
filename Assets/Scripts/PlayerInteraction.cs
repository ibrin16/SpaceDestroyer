/// <summary>
/// Purpose: Controls all of the interactive elements associated with the player
/// including weapons pickup, damage update, and game restarting when health = 0
/// Written by: Isacc Brinkman and Daya Shrestha
/// Game Development: Programming and Practice B
/// DIS, Spring 2019
/// Some parts borrowed from Benno Lüders' code
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    public int health;
    public static PlayerInteraction instance;
    //public GameObject equipedGun;
    public float hurtTimer = 0.1f;
    public SpriteRenderer[] sr;
    Coroutine hurtRoutine;
    public Sprite death;
    public bool left;
    private bool hurtable;



    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        sr = GetComponents<SpriteRenderer>();
        hurtable = true;
    }

    public void Hit(int damage)
    {
        if (hurtable)
        {
            // need to also implement the health bar changing
            health -= damage;
            UIHealthPanel.instance.UpdateHealth();

            if (health <= 0)
            {
                StartCoroutine(RestartTheGameAfterSeconds(1));
                // reload current scene
            }

            // player hurt coroutine which handles player blinking
            if (hurtRoutine != null)
            {
                //Knockback(new Vector2(30, 1));
                StopCoroutine(hurtRoutine);
            }
            hurtRoutine = StartCoroutine(HurtRoutine());
        }
    }

    public void Knockback (Vector2 force)
    {
        GetComponent<Rigidbody2D>().AddForce(force);
        //PlayerController.instance.Knockback(force);
        //StartCoroutine(KnockBackRoutine());
    }

    //IEnumerator KnockBackRoutine()
    //{

    //    PlayerController.instance.canMove = false;
    //    yield return new WaitForSeconds(knockStunTime);
    //    PlayerController.instance.canMove = true;
    //}

    IEnumerator HurtRoutine ()
    {
        hurtable = false;
        PlayerController.instance.moveable = false;
        
        float timer = 0;
        bool blink = false;
        while (timer < hurtTimer)
        {
            blink = !blink;
            timer += Time.deltaTime;
            if (timer < hurtTimer / 2)
            {
                // should check if enemy is too the left or to the right in order to decide which way to go
                if (left)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector3(-25, 4, 0);

                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector3(25, 4, 0);

                }
            }
            else
            {
                PlayerController.instance.moveable = true;
            }
            if (blink)
            {
                foreach (SpriteRenderer sprite in sr)
                {
                    sprite.color = Color.white;
                }
            } 
            else { 
                foreach (SpriteRenderer sprite in sr)
                {
                    sprite.color = Color.red;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
        foreach (SpriteRenderer sprite in sr)
        {
            sprite.color = Color.white;
        }

        hurtable = true;
    }
    IEnumerator RestartTheGameAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("Lose");
    }



}
