using MoreMountains.Feedbacks;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject cameraShaker;
    public GameObject cameraFadeIn;
    public GameObject cameraFadeOut;

    public void ShakeCamera()
    {
        cameraShaker.GetComponent<MMF_Player>().PlayFeedbacks();
    }

    public void FadeIn()
    {
        cameraFadeIn.GetComponent<MMF_Player>().PlayFeedbacks();
    }

    public void FadeOut()
    {
        cameraFadeOut.GetComponent<MMF_Player>().PlayFeedbacks();
    }
}
