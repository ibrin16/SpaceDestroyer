/// <summary>
/// Purpose: Script to control and define player movement, animation, physics,
/// and shoot mechanism
/// Written by: Isacc Brinkman and Daya Shrestha
/// Game Development: Programming and Practice B
/// DIS, Spring 2019
/// Some parts borrowed from Benno Lüders' code
/// </summary>


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
    //public bool reload;



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
    //public bool key3;

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
        //reload = false;
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
        // if fall thru the hole start the level
        if (collision.CompareTag("Finish"))
        {
            SceneManager.LoadScene("Level1");

        }

        // the original gun
        if (collision.CompareTag("Gun"))
        {
            Gun.instance.first = true;
            gunsOn = true;
            equipedGun = collision.gameObject;
            equipedGun.transform.SetParent(transform);
            equipedGun.transform.position = transform.position + Vector3.right * 0.5f;

            Gun.instance.equiped = true;
            Gun.instance.myGuns[0] = 0;
            UIHealthPanel.instance.ammo = Fire.instance.currentAmmo[0];
            UIHealthPanel.instance.maxAmmo = Fire.instance.startAmmo[0];
            UIHealthPanel.instance.UpdateAmmo();

        }

        // maybe keep track of the last pressed number and then automatically press it again
        // so check the tag of the gun
        if (collision.CompareTag("Pistol"))
        { 
            // destroy the hun
            Destroy(collision.gameObject);
            
            if (AlreadyEquiped("Pistol"))
            {
                Fire.instance.currentAmmo[0] = Fire.instance.startAmmo[0];
                // the problem with this line is if the gun you just picked up isnt the one equipped it still changes the ammo
                // if the gun equipped is the pistol
                if(Gun.instance.fireType == 0)
                {
                    UIHealthPanel.instance.ammo = Fire.instance.startAmmo[0];
                }
            }
            else
            {
                // myGuns knows that this gun is equipped
                Gun.instance.myGuns[0] = 0;
                

                // also need to switch guns
                if (switchGun)
                {
                    Gun.instance.sp.sprite = Gun.instance.difGuns[0];
                    Gun.instance.fireType = 0;
                    Gun.instance.auto = false;
                    Gun.instance.sp.sprite = Gun.instance.difGuns[0];
                    Fire.instance.fireRate = Fire.instance.rates[0];
                    Fire.instance.ammoIndex = 0;
                    UIHealthPanel.instance.ammo = Fire.instance.startAmmo[0];
                    UIHealthPanel.instance.maxAmmo = Fire.instance.startAmmo[0];
                    UIHealthPanel.instance.name = "Pistol: ";
                }
            }
            UIHealthPanel.instance.UpdateAmmo();

        }
        if (collision.CompareTag("Shotgun"))
        {
            //gunsOn[1] = true;
            Destroy(collision.gameObject);
            
            if (AlreadyEquiped("Shotgun"))
            {
                Fire.instance.currentAmmo[1] = Fire.instance.startAmmo[1];
                if (Gun.instance.fireType == 1)
                {
                    UIHealthPanel.instance.ammo = Fire.instance.startAmmo[1];
                }
            }
            else
            {
                Gun.instance.myGuns[SlotFinder()] = 1;
                


                if (switchGun)
                {
                    currentGun = 1;
                    Gun.instance.fireType = 1;
                    Gun.instance.auto = false;
                    Gun.instance.sp.sprite = Gun.instance.difGuns[1];
                    Fire.instance.fireRate = Fire.instance.rates[1];
                    Fire.instance.ammoIndex = 1;
                    UIHealthPanel.instance.ammo = Fire.instance.startAmmo[1];
                    UIHealthPanel.instance.maxAmmo = Fire.instance.startAmmo[1];
                    UIHealthPanel.instance.name = "Shotgun: ";

                }
            }
            UIHealthPanel.instance.UpdateAmmo();

        }
        if (collision.CompareTag("AR"))
        {
            //gunsOn[2] = true;
            Destroy(collision.gameObject);
            
            if (AlreadyEquiped("AR"))
            {
                Fire.instance.currentAmmo[2] = Fire.instance.startAmmo[2];
                if (Gun.instance.fireType == 2)
                {
                    UIHealthPanel.instance.ammo = Fire.instance.startAmmo[2];
                }
            }
            else
            {
                Gun.instance.myGuns[SlotFinder()] = 2;
                
                if (switchGun)
                {
                    currentGun = 2;
                    Gun.instance.fireType = 2;
                    Gun.instance.auto = true;
                    Gun.instance.sp.sprite = Gun.instance.difGuns[2];
                    Fire.instance.fireRate = Fire.instance.rates[2];
                    Fire.instance.ammoIndex = 2;
                    UIHealthPanel.instance.ammo = Fire.instance.startAmmo[2];
                    UIHealthPanel.instance.maxAmmo = Fire.instance.startAmmo[2];
                    UIHealthPanel.instance.name = "SMG: ";

                }
            }
            UIHealthPanel.instance.UpdateAmmo();

        }
        if (collision.CompareTag("Better AR"))
        {

            //gunsOn[3] = true;
            Destroy(collision.gameObject);
            
            if (AlreadyEquiped("Better AR"))
            {
                Fire.instance.currentAmmo[3] = Fire.instance.startAmmo[3];
                if (Gun.instance.fireType == 3)
                {
                    UIHealthPanel.instance.ammo = Fire.instance.startAmmo[3];
                }
            }
            else
            {
                Gun.instance.myGuns[SlotFinder()] = 3;
               


                if (switchGun)
                {
                    currentGun = 3;

                    Gun.instance.fireType = 3;
                    Gun.instance.auto = true;
                    Gun.instance.sp.sprite = Gun.instance.difGuns[3];
                    Fire.instance.fireRate = Fire.instance.rates[3];
                    Fire.instance.ammoIndex = 3;
                    UIHealthPanel.instance.ammo = Fire.instance.startAmmo[3];
                    UIHealthPanel.instance.maxAmmo = Fire.instance.startAmmo[3];
                    UIHealthPanel.instance.name = "AR: ";

                }
            }
            UIHealthPanel.instance.UpdateAmmo();

        }

        if (collision.CompareTag("RPG"))
        {
            Destroy(collision.gameObject);
            if (AlreadyEquiped("RPG"))
            {
                Fire.instance.currentAmmo[4] = Fire.instance.startAmmo[4];
                if (Gun.instance.fireType == 4)
                {
                    UIHealthPanel.instance.ammo = Fire.instance.startAmmo[4];
                }
            }
            else
            {
                Gun.instance.myGuns[SlotFinder()] = 4;
                

                if (switchGun)
                {
                    currentGun = 4;

                    Gun.instance.fireType = 4;
                    Gun.instance.auto = false;
                    Gun.instance.sp.sprite = Gun.instance.difGuns[4];
                    Fire.instance.fireRate = Fire.instance.rates[4];
                    Fire.instance.ammoIndex = 4;
                    UIHealthPanel.instance.ammo = Fire.instance.startAmmo[4];
                    UIHealthPanel.instance.maxAmmo = Fire.instance.startAmmo[4];
                    UIHealthPanel.instance.name = "RPG: ";
                }
            }
            UIHealthPanel.instance.UpdateAmmo();
    
        }



    }
    private bool switchGun = false;

    private int SlotFinder()
    {
        // if no gun is equipped in the first slot
        if (!Gun.instance.first)
        {
            Gun.instance.first = true;
            switchGun = false;
            return 0;
        }
        // else if no gun is equipped in the second slot
        else if (!Gun.instance.second)
        {
            Gun.instance.second = true;
            switchGun = false;
            return 1;
        }
  
        // else go to the current slot
        else
        {
     
            print("Current Gun: " + currentGun);
            switchGun = true;
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

}
