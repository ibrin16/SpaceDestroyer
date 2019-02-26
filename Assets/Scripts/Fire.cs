using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Projectile laserPrefab;
    public Projectile ballPrefab;
    public Projectile misslePrefab;
    public float fireSide;

    public bool explosion;

    [HideInInspector]
    public Projectile current;

    public static Fire instance;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
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
    }


    public void GunFire()
    {
        //print("here");
        if (Gun.instance.equiped)
        {
            // need to check what the gun type is and then change the prefab that is shot accordingly
            switch (Gun.instance.fireType)
            {
                case 0:
                    current = laserPrefab;
                    explosion = false;

                    break;
                case 1:
                    current = ballPrefab;
                    explosion = false;

                    break;
                case 2:
                    current = laserPrefab;
                    explosion = false;

                    break;
                case 3:
                    current = ballPrefab;
                    explosion = false;

                    break;
                case 4:
                    current = misslePrefab;
                    explosion = true;
                    break;
            }
            if (Gun.instance.auto)
            {
                Autofire();
            }

            if (!Gun.instance.auto)
            {
                Singlefire();
            }
        }

    }

    // auto fire will be the same as single fire but instead of just once per click as long as it is held down
    // also need to factor in fire rate
    public void Autofire()
    {
        Vector3 start = new Vector3(transform.position.x + fireSide, transform.position.y, 0);
        Projectile shot = Instantiate(current, start, Quaternion.identity);
    }

    // this needs to go in the correct direction
    public void Singlefire()
    {
        Vector3 start = new Vector3(transform.position.x + fireSide, transform.position.y, 0);
        Projectile shot = Instantiate(current, start, Quaternion.identity);
    }
}
