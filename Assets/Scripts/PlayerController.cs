using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float speed;
    private SpriteRenderer sp;
    public Sprite[] sprites;
    private Rigidbody2D rgbd;
    public float animationTime;
    private int currentSprite = 0;
    private float switchTime = 0;
    public float deltaX = 0;
    public bool[] gunsOn;


    //public Gun[] allGuns;
    public Gun equipedActualGun;

    public GameObject equipedGun;
    private bool grounded;
    public float jumpTakeoff;
    private float normalSpeed;
    private float jumpSpeed;

    public void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        rgbd = GetComponent<Rigidbody2D>();
        grounded = true;
        normalSpeed = speed;
        jumpSpeed = speed / 2;
        // = PlayerInteraction.instance.equipedGun;
        gunsOn = new bool[] { false, false, false, false, false, false };

    }

    // Update is called once per frame
    void Update()
    {
        deltaX = 0;
        float deltaY = 0;
        // move the player
       if(Input.GetKey(KeyCode.A))
        {
            //print(equipedGun);
            deltaX = -1;
            sp.flipX = false;
            // if the equiped gun is not null then flip it
            
            if (equipedGun != null)
            {
                    equipedGun.GetComponent<SpriteRenderer>().flipX = false;
                    equipedGun.transform.position = this.transform.position + new Vector3(-.5f, 0, 0);
                    Fire.instance.fireSide = -.75f;


            }
            switchTime += Time.deltaTime;
            if(switchTime >= animationTime)
            {
                currentSprite = (currentSprite + 1)%sprites.Length;
                sp.sprite = sprites[currentSprite];
                switchTime = 0;
       
            }

        }
        if (Input.GetKey(KeyCode.D))
        {
            deltaX = 1;
            sp.flipX = true;
            if (equipedGun != null)
            {
                equipedGun.GetComponent<SpriteRenderer>().flipX = true;
                equipedGun.transform.position = this.transform.position + new Vector3(.5f, 0, 0);
                Fire.instance.fireSide = -.75f;

            }
            switchTime += Time.deltaTime;
            if (switchTime >= animationTime)
            {
                currentSprite = (currentSprite + 1) % sprites.Length;
                sp.sprite = sprites[currentSprite];
                switchTime = 0;
                

            }
        }
        //jump
        if (Input.GetKey(KeyCode.W) && grounded)
        {
            deltaY = 7;
            speed = jumpSpeed;
            grounded = false;
        }

        rgbd.velocity = new Vector3(deltaX, deltaY, 0) * speed;


        if (Gun.instance.auto)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Fire.instance.GunFire();
            }
        }

        if (!Gun.instance.auto)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Fire.instance.GunFire();
            }
        }
        
        if(rgbd.velocity.y != 0)
        {
            grounded = false;
            speed = normalSpeed;
        }
        else
        {
            grounded = true;
        }



    }

   

    private void Jump()
    {

        //deltaY = 1;
        print("here");
        //else if (Input.GetButtonUp("Jump"))
        //{
        //    if (velocity.y > 0)
        //    {
        //        velocity.y = velocity.y * 0.5f;
        //    }
        //}
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
                equipedGun.transform.position = transform.position + Vector3.right * 0.5f;

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
        else if (!Gun.instance.third)
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
