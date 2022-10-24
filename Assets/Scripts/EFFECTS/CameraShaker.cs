using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraShaker : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    private float shakeTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer <= 0f)
        {
            CinemachineBasicMultiChannelPerlin cmbmcp = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cmbmcp.m_AmplitudeGain = 0;
        } else
        {
            shakeTimer -= Time.deltaTime;
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cmbmcp = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cmbmcp.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

}
