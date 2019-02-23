using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // need to know which direction it is shooting in probably taken from gun class when shot
    public Vector3 direction;
    private Rigidbody2D rgbd;
    public float speed;
    // need to make it so the object is destroyed on collision
    // Start is called before the first frame update
    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        rgbd.velocity = new Vector3(5,0,0) * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // need to destroy on collision with the wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
