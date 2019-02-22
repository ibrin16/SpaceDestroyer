using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawner : MonoBehaviour
{
    public Sprite[] guns;
    private SpriteRenderer sp;
    public string gun;

    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        int choice = Random.Range(0, 100);
        if(choice <= 35)
        {
            gun = "Pistol";
            sp.sprite = guns[0];
            this.tag = "Pistol";
        }
        if(choice > 35 && choice <= 60)
        {
            gun = "Shotgun";
            sp.sprite = guns[1];
            this.tag = "Shotgun";
        }
        if(choice > 60 && choice <= 80)
        {
            gun = "AR";
            sp.sprite = guns[2];
            this.tag = "AR";
        }
        if(choice > 75 && choice <= 90)
        {
            gun = "Better AR";
            sp.sprite = guns[3];
            this.tag = "Better AR";
        }
        if(choice >90)
        {
            gun = "RPG";
            sp.sprite = guns[4];
            this.tag = "RPG";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
