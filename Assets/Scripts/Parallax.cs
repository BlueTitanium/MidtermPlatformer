using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    // public GameObject cam;
    private GameObject cam;
    private float offset;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("CM vcam1");
        startpos = transform.position.x;
        offset = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, cam.transform.position.y + offset, transform.position.z);

        if(temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
        
        // float distance = (cam.transform.position.x * parallaxEffect);
        // transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);

    }
}
