using UnityEngine;

public class BubbleSignalReceiver : MonoBehaviour
{
   public void OnBubbleSignal()
    {
        BubbleManager.Instance.DisplayBubble();
    }
}
