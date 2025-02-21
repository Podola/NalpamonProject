using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI() 
    {
        // 기본 인스펙터 필드 출력 (YarnDialogueRunner, debugLogs 등)
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("플레이리스트 아이템", EditorStyles.boldLabel);

        GameManager manager = (GameManager)target;
        if (manager.playlist != null) 
        {
            // 각 아이템별로 ID, 타입, 데이터 입력 및 선택 토글 표시
            for (int i = 0; i < manager.playlist.Count; i++) {
                var item = manager.playlist[i];
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                // 선택 토글 (실행 여부)
                item.selected = EditorGUILayout.Toggle("Selected", item.selected, GUILayout.Width(70));
                EditorGUILayout.LabelField($"Item {i} (ID: {item.id})", EditorStyles.boldLabel);
                EditorGUILayout.EndHorizontal();

                item.id = EditorGUILayout.TextField("Cutscene ID", item.id);
                item.itemType = (PlaylistItemType)EditorGUILayout.EnumPopup("Item Type", item.itemType);
                if (item.itemType == PlaylistItemType.Timeline) 
                {
                    item.timeline = (PlayableDirector)EditorGUILayout.ObjectField("PlayableDirector", item.timeline, typeof(PlayableDirector), true);
                } 
                else if (item.itemType == PlaylistItemType.Dialogue)
                {
                    item.dialogueNode = EditorGUILayout.TextField("Dialogue Node", item.dialogueNode);
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
        }

        EditorGUILayout.Space();
        // 런타임 플레이 모드에서 선택된 아이템 실행 버튼
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
