﻿/// <summary>
/// Purpose: Control the management of weapons including switching, ammo 
/// deductions and refills
/// Written by: Isacc Brinkman and Daya Shrestha
/// Game Development: Programming and Practice B
/// DIS, Spring 2019
/// Some parts borrowed from Benno Lüders' code
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // need to know ammo, fire rate, mag size, mag increase size
    // and how to fire
    public static Gun instance;
    //public int ammo;
    //public int clipAmmo;
    public float fireRate;
    public int magSize;
    public int magIncrease;
    public bool equiped;
    public bool auto;

    public bool first;
    public bool second;
    //public bool third;

    public Sprite[] difGuns;
    public int fireType; //0 is pistol 1 is shotgun

    public int[] myGuns;
    public int equipedGun;
    public int last;
    

    


    //public PlayerController player;
    public SpriteRenderer sp;

    private void Awake()
    {
        instance = this;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        equiped = false;
        myGuns = new int[2];
        equipedGun = 0;
        first = true;
        second = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.gunsOn)
        {
            equiped = true;
        }
        
        // equip the pistol
        // what if just had one object that changed sprites and shooting style
        if (PlayerController.instance.equipedGun != null)
        {
            // maybe instead look if they are all full if they are then destroy the current thing
            if (Input.GetKeyDown(KeyCode.Alpha1) || PlayerController.instance.key1)
            {
                SwitchGuns(0);
                equipedGun = 0;               
                UIHealthPanel.instance.UpdateAmmo();
                PlayerController.instance.key1 = false;
                last = 1;

            }
            // equip the shotgun
            if (Input.GetKeyDown(KeyCode.Alpha2) || PlayerController.instance.key2)
            {
                SwitchGuns(1);
                equipedGun = 1;
                UIHealthPanel.instance.UpdateAmmo();
                PlayerController.instance.key2 = false;
                last = 2;
            }

            // only going to have 2 guns in order to implement this
            //if (Input.GetKeyDown(KeyCode.Alpha3) || PlayerController.instance.key3)
            //{
            //    SwitchGuns(2);
            //    equipedGun = 2;
            //    UIHealthPanel.instance.UpdateAmmo();
            //    last = 3;
            //    PlayerController.instance.key3 = false;

            //}

        }
       
    }

    // takes the index to switch the gun into
    public void SwitchGuns(int index)
    {
        if(index == 1)
        {
            second = true;
        }
        // if the gun at that index is 0
        if (myGuns[index] == 0)
        {
            // change the sprite to the pistol
            sp.sprite = difGuns[0];
            // and change the fire type
            fireType = 0;
            // then update the panel to ammo / max ammo
            // this always works
            UIHealthPanel.instance.ammo = Fire.instance.currentAmmo[0];
            UIHealthPanel.instance.maxAmmo = Fire.instance.startAmmo[0];
            auto = false;
        }
        if (myGuns[index] == 1)
        {
            // change the sprite of the gun
            sp.sprite = difGuns[1];
            // and change the fire type
            fireType = 1;
            UIHealthPanel.instance.ammo = Fire.instance.currentAmmo[1];
            UIHealthPanel.instance.maxAmmo = Fire.instance.startAmmo[1];
            auto = false;

        }
        if (myGuns[index] == 2)
        {
            // change the sprite of the gun
            sp.sprite = difGuns[2];
            // and change the fire type
            fireType = 2;
            UIHealthPanel.instance.ammo = Fire.instance.currentAmmo[2];
            UIHealthPanel.instance.maxAmmo = Fire.instance.startAmmo[2];
            auto = true;
        }
        if (myGuns[index] == 3)
        {
            // change the sprite of the gun
            sp.sprite = difGuns[3];
            // and change the fire type
            fireType = 3;
            UIHealthPanel.instance.ammo = Fire.instance.currentAmmo[3];
            UIHealthPanel.instance.maxAmmo = Fire.instance.startAmmo[3];
            auto = true;
        }
        if (myGuns[index] == 4)
        {
            // change the sprite of the gun
            sp.sprite = difGuns[4];
            // and change the fire type
            fireType = 4;
            UIHealthPanel.instance.ammo = Fire.instance.currentAmmo[4];
            UIHealthPanel.instance.maxAmmo = Fire.instance.startAmmo[4];
            auto = false;
        }
        
    }


   



}
