using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public string speaker;
    public TextMeshPro textMeshPro;
    public MMF_Player mmfPlayer;

    public void UpdateBubble(BubbleLine newLine)
    {
        textMeshPro.text = newLine.bubbleText;
        mmfPlayer.PlayFeedbacks();
    }

    public void HideBubble()
    {
        gameObject.SetActive(false);
    }
}
