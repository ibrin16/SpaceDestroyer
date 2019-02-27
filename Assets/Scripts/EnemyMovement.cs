﻿using System.Collections;
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
    private float knockStunTime = 1f;
    private bool dead = false;
    public float knockBackForce = 20;

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
    

    // Start is called before the first frame update
    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        sr = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = new Vector2(5, 0);
        Vector2 directionBack = new Vector2(-5, 0);
        RaycastHit2D hitForward = Physics2D.Raycast(transform.position, direction, direction.magnitude, playerLayerMask);
        RaycastHit2D hitBack = Physics2D.Raycast(transform.position, directionBack, directionBack.magnitude, playerLayerMask);

        if (hitForward.collider != null || hitBack.collider != null)
        {
            move = true;
        }

        // shoot a ray cast for move
        if (move)
        {
            float deltaX = 0;
            float deltaY = 0;
            if(PlayerInteraction.instance.transform.position.x < transform.position.x)
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
        if(rgbd.velocity.x < 0)
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

        if (dead && this.CompareTag("BossAlien"))
        {
            SceneManager.LoadScene("EndGame");

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
        // maybe should shoot a ray cast to hit the player instead
        // may work better
    {
        Vector2 knockBackDir = collision.transform.position - transform.position;
        knockBackDir.Normalize();
        //look into this more
        if (collision.CompareTag("Player") || collision.CompareTag("Pistol")) 
        {
            PlayerInteraction.instance.Hit(damage);
        }
        if (collision.CompareTag("Laser"))
        {
            Hit(1);
            Destroy(collision.gameObject);
            KnockBack(knockBackDir * knockBackForce);
        }
        if (collision.CompareTag("Ball"))
        {
            Hit(2);
            Destroy(collision.gameObject);
            KnockBack(knockBackDir * knockBackForce);
        }
        if (collision.CompareTag("RPG")){
            Hit(5);
            Destroy(collision.gameObject);
            KnockBack(knockBackDir * knockBackForce);
        }
    }

    private void Hit(int hitDamage)
    {
        //KnockBack(knockback);
        health -= hitDamage;
        StartCoroutine(HurtRoutine());
        if (health <= 0)
        {
            dead = true;
            Destroy(gameObject);
            //print("enemy death");
        }
    }   

    // this knock back is not working
    private void KnockBack(Vector2 force)
    {
        rgbd.velocity = force;
        StartCoroutine(PushBackRoutine());
    }

    IEnumerator PushBackRoutine()
    {
        move = false;
        yield return new WaitForSeconds(knockStunTime);
        move = true;
    }

    IEnumerator HurtRoutine()
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


}
