using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Projectile laserPrefab;
    public Projectile ballPrefab;
    public Projectile misslePrefab;
    public Projectile top;
    public Projectile bottom;
    public float fireSide;

    public GameObject largeBlast;
    public GameObject smallBlast;
    public float destroyTime;
    private GameObject blast;

    public bool explosion;

    [HideInInspector]
    public Projectile current;

    public static Fire instance;
    public float[] rates;
    public float fireRate;
    public float timer;

    public float[] startAmmo;
    public float[] currentAmmo;
    public int ammoIndex;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        for (int i = 0; i < startAmmo.Length; i++)
        {
            currentAmmo[i] = startAmmo[i];
        }
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.GetComponent<SpriteRenderer>().flipX)
        {
            fireSide = .75f;
        }
        else
        {
            fireSide = -.75f;
        }
        switch (Gun.instance.fireType)
        {
            case 0:
                current = ballPrefab;
                explosion = false;
                fireRate = rates[0];
                ammoIndex = 0;
                
                break;

            case 1:
                current = ballPrefab;
                explosion = false;
                fireRate = rates[1];
                ammoIndex = 1;

                break;

            case 2:
                current = laserPrefab;
                explosion = false;
                fireRate = rates[2];
                ammoIndex = 2;

                break;

            case 3:
                current = laserPrefab;
                explosion = false;
                fireRate = rates[3];
                ammoIndex = 3;

                break;

            case 4:
                current = misslePrefab;
                explosion = true;
                fireRate = rates[4];
                ammoIndex = 4;

                break;
        }

    }


    public void GunFire()
    {

        if (Gun.instance.equiped)
        {
            // need to check what the gun type is and then change the prefab that is shot accordingly
            
            if (Gun.instance.auto && currentAmmo[ammoIndex] > 0) 
            {
                Autofire();
            }

            if (!Gun.instance.auto && currentAmmo[ammoIndex] > 0)
            {
                Singlefire();
            }
        }

    }

    // auto fire will be the same as single fire but instead of just once per click as long as it is held down
    // also need to factor in fire rate
    public void Autofire()
    {
        // this is broken
        //StartCoroutine(autoFire());
        StartCoroutine(destrotyBlast());
        Vector3 start = new Vector3(transform.position.x + fireSide, transform.position.y, 0);
        Projectile shot = Instantiate(current, start, Quaternion.identity);
        currentAmmo[ammoIndex] -= 1;
        // print(currentAmmo[ammoIndex]);
        UIHealthPanel.instance.ammo --;
        UIHealthPanel.instance.UpdateAmmo();
        blast = Instantiate(smallBlast, start, Quaternion.identity);
        blast.transform.SetParent(this.transform);
        if (!PlayerController.instance.sp.flipX)
        {
            blast.GetComponent<SpriteRenderer>().flipX = true;
        }
      
    }

    // this needs to go in the correct direction
    public void Singlefire()
    {
     
        Vector3 start = new Vector3(transform.position.x + fireSide, transform.position.y, 0);
        Projectile shot = Instantiate(current, start, Quaternion.identity);
        currentAmmo[ammoIndex] -= 1;
        //print(currentAmmo[ammoIndex]);
        UIHealthPanel.instance.ammo--;

        UIHealthPanel.instance.UpdateAmmo();
        GameObject changeBlast;
        if (fireRate == rates[1])
        {
            // then it is a shotgun and needs to shoot three shots
            top = Instantiate(current, start, Quaternion.identity);
            top.yDir = .25f;

            bottom = Instantiate(current, start, Quaternion.identity);
            bottom.yDir = -.25f;



        }
        if (fireRate == rates[4])
        {
            changeBlast = largeBlast;
        }
        else
        {
            changeBlast = smallBlast;
        }
        blast = Instantiate(changeBlast, start, Quaternion.identity);
        blast.transform.SetParent(this.transform);
        if (!PlayerController.instance.sp.flipX)
        {
            blast.GetComponent<SpriteRenderer>().flipX = true;
        }
        StartCoroutine(destrotyBlast());
        
    }

    // destroys the blast 
    IEnumerator destrotyBlast()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(blast.gameObject);

    }
    public IEnumerator AutoFireCoroutine()
    {
        yield return new WaitForSeconds(fireRate);
        
    }
}
