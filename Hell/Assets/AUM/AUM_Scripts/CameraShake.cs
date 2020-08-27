using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using XInputDotNetPure;

public class CameraShake : MonoBehaviour
{
    public static CameraShake cs;

    public CinemachineVirtualCamera cmVcam;

    public CinemachineBasicMultiChannelPerlin cmArenaNoise;

    private CinemachineBasicMultiChannelPerlin cmVcamNoise;

    [HideInInspector] public Coroutine lastCameraShake;

    [HideInInspector] public bool shaking;
    
    // Start is called before the first frame update
    void Start()
    {
        cs = this;

        cmVcam.Follow = MovementController.mC.transform;

        cmVcamNoise = cmVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        shaking = false;
    }

    public IEnumerator CameraShakeFor(float time, float secondTime, float force,float frequency,float gamePadForce)
    {
        shaking = true;

        cmVcamNoise.m_AmplitudeGain = force;
        cmVcamNoise.m_FrequencyGain = frequency;

        if(cmArenaNoise != null)
        {
            cmArenaNoise.m_AmplitudeGain = force;
            cmArenaNoise.m_FrequencyGain = frequency;
        }

        if (OptionData.od != null)
            if (OptionData.od.shaking == true)
                GamePad.SetVibration(0, gamePadForce / 10, gamePadForce / 10);

        yield return new WaitForSeconds(time);

        cmVcamNoise.m_AmplitudeGain = 0;
        cmVcamNoise.m_FrequencyGain = 0;

        if (cmArenaNoise != null)
        {
            cmArenaNoise.m_AmplitudeGain = 0;
            cmArenaNoise.m_FrequencyGain = 0;
        }

        yield return new WaitForSeconds(secondTime);

        GamePad.SetVibration(0, 0, 0);

        shaking = false;
    }

    public void StopCameraShake()
    {
        cmVcamNoise.m_AmplitudeGain = 0;

        GamePad.SetVibration(0, 0, 0);

        if(lastCameraShake != null)
        StopCoroutine(lastCameraShake);
    }

    public void WeakShake()
    {
        StartCoroutine(CameraShakeFor(0.2f, 0.1f, 3, 2, 2));
    }

    public void StrongShake()
    {
        StartCoroutine(CameraShakeFor(0.2f, 0.2f, 5, 3, 3));
    }

    public void PropShake()
    {
        StartCoroutine(CameraShakeFor(0.1f, 0.1f, 0.75f, 1, 1));
    }
}
