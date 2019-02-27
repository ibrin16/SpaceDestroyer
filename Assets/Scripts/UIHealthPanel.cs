using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthPanel : MonoBehaviour
{
    public int health;
    public RectTransform UIImage;
    public Text ammoText;
    public Text gunText;

    public static UIHealthPanel instance;

    public void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealth()
    {
        if(UIImage == null)
        {
            return;
        }

        health = PlayerInteraction.instance.health;
        float scale = (float)health / 100f;
        Vector3 scaleHealth = UIImage.transform.localScale;
        scaleHealth.x = scale;
        UIImage.transform.localScale = scaleHealth;
        print(scale);
    }
    public void UpdateAmmo()
    {
        float ammo = Fire.instance.currentAmmo[Fire.instance.ammoIndex];
        float maxAmmo = Fire.instance.startAmmo[Fire.instance.ammoIndex];
        ammoText.text = ammo + "/" + maxAmmo;
        string gun = "";
        switch (Fire.instance.ammoIndex)
        {
            case 0:
                gun = "Pistol: ";
                break;
            case 1:
                gun = "Shotgun: ";
                break;
            case 2:
                gun = "SMG: ";
                break;
            case 3:
                gun = "AR: ";
                break;
            case 4:
                gun = "RPG: ";
                break;
        }
        gunText.text = gun;
    }
}
