using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Vector2 parallaxEffectMultiplier;
    private Camera cam;
    private float width;
    private GameObject leftClone;

    void Start()
    {
        cam = Camera.main;
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        leftClone = transform.GetChild(0).gameObject;
        leftClone.transform.position = new Vector3(transform.position.x - width, transform.position.y, transform.position.z);
    }

    
    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffectMultiplier.x));
        float dist = (cam.transform.position.x * parallaxEffectMultiplier.x);

        transform.position = new Vector3(dist, transform.position.y, transform.position.z);

        if (temp > transform.position.x + width)
        {
            transform.position = new Vector3(leftClone.transform.position.x + width, transform.position.y, transform.position.z);
        }
        else if (temp < transform.position.x - width)
        {
            transform.position = new Vector3(leftClone.transform.position.x - width, transform.position.y, transform.position.z);
        }
    }
}
