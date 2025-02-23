using UnityEngine;

public class BubbleSignalReceiver : MonoBehaviour
{
    public void OnBubbleSignal()
    {
        // 글로벌 매니저(Singleton) 호출
        if (BubbleManager.Instance != null)
        {
            BubbleManager.Instance.DisplayBubble();
        }
        else
        {
            Debug.LogWarning("BubbleManager.Instance is null. Did you create the GlobalManagers?");
        }
    }
}
