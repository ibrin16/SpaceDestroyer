using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private SpriteRenderer sp;
    public Sprite[] sprites;
    private Rigidbody2D rgbd;
    public float animationTime;
    private int currentSprite = 0;
    private float switchTime = 0;
    public float deltaX = 0;

    public Gun[] allGuns;
    public Gun equipedActualGun;
    public float endJumpTime;
    private float jumpTime = 0;

    public bool[] gunsOn;
    public GameObject equipedGun;


    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        rgbd = GetComponent<Rigidbody2D>();
        // set all the guns to false rn as one is picked up then it will turn to true
        gunsOn = new bool[] {false, false, false, false, false, false};
        //gunsOn = new bool[] {true, true, true, true, true, true };

    }

    // Update is called once per frame
    void Update()
    {
        deltaX = 0;
        float deltaY = 0;
        // move the player
       if(Input.GetKey(KeyCode.A))
        {
            print(equipedGun);
            deltaX = -1;
            sp.flipX = false;
            // if the equiped gun is not null then flip it
            
            if (equipedGun != null)
            {
                    equipedGun.GetComponent<SpriteRenderer>().flipX = false;
                    equipedGun.transform.position = this.transform.position + new Vector3(-.5f, 0, 0);             
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
        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }

        rgbd.velocity = new Vector3(deltaX, deltaY, 0) * speed;

        if (Input.GetMouseButton(0))
        {
            equipedActualGun.Fire();
        }

       

       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pistol"))
        {
            gunsOn[0] = true;
            //Destroy(collision.gameObject);
            equipedGun = collision.gameObject;
            equipedGun.transform.SetParent(transform);
            //pickedItem.transform.position = transform.position + Vector3.right * 0.5f;

            //player.equipedGun = pickedItem;
        }
        if (collision.CompareTag("Shotgun"))
        {
            gunsOn[1] = true;
            Destroy(collision.gameObject);
        }
        
    }

    private void Jump()
    {
        print("here");
    }

    
    
}
