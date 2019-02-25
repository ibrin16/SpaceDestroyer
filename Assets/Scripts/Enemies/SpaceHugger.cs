using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceHugger : Enemies
{
    public float framesPerSecond;

    public override void SetHealthAndDamage()
    {
        start_health = 6;
        damage = 20;
    }

    public override void Movement()
    {
        sr.flipX = false;
        Vector3 offset = new Vector3(0.5f, 0, 0) * Mathf.Sin(Time.time);
        if (offset.x < 0)
        {
            sr.flipX = true;   
        }
        else
        {
            sr.flipX = false;
        }
        transform.position = startPosition + offset;
    }


    public override void Animate()
    {
        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        while (true)
        {
            int currentFrameIndex = 0;
            while (currentFrameIndex < aframes.Length)
            {
                sr.sprite = aframes[currentFrameIndex];
                yield return new WaitForSeconds(1f / framesPerSecond);
                currentFrameIndex++;
            }
        }

    }
}
