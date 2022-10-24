using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HitEffect : MonoBehaviour
{
    private float oldIntensity = .6f;
    private Light2D global;
    public float newIntensity = 0;
    private bool isDimmed = false;
    // Start is called before the first frame update
    void Start()
    {
        global = GameObject.FindGameObjectWithTag("GlobalLights").GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDimmed)
        {
            global.intensity = newIntensity;
            
        }
    }

    public void Dim()
    {
        //print("dim");
        isDimmed = true;
        global.intensity = newIntensity;
        var ghe = global.GetComponent<GlobalHitEffects>();
        if (ghe != null)
        {
            global.GetComponent<GlobalHitEffects>().curTimeSlowLeft = .08f;
        }        
    }

    public void Die()
    {
        //print("undim");

        if (isDimmed)
        {
            global.intensity = oldIntensity;
            isDimmed = false;
        }
        
        Destroy(transform.parent.gameObject);
    }
}
