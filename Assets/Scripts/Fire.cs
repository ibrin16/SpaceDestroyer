using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Projectile shotPrefab;
    public static Fire instance;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void GunFire()
    {
        //print("here");
        if (Gun.instance.equiped)
        {
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

    }

    // this needs to go in the correct direction
    public void Singlefire()
    {
        Vector3 start = new Vector3(transform.position.x + 3, transform.position.y, 0);
        Projectile shot = Instantiate(shotPrefab, start, Quaternion.identity);
    }
}
