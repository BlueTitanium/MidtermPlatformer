using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HitEffect : MonoBehaviour
{
    private float oldIntensity = .2f;
    private Light2D global;
    public float newIntensity = 0;
    // Start is called before the first frame update
    void Start()
    {
        global = GameObject.FindGameObjectWithTag("GlobalLights").GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Dim()
    {
        print("dim");
        global.intensity = newIntensity;
    }

    public void Die()
    {
        print("undim");
        global.intensity = oldIntensity;
        Destroy(transform.parent.gameObject);
    }
}
