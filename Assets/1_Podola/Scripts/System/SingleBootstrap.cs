using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// 어떤 씬을 열어도(Play 버튼을 눌러도) GlobalManagers가 없으면
/// 자동으로 Prefab에서 인스턴스화해주는 부트스트랩 스크립트
/// </summary>
public static class SingletonBootstrap
{
     // GlobalManagers 프리팹의 EventSystem을 캐싱하기 위한 변수
    private static EventSystem prefabEventSystem;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void EnsureGlobalManagers()
    {
        // 혹시 이미 GameManager.Instance가 씬 어딘가에 있다면 아무것도 안 함
        if (GameManager.Instance != null) return;

        // 1) Resources 폴더에 "GlobalManagers" 라는 prefab을 로드
        var prefab = Resources.Load<GameObject>("GlobalManagers");
        if (prefab == null)
        {
            Debug.LogWarning("[SingleBootstrap] GlobalManagers 프리팹이 리소스 폴더에 없습니다");
            return;
        }

        // 2) 인스턴스화
        GameObject instance = GameObject.Instantiate(prefab);
        instance.name = "GlobalManagers";  // (Clone) 꼬리표 제거
        Debug.Log("[SingleBootstrap] Load GlobalManagers");

       // 프리팹 내부에 있는 EventSystem 컴포넌트를 캐싱 (없으면 null)
        prefabEventSystem = instance.GetComponentInChildren<EventSystem>();

        // 씬이 로드된 후 중복 EventSystem을 체크하도록 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }
      // 씬 로드 완료 시 호출되는 콜백
    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬에 존재하는 모든 EventSystem 검색
        EventSystem[] allEventSystems = Object.FindObjectsByType<EventSystem>(FindObjectsSortMode.None);

        // 프리팹의 EventSystem 이외에 다른 EventSystem이 있는지 확인
        bool foundOther = false;
        foreach (var ev in allEventSystems)
        {
            if (ev != prefabEventSystem)
            {
                foundOther = true;
                break;
            }
        }

        // 만약 다른 EventSystem이 있다면 프리팹의 것을 비활성화
        if (foundOther && prefabEventSystem != null)
        {
            prefabEventSystem.gameObject.SetActive(false);
            Debug.Log("[SingleBootstrap] 이벤트 시스템 중복으로 GlobalManagers의 이벤트 시스템을 비활성화합니다.");
        }

        // 한 번 체크한 후 이벤트 구독 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
