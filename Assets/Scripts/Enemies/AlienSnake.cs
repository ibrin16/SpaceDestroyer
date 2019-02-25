using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSnake : Enemies
{
    public float framesPerSecond;

    public override void SetHealthAndDamage()
    {
        start_health = 3;
        damage = 10;
    }

    public override void Movement()
    {
        Vector3 offset = new Vector3(0, 0.3f, 0) * Mathf.Sin(Time.time);
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
