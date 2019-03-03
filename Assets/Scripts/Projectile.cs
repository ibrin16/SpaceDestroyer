/// <summary>
/// Purpose: This script is attached to all ammunitions and controls 
/// instantiation and destruction when needed
/// Written by: Isacc Brinkman and Daya Shrestha
/// Game Development: Programming and Practice B
/// DIS, Spring 2019
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // need to know which direction it is shooting in probably taken from gun class when shot
    public Vector3 direction;
    private Rigidbody2D rgbd;
    public float speed;
    public float xDir;
    public LayerMask wallLayer;
    public GameObject explosionPrefab;
    public float yDir;
    //public Vector2 direction;
    // need to make it so the object is destroyed on collision
    // Start is called before the first frame update
    void Start()
    {
        //yDir = 0;
        rgbd = GetComponent<Rigidbody2D>();
        direction.Normalize();
        if (PlayerController.instance.GetComponent<SpriteRenderer>().flipX)
        {
            xDir = 1;
        }
        else
        {
            xDir = -1;
        }
        if (PlayerController.instance.sp.flipX)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        rgbd.velocity = new Vector3(xDir,yDir,0) * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Fire.instance.explosion)
        {
            if (collision.CompareTag("Wall") || collision.CompareTag("Snake") || collision.CompareTag("SpaceHugger")
                || collision.CompareTag("BossAlien") || collision.CompareTag("GroundEgg"))
            {
                GameObject expl = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        // need to destroy on collision with the wall
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
       
    }
}
