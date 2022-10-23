using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalHitEffects : MonoBehaviour
{
    public float curTimeSlowLeft = 0f;
    private Sword sword;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        sword = FindObjectOfType<Sword>();
    }

    // Update is called once per frame
    void Update()
    {
        if(curTimeSlowLeft > 0)
        {
            Time.timeScale = 0.05f;
            curTimeSlowLeft -= Time.unscaledDeltaTime;
        } else
        {
            Time.timeScale = sword.properTimeScale;
        }
    }
}
