using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float speed = 5;
    private SpriteRenderer sp;
    public Sprite[] sprites;
    private Rigidbody2D rgbd;
    public float animationTime;
    private int currentSprite = 0;
    private float switchTime = 0;
    public float deltaX = 0;
    public bool[] gunsOn;

    public bool canMove = true;

    //public Gun[] allGuns;
    public Gun equipedActualGun;
    public GameObject equipedGun;
 
 
    //public float jumpTakeoff;
    //private float normalSpeed = 5;
    //private float jumpSpeed;

    [Header("Jump Controls")]
    public float jumpVelocity = 15;
    public float gravity = 40;
    public float jumpingToleranceTimer = .1f;
    public float groundingToleranceTimer = .1f;

    [Header("Grounding")]
    public Vector2 groundCheckOffset = new Vector2(0, 0.1f);
    public float groundCheckWidth = 1;
    public float groundCheckDepth = 0.2f;
    public int groundCheckRayCount = 3;
    public LayerMask Ground = 0;

    bool grounded = false;

    float lostGroundingTime = 0;
    float lastJumpTime = 0;
    //float lastInputJump = 0;


    public void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        rgbd = GetComponent<Rigidbody2D>();
        //grounded = true;
        //normalSpeed = speed;
        //jumpSpeed = speed / 2;
        // = PlayerInteraction.instance.equipedGun;
        gunsOn = new bool[] { false, false, false, false, false, false };
        canMove = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateGrounding();

        Vector2 vel = rgbd.velocity;

        //deltaX = 0;
        //float deltaY = 0;
        // move the player
       if((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.LeftArrow)))
        {
            //print(equipedGun);
            vel.x = -1 * speed;
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
        if ((Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.RightArrow)))
        {
            vel.x = 1 * speed;
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
        print(PermissionToJump());

        //jump
        if (Input.GetKey(KeyCode.W))
        // && PermissionToJump())
        {
            vel = ApplyJump(vel);
        }
        vel.y += -gravity * Time.deltaTime;
        rgbd.velocity = vel;
        //rgbd.velocity = new Vector3(deltaX, deltaY, 0) * speed;


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
    }

    
    /// <summary>
    /// Functions to implement jumping
    /// </summary>
    Vector2 ApplyJump (Vector2 vel)
    {
        vel.y = jumpVelocity;
        lastJumpTime = Time.time;
        grounded = false;
        return vel;
    }

    bool PermissionToJump ()
    {
        bool wasJustGrounded = Time.time < lostGroundingTime + groundingToleranceTimer;
        bool hasJustJumped = Time.time <= lastJumpTime + Time.deltaTime;
        return (grounded || wasJustGrounded) && !hasJustJumped;
    }

    void UpdateGrounding()
    {
        Vector2 groudCheckCenter = new Vector2(transform.position.x + groundCheckOffset.x, transform.position.y + groundCheckOffset.y);
        Vector2 groundCheckStart = groudCheckCenter + Vector2.left * groundCheckWidth * 0.5f;
        if (groundCheckRayCount > 1)
        {
            for (int i = 0; i < groundCheckRayCount; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(groundCheckStart, Vector2.down, groundCheckDepth, Ground);
                if (hit.collider != null)
                {
                    grounded = true;
                    return;
                }
                groundCheckStart += Vector2.right * (1.0f / (groundCheckRayCount - 1.0f)) * groundCheckWidth;
            }
        }
        if (grounded)
        {
            lostGroundingTime = Time.time;
        }
        grounded = false;
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
                Gun.instance.myGuns[SlotFinder()] = 0;
            }

        }
        if (collision.CompareTag("Shotgun"))
        {
            gunsOn[1] = true;
            Destroy(collision.gameObject);
            Gun.instance.myGuns[SlotFinder()] = 1;
        }
        if (collision.CompareTag("AR"))
        {
            gunsOn[1] = true;
            Destroy(collision.gameObject);
            Gun.instance.myGuns[SlotFinder()] = 2;
        }
        if (collision.CompareTag("Better AR"))
        {
            gunsOn[1] = true;
            Destroy(collision.gameObject);
            Gun.instance.myGuns[SlotFinder()] = 3;
        }
        if (collision.CompareTag("RPG"))
        {
            gunsOn[1] = true;
            Destroy(collision.gameObject);
            Gun.instance.myGuns[SlotFinder()] = 4;
        }

    }
    private int SlotFinder()
    {
        if (!Gun.instance.first)
        {
            Gun.instance.first = true;
            return 0;
        }
        else if (!Gun.instance.second)
        {
            Gun.instance.second = true;
            return 1;
        }
        else if (!Gun.instance.third)
        {
            Gun.instance.third = true;
            return 2;
        }
        else
        {
            // then it should go to the equiped one
            return Gun.instance.equipedGun;
        }
    }



}
