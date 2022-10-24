using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateThings : MonoBehaviour
{
    public Transform focusPoint;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (focusPoint == null)
        {
            Destroy(transform.parent.gameObject);
        }
        transform.position = focusPoint.position;
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + speed * Time.deltaTime);
        
    }
}
