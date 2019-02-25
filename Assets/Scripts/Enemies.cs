using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemies : MonoBehaviour
{
    protected Rigidbody2D rgbd;
    protected SpriteRenderer sr;
    protected Vector3 startPosition;

    protected int start_health;
    protected int damage;
    public bool dead = false;
    public Sprite[] aframes;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        sr = GetComponent<SpriteRenderer>();
        rgbd = GetComponent<Rigidbody2D>();
        Animate();
        SetHealthAndDamage();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        if (dead)
        {
            Destroy(gameObject);
        }
    }

    public abstract void Movement();

    public abstract void Animate();

    public abstract void SetHealthAndDamage();

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Pistol"))
        {
            PlayerInteraction.instance.Hit(damage);
            print("here1");
        }
        if (collision.CompareTag("Laser"))
        {
            Hit(1);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Ball"))
        {
            Hit(2);
            Destroy(collision.gameObject);

        }
        if (collision.CompareTag("RPG"))
        {
            Hit(5);
            Destroy(collision.gameObject);

        }
    }

    public void Hit(int damage)
    {
        start_health -= damage;
        if (start_health <= 0)
        {
            dead = true;
            print("enemy death");
        }
    }

}
