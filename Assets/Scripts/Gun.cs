using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // need to know ammo, fire rate, mag size, mag increase size
    // and how to fire
    public static Gun instance;
    public int ammo;
    public int clipAmmo;
    public float fireRate;
    public int magSize;
    public int magIncrease;
    public bool equiped;
    public bool auto;

    public Sprite[] difGuns;
    private int fireType; //0 is pistol 1 is shotgun

    public int[] myGuns;


    //public PlayerController player;
    protected SpriteRenderer sp;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        equiped = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.gunsOn[0])
        {
            // i think this is the problem
            equiped = true;
        }
        
        // equip the pistol
        // what if just had one object that changed sprites and shooting style
        if (PlayerController.instance.equipedGun != null)
        {

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchGuns(0); 
            }
            // equip the shotgun
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchGuns(1);

            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchGuns(2);
            }
      
        }
       


    }

    public void SwitchGuns(int index)
    {
        if (myGuns[index] == 0)
        {
            // change the sprite of the gun
            sp.sprite = difGuns[0];
            // and change the fire type
            fireType = 0;
            auto = false;
        }
        if (myGuns[index] == 1)
        {
            // change the sprite of the gun
            sp.sprite = difGuns[1];
            // and change the fire type
            fireType = 1;
            auto = false;
        }
        if (myGuns[index] == 2)
        {
            // change the sprite of the gun
            sp.sprite = difGuns[2];
            // and change the fire type
            fireType = 2;
            auto = true;
        }
        if (myGuns[index] == 3)
        {
            // change the sprite of the gun
            sp.sprite = difGuns[3];
            // and change the fire type
            fireType = 3;
            auto = true;
        }
        if (myGuns[index] == 4)
        {
            // change the sprite of the gun
            sp.sprite = difGuns[4];
            // and change the fire type
            fireType = 4;
            auto = false;
        }
        
    }


   



}
