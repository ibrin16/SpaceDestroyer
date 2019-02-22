using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthPanel : MonoBehaviour
{
    public int health;
    public RectTransform UIImage;

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
    
}
