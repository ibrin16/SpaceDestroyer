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



    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        sr = GetComponents<SpriteRenderer>();
    }

    public void Hit(int damage)
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
            StopCoroutine(hurtRoutine);
        }
        hurtRoutine = StartCoroutine(HurtRoutine());
    }

    IEnumerator HurtRoutine ()
    {
        float timer = 0;
        bool blink = false;
        while (timer < hurtTimer)
        {
            blink = !blink;
            timer += Time.deltaTime;
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
    }
    IEnumerator RestartTheGameAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnTrigggerEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            SceneManager.LoadScene("Level1");
        }
    }

}
