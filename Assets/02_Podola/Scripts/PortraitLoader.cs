using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class PortraitLoader : MonoBehaviour
{
    [SerializeField] private RawImage leftPortrait;
    [SerializeField] private RawImage rightPortrait;

    private AsyncOperationHandle<Texture2D>? leftPortraitHandle;
    private AsyncOperationHandle<Texture2D>? rightPortraitHandle;

    public void LoadLeftPortrait(string characterName, string expression)
    {
        // 기존에 로드된 초상화가 있다면 해제
        if (leftPortraitHandle.HasValue)
        {
            Addressables.Release(leftPortraitHandle.Value);
            leftPortraitHandle = null;
        }

        string address = $"Assets/00_Shared/Art/Portrait/{characterName}/{expression}.png";
        leftPortraitHandle = Addressables.LoadAssetAsync<Texture2D>(address);
        leftPortraitHandle.Value.Completed += handle =>
        {
            leftPortrait.texture = handle.Result;
            leftPortrait.enabled = true;
        };

    }
    public void LoadRightPortrait(string characterName, string expression)
    {
        if (rightPortraitHandle.HasValue)
        {
            Addressables.Release(rightPortraitHandle.Value);
            rightPortraitHandle = null;
        }
        string address = $"Assets/00_Shared/Art/Portrait/{characterName}/{expression}.png";
        rightPortraitHandle = Addressables.LoadAssetAsync<Texture2D>(address);
        rightPortraitHandle.Value.Completed += handle =>
        {
            rightPortrait.texture = handle.Result;
            rightPortrait.enabled = true;
        };
    }

}