using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // this is the basic parent for all the guns
    // need to know ammo, fire rate, mag size, mag increase size
    // and how to fire
    public int ammo;
    public int clipAmmo;
    public float fireRate;
    public int magSize;
    public int magIncrease;
    public bool equiped;
    public bool auto;

    public Projectile shotPrefab;
    public PlayerController player;
   // public SpriteRenderer sp;
    public Sprite[] difGuns;
    private int fireType; //0 is pistol 1 is shotgun


    //public PlayerController player;
    protected SpriteRenderer sp;

    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        equiped = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.gunsOn[0])
        {
            equiped = true;
        }
        // check for mouse then fire
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
        // equip the pistol
        // what if just had one object that changed sprites and shooting style
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (player.gunsOn[0])
            {
                if (player.equipedGun != null)
                {
                    // change the sprite of the gun
                    sp.sprite = difGuns[0];
                    // and change the fire type
                    fireType = 0;
                    auto = false;
                }
            }
        }
        // equip the shotgun
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (player.gunsOn[1])
            {
                if (player.equipedGun != null)
                {
                    // change the sprite of the gun
                    sp.sprite = difGuns[1];
                    // and change the fire type
                    fireType = 1;
                    auto = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (player.gunsOn[2])
            {
                if (player.equipedGun != null)
                {
                    // change the sprite of the gun
                    sp.sprite = difGuns[2];
                    // and change the fire type
                    fireType = 2;
                    auto = true;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (player.gunsOn[3])
            {
                if (player.equipedGun != null)
                {
                    // change the sprite of the gun
                    sp.sprite = difGuns[3];
                    // and change the fire type
                    fireType = 3;
                    auto = true;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (player.gunsOn[4])
            {
                if (player.equipedGun != null)
                {
                    // change the sprite of the gun
                    sp.sprite = difGuns[4];
                    // and change the fire type
                    fireType = 4;
                    auto = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (player.gunsOn[1])
            {
                if (player.equipedGun != null)
                {
                    // change the sprite of the gun
                    sp.sprite = difGuns[1];
                    // and change the fire type
                    fireType = 1;
                    auto = false;
                }
            }
        }


    }

    public void Fire()
    {
        if(clipAmmo > 0 && equiped)
        {
            if (auto)
            {
                Autofire();
            }

            if (!auto)
            {
                Singlefire();
            }
        }
      
    }

    public void Autofire()
    {

    }
    
    public void Singlefire()
    {
        Vector3 start = new Vector3(transform.position.x + 3, transform.position.y, 0);
        Projectile shot = Instantiate(shotPrefab, start, Quaternion.identity);
        //shot.direction = GetMouseWorldPosition();
    }

    Vector3 GetMouseWorldPosition()
    {
        // this gets the current mouse position (in screen coordinates) and transforms it into world coordinates
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // the camera is on z = -10, so all screen coordinates are on z = -10. To be on the same plane as the game, we need to set z to 0
        mouseWorldPos.z = 0;

        return mouseWorldPos;
    }



}
