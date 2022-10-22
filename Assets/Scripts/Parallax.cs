using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, width, startpos;
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
        //length = GetComponent<SpriteRenderer>().bounds.size.x;
        length = Camera.main.orthographicSize * 1f * Camera.main.aspect;
        width = GetComponent<BoxCollider2D>().size.x * transform.lossyScale.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, cam.transform.position.y + offset, transform.position.z);


        if (temp > startpos + length + width)
        {
            startpos += 2*length + 2*width;
        }
        else if (temp < startpos - (length + width))
        {
            startpos -= (2*length + 2*width);
        }
        
        // float distance = (cam.transform.position.x * parallaxEffect);
        // transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);

    }
    private void OnBecameInvisible()
    {
        /*
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;
        //startpos += width + length;
        if (temp > startpos + length) { startpos += width + length; }
        else if (temp < startpos - length)
        {
            startpos -= width;
            startpos -= length;
        }*/
    }
}
