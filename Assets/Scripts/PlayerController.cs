using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float speed = 5;
    public SpriteRenderer sp;
    public Sprite[] sprites;
    public AudioSource jumpSound;
    //public Sprite death;
    //public AudioClip jumpSound;

    public Rigidbody2D rgbd;
    public float animationTime;
    private int currentSprite = 0;
    private float switchTime = 0;
    public float deltaX = 0;
    public bool gunsOn = false;
    public bool moveable;

    public bool canMove = true;

    //public Gun[] allGuns;
    public Gun equipedActualGun;
    public GameObject equipedGun;
    private float timer = 0;
    public bool reload;



    //public float jumpTakeoff;
    //private float normalSpeed = 5;
    //private float jumpSpeed;

    [Header("Jump Controls")]
    public float jumpVelocity = 15;
    public float gravity = 40;
    public float jumpingToleranceTimer = .1f;
    public float groundingToleranceTimer = .1f;

    [Header("Grounding")]
    public Vector2 groundCheckOffset = new Vector2(0, -0.42f);
    public float groundCheckWidth = 1;
    public float groundCheckDepth = 0.2f;
    public int groundCheckRayCount = 3;
    public LayerMask groundLayers = 0;
    private int currentGun;

    public bool key1;
    public bool key2;
    public bool key3;

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
        canMove = true;
        gunsOn = false;
        moveable = true;
        jumpSound = GetComponent<AudioSource>();
        reload = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       // print(gunsOn);
        if (PlayerInteraction.instance.health > 0)
        {

            UpdateGrounding();
            if (!grounded)
            {
                sp.sprite = sprites[3];
            }
            if (moveable)
            {

            Vector2 vel = rgbd.velocity;
            vel.x = 0;
            //vel.y = 0;

            // move the player
            if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.LeftArrow)))
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
                if (switchTime >= animationTime && grounded)
                {
                    currentSprite = (currentSprite + 1) % sprites.Length;
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
                    Fire.instance.fireSide = .75f;
                }
                switchTime += Time.deltaTime;
                if (switchTime >= animationTime && grounded)
                {
                    currentSprite = (currentSprite + 1) % sprites.Length;
                    sp.sprite = sprites[currentSprite];
                    switchTime = 0;
                }
            }

            //jump
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && PermissionToJump())
            {
                jumpSound.Play(0);

                vel = ApplyJump(vel);

            }
            vel.y += -gravity * Time.deltaTime;
            rgbd.velocity = vel;
            if (vel.x == 0 && grounded)
            {
                sp.sprite = sprites[1];
            }
            }


            if (Gun.instance.auto)
            {
                //timer += Time.deltaTime;
                //if (timer >= Fire.instance.fireRate)
                //{

                if (Input.GetKey(KeyCode.Space))
                {
                    timer += Time.deltaTime;
                    if (timer > Fire.instance.fireRate)
                    {
                        Fire.instance.GunFire();

                        timer = 0;
                    }
                }
            }

            if (!Gun.instance.auto)
            {

                timer += Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.Space)
                && (timer > Fire.instance.fireRate))
                {
                    //print("gere");
                    timer = 0;
                    Fire.instance.GunFire();

                }
            }
        }
        else
        {
            // show the dead sprite
            // have to edit the collider I think but thats hard and idk how to do it
            sp.sprite = PlayerInteraction.instance.death;
            rgbd.velocity = new Vector3(0, 0, 0);
            // want to just change the position of the sprite now the whole thing
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, 0);
            sp.transform.position = newPos;

        }
    }


    /// <summary>
    /// Functions to implement jumping
    /// </summary>
    Vector2 ApplyJump(Vector2 vel)
    {
        vel.y = jumpVelocity;
        lastJumpTime = Time.time;
        grounded = false;
        return vel;
    }

    bool PermissionToJump()
    {
        bool wasJustGrounded = Time.time < lostGroundingTime + groundingToleranceTimer;
        bool hasJustJumped = Time.time <= lastJumpTime + Time.deltaTime;
        return (grounded || wasJustGrounded) && !hasJustJumped;
    }

    void UpdateGrounding()
    {
        Vector2 groudCheckCenter = new Vector2(transform.position.x + groundCheckOffset.x, transform.position.y + groundCheckOffset.y);
        Vector2 groundCheckStart = groudCheckCenter + Vector2.down * groundCheckWidth * 0.5f;
        if (groundCheckRayCount > 1)
        {
            for (int i = 0; i < groundCheckRayCount; i++)
            {
                //print("second check");
                //print(groundCheckStart);
                //rint(groundCheckDepth);
                RaycastHit2D hit = Physics2D.Raycast(groundCheckStart, Vector2.down, groundCheckDepth, groundLayers);
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
        if (collision.CompareTag("Finish"))
        {
            SceneManager.LoadScene("Level1");

        }
        // maybe keep track of the last pressed number and then automatically press it again
        if (collision.CompareTag("Pistol"))
        {
            //if (gunsOn)
            //{

            //gunsOn[0] = true;
            Destroy(collision.gameObject);
            Gun.instance.myGuns[0] = 0;
            currentGun = 0;
            if (AlreadyEquiped("Pistol"))
            {
                Fire.instance.currentAmmo[0] = Fire.instance.startAmmo[0];
                UIHealthPanel.instance.ammo = Fire.instance.startAmmo[0];
                reload = true;
            }
        }

            //}
            if (collision.CompareTag("Gun")) 
            {

                gunsOn = true;
                equipedGun = collision.gameObject;
                equipedGun.transform.SetParent(transform);
                equipedGun.transform.position = transform.position + Vector3.right * 0.5f;

                Gun.instance.equiped = true;
                Gun.instance.myGuns[SlotFinder()] = 0;
                UIHealthPanel.instance.ammo = Fire.instance.currentAmmo[0];
                UIHealthPanel.instance.maxAmmo = Fire.instance.startAmmo[0];
                UIHealthPanel.instance.UpdateAmmo();

            }

        
        if (collision.CompareTag("Shotgun"))
        {
            //gunsOn[1] = true;
            Destroy(collision.gameObject);
            Gun.instance.myGuns[SlotFinder()] = 1;
            currentGun = 1;
            if (AlreadyEquiped("Shotgun"))
            {
                Fire.instance.currentAmmo[1] = Fire.instance.startAmmo[1];
                UIHealthPanel.instance.UpdateAmmo();
                reload = true;


            }
        }
        if (collision.CompareTag("AR"))
        {
            //gunsOn[2] = true;
            Destroy(collision.gameObject);
            Gun.instance.myGuns[SlotFinder()] = 2;
            currentGun = 2;
            if (AlreadyEquiped("AR"))
            {
                Fire.instance.currentAmmo[2] = Fire.instance.startAmmo[2];
                UIHealthPanel.instance.UpdateAmmo();
                reload = true;


            }
        }
        if (collision.CompareTag("Better AR"))
        {

            //gunsOn[3] = true;
            Destroy(collision.gameObject);
            Gun.instance.myGuns[SlotFinder()] = 3;
            currentGun = 3;
            if (AlreadyEquiped("Better AR"))
            {
                Fire.instance.currentAmmo[3] = Fire.instance.startAmmo[3];
                UIHealthPanel.instance.UpdateAmmo();
                reload = true;


            }
        }

        if (collision.CompareTag("RPG"))
        {
            //gunsOn[4] = true;
            Destroy(collision.gameObject);
            currentGun = 4;
            Gun.instance.myGuns[SlotFinder()] = 4;
            if (AlreadyEquiped("RPG"))
            {
                Fire.instance.currentAmmo[4] = Fire.instance.startAmmo[4];
                UIHealthPanel.instance.UpdateAmmo();
                reload = true;


            }
        }
        //Repress(Gun.instance.last);



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
            // the problem is the gun doesn't update till a button is pressed
            print(currentGun);
            // switch gun uses myguns which only has 3 max values
            // so AR and RPG doesnt work
            // want something that switches

            // so switch all of it here
            //Gun.instance.equipedGun = currentGun;
            Gun.instance.fireType = currentGun;
            Gun.instance.sp.sprite = Gun.instance.difGuns[currentGun];
            //print(currentGun);
            //foreach (int i in Gun.instance.myGuns)
            //{
            //    print(i);
            //}
            return Gun.instance.equipedGun;
        }
    }

    /// <summary>
    /// Used to draw the red lines for the grounding raycast. Only active in the editor and when the instance is selected.
    /// Borrowed from Benno Lueders, DIS
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Vector2 groudCheckCenter = new Vector2(transform.position.x + groundCheckOffset.x, transform.position.y + groundCheckOffset.y);
        Vector2 groundCheckStart = groudCheckCenter + Vector2.left * groundCheckWidth * 0.5f;
        if (groundCheckRayCount > 1)
        {
            for (int i = 0; i < groundCheckRayCount; i++)
            {
                Debug.DrawLine(groundCheckStart, groundCheckStart + Vector2.down * groundCheckDepth, Color.red);
                groundCheckStart += Vector2.right * (1.0f / (groundCheckRayCount - 1.0f)) * groundCheckWidth;
            }
        }
    }

    private bool AlreadyEquiped(string tag)
    {
        int[] myGuns = Gun.instance.myGuns;
        for (int i = 0; i < myGuns.Length; i++)
        {
            // if it is a pistol and the gun just grabbed has pistol tag
            if (myGuns[i] == 0 && tag.Equals("Pistol"))
            {

                return true;
            }
            if (myGuns[i] == 1 && tag.Equals("Shotgun"))
            {
                return true;
            }
            if (myGuns[i] == 2 && tag.Equals("AR"))
            {
                return true;
            }
            if (myGuns[i] == 3 && tag.Equals("Better AR"))
            {
                return true;
            }
            if (myGuns[i] == 4 && tag.Equals("RPG"))
            {
                return true;
            }
        }
        return false;
    }

    void Repress(int last)
    {
        if(last == 1)
        {
            key1 = true;
        }
        if (last == 2)
        {
            key2 = true;
        }
        else
        {
            key3 = true;
        }
    }

}
