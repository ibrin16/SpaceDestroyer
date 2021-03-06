﻿/// <summary>
/// Enemy Movement Script
/// Purpose: Controls the movement features of the enemy objects including
/// animation, knockback forces on hit and when to attack the player
/// Written by: Isacc Brinkman and Daya Shrestha
/// Game Development: Programming and Practice B
/// DIS, Spring 2019
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMovement : MonoBehaviour
{
    // need to know who can jump and who cant
    // need to only go towards enemy if inside the collider
    private bool move = false;
    public bool jump;
    public float speed;
    public float hurtTimer = 0.1f;
    public bool boss;
    private bool dead;
    public float playerKnockBackForce = 20;
    private Vector3 force;
    private bool knockback;
    //public PlayerInteraction player;
    private Rigidbody2D rgbd;

    public int health;
    public int damage;
    public LayerMask playerLayerMask;
    public Sprite[] sprites;
    public float waitTime;
    private float time=0;
    private int currentSprite;

    private SpriteRenderer sp;
    public SpriteRenderer[] sr;
    private AudioSource audioHits;
    public AudioClip[] sounds;

    

    // Start is called before the first frame update
    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        sr = GetComponentsInChildren<SpriteRenderer>();
        knockback = true;
        audioHits = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = new Vector2(10, 0);
        Vector2 directionBack = new Vector2(-10, 0);
        Vector2 startPosition = transform.position;
        Vector2 offset = new Vector2(0, -0.3f);
        if (gameObject.CompareTag("BossAlien"))
        {
            startPosition += offset;
        }
        if (knockback && !dead)
        {


            RaycastHit2D hitForward = Physics2D.Raycast(startPosition, direction, direction.magnitude, playerLayerMask);
            RaycastHit2D hitBack = Physics2D.Raycast(startPosition, directionBack, directionBack.magnitude, playerLayerMask);
            if (hitForward.collider != null || hitBack.collider != null)
            {
                move = true;
            }

            // shoot a ray cast for move
            if (move)
            {
                float deltaX = 0;
                float deltaY = -13;
                if (PlayerInteraction.instance.transform.position.x < transform.position.x)
                {
                    deltaX = -1 * speed;
                }
                if (PlayerInteraction.instance.transform.position.x > transform.position.x)
                {
                    deltaX = 1 * speed;
                }
                rgbd.velocity = new Vector3(deltaX, deltaY, 0);
               
            }

            // animation
            if (rgbd.velocity.x < 0)
            {
                sp.flipX = false;
                time += Time.deltaTime;

                if (time >= waitTime)
                {
                    currentSprite = (currentSprite + 1) % sprites.Length;
                    sp.sprite = sprites[currentSprite];
                    time = 0;
                }
            }
            if (rgbd.velocity.x > 0)
            {
                sp.flipX = true;
                time += Time.deltaTime;

                if (time >= waitTime)
                {
                    currentSprite = (currentSprite + 1) % sprites.Length;
                    sp.sprite = sprites[currentSprite];
                    time = 0;
                }
            }
        }
      
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        Vector2 knockBackDir = collision.transform.position - transform.position;
        knockBackDir.Normalize();
        //look into this more
        if (collision.CompareTag("Player") || collision.CompareTag("Gun")) 
        {
            PlayerInteraction.instance.Hit(damage);
            if(PlayerInteraction.instance.transform.position.x < transform.position.x)
            {
                PlayerInteraction.instance.left = true;
            }
            else
            {
                PlayerInteraction.instance.left = false;
            }
            //PlayerInteraction.instance.Knockback(knockBackDir * playerKnockBackForce);
        }
        int dir = 1;
        if (collision.transform.position.x > transform.position.x)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }
        if (collision.CompareTag("Laser"))
        {
            Hit(1);
            
            
                force = new Vector3(30*dir, 1, 0);
            
            //KnockBack(-knockBackDir * knockBackForce);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Ball"))
        {
            force = new Vector3(40*dir, 1, 0);

            Hit(2);
            //KnockBack(-knockBackDir * knockBackForce);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Missle")){
            force = new Vector3(80*dir, 1, 0);

            Hit(5);
            Destroy(collision.gameObject);
        }
    }

    private void Hit(int hitDamage)
    {
        move = true;
        health -= hitDamage;
        audioHits.PlayOneShot(sounds[0]);
        StartCoroutine(HurtRoutine());
        if (health <= 0)
        {
            audioHits.PlayOneShot(sounds[1]);
            if (boss)
            {
                StartCoroutine(EndGame(3f));
                sp.sprite = null;
                dead = true;
                Destroy(sp.GetComponent<CapsuleCollider2D>());
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }   



    IEnumerator HurtRoutine()
    {
        knockback = false;
        float timer = 0;
        bool blink = false;
        while (timer < hurtTimer)
        {
            if (timer < hurtTimer / 2)
            {
                rgbd.velocity = force;

            }
            knockback = true;
            blink = !blink;
            timer += Time.deltaTime;
            if (blink)
            {
                foreach (SpriteRenderer sprite in sr)
                {
                    sprite.color = Color.white;
                }
            }
            else
            {
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

    IEnumerator EndGame(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("Win");
    }

}
