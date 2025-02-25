using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 어떤 씬을 열어도(Play 버튼을 눌러도) GlobalManagers가 없으면
/// 자동으로 Prefab에서 인스턴스화해주는 부트스트랩 스크립트
/// </summary>
public static class SingletonBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void EnsureGlobalManagers()
    {
        // 혹시 이미 GameManager.Instance가 씬 어딘가에 있다면 아무것도 안 함
        if (GameManager.Instance != null) return;

        // 1) Resources 폴더에 "GlobalManagers" 라는 prefab을 배치
        var prefab = Resources.Load<GameObject>("GlobalManagers");
        if (prefab == null)
        {
            Debug.LogWarning("GlobalManagers prefab not found in Resources folder!");
            return;
        }

        // 2) 인스턴스화
        GameObject instance = GameObject.Instantiate(prefab);
        instance.name = "GlobalManagers";  // (Clone) 꼬리표 제거
        Debug.Log("Load GlobalManagers");

          // 만약 씬에 이미 EventSystem이 존재한다면
        var existingEventSystem = Object.FindAnyObjectByType<EventSystem>();
        if (existingEventSystem != null)
        {
            // 프리팹 내부의 EventSystem 컴포넌트를 찾아 비활성화
            var prefabEventSystem = instance.GetComponentInChildren<EventSystem>();
            if (prefabEventSystem != null)
            {
                prefabEventSystem.gameObject.SetActive(false);
                Debug.Log("Disable duplicate EventSystem on GlobalManagers prefab.");
            }
        }
    }
}
