﻿/// <summary>
/// Explosion animation script
/// Purpose: Develops animation on explosion for each projectile
/// Written by: Isacc Brinkman and Daya Shrestha
/// Game Development: Programming and Practice B
/// DIS, Spring 2019
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimation : MonoBehaviour
{
    private SpriteRenderer sp;
    public Sprite[] sprites;
    public float waitTime;
    private float timer = 0;
    private int currentSprite = 0;
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    private float timerCheck;
    // Update is called once per frame
    void Update()
    {
        if (timerCheck >= waitTime * 4) 
        {
            Destroy(this.gameObject);
        }
        timer += Time.deltaTime;
        timerCheck += Time.deltaTime;
        if (timer >= waitTime)
        {
            currentSprite++;
            //currentSprite %= 7;
            sp.sprite = sprites[currentSprite];
            timer = 0;

        }
       
    }

    
}
