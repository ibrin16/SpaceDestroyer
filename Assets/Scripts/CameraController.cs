using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controller for the camera which focuses on player object
public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target = null;
    [SerializeField] float lerpFactor = 1;
    [SerializeField] float minSpeed = 1;

    static CameraController instance;
    Vector3 offset;

    void Awake()
    {
        offset = new Vector3(-10, 0, transform.position.z);
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }

        Vector3 newPos = transform.position;
        Vector3 targetPos = target.position + offset;
        Vector3 targetLerp = Vector3.Lerp(newPos, targetPos, Time.deltaTime * lerpFactor);
        if ((newPos - targetLerp).magnitude > minSpeed * Time.deltaTime)
        {
            newPos = targetLerp;
        } else if ((newPos - targetPos).magnitude > minSpeed * Time.deltaTime)
        {
            Vector3 targetDir = targetPos - newPos;
            targetDir.Normalize();
            newPos += targetDir * (Time.deltaTime * minSpeed);
        }
        newPos.x = Mathf.Clamp(newPos.x, -10f, 10f);
        newPos.y = Mathf.Clamp(newPos.y, -10f, 10f);
    }
}
