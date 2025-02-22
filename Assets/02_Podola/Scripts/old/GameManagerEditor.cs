using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 기본 인스펙터 출력 (다른 필드들은 그대로 표시)
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("플레이리스트 아이템", EditorStyles.boldLabel);

        GameManager manager = (GameManager)target;
        if (manager.playlist != null)
        {
            for (int i = 0; i < manager.playlist.Count; i++)
            {
                var item = manager.playlist[i];

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                // 인덱스 번호와 타입을 라벨로 표시 (예: "1 (Timeline)" 또는 "2 (Dialogue)")
                EditorGUILayout.LabelField($"Item {i + 1} ({item.itemType.ToString()})", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                // 체크박스 오른쪽 정렬
                item.selected = EditorGUILayout.Toggle(item.selected, GUILayout.Width(20));
                EditorGUILayout.EndHorizontal();

                // 타입에 따라 하위 필드만 표시하도록 처리
                if (item.itemType == PlaylistItemType.Dialogue)
                {
                    item.dialogueNode = EditorGUILayout.TextField("Dialogue Node", item.dialogueNode);
                    // id 자동 동기화: 대화 타입이면 id를 dialogueNode로 설정
                    if (string.IsNullOrEmpty(item.id) || item.id != item.dialogueNode)
                    {
                        item.id = item.dialogueNode;
                    }
                }
                else if (item.itemType == PlaylistItemType.Timeline)
                {
                    if (item.sceneTimelineMapping == null)
                    {
                        item.sceneTimelineMapping = new SceneTimelineMapping();
                    }
                    item.sceneTimelineMapping.sceneName = EditorGUILayout.TextField("Scene Name", item.sceneTimelineMapping.sceneName);
                    item.sceneTimelineMapping.timelineAsset = (TimelineAsset)EditorGUILayout.ObjectField("Timeline Asset", item.sceneTimelineMapping.timelineAsset, typeof(TimelineAsset), false);
                    // id 자동 동기화: 타임라인 타입이면 id를 TimelineAsset의 이름으로 설정
                    if (item.sceneTimelineMapping.timelineAsset != null)
                    {
                        string autoID = item.sceneTimelineMapping.timelineAsset.name;
                        if (string.IsNullOrEmpty(item.id) || item.id != autoID)
                        {
                            item.id = autoID;
                        }
                    }
                }

                // ItemType은 선택 후 수정 가능 (id 필드는 자동 갱신되므로 노출하지 않음)
                item.itemType = (PlaylistItemType)EditorGUILayout.EnumPopup("Item Type", item.itemType);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
        }

        EditorGUILayout.Space();
        if (Application.isPlaying)
        {
            if (GUILayout.Button("Play Selected Items"))
            {
                manager.PlaySelectedItems();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("플레이 모드에서 컷신 실행 테스트가 가능합니다.", MessageType.Info);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug Logs", EditorStyles.boldLabel);
        if (manager.debugLogs != null)
        {
            EditorGUILayout.BeginVertical("box");
            foreach (string log in manager.debugLogs)
            {
                EditorGUILayout.LabelField(log);
            }
            EditorGUILayout.EndVertical();
        }
    }
}
