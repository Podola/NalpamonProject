using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public List<PlayableDirector> directorList;
    public event Action<int> OnCutsceneFinished;

    public void PlayCutscene(int index)
    {
        if (index < 0 || index >= directorList.Count)
        {
            Debug.LogWarning($"잘못된 인덱스({index})입니다. 리스트 범위를 확인하세요.");
            return;
        }

        // 이미 재생 중인 타임라인 정지
        foreach (var pd in directorList)
        {
            if (pd != null && pd.state == PlayState.Playing)
            {
                pd.Stop();
            }
        }

        PlayableDirector director = directorList[index];
        if (director == null)
        {
            Debug.LogWarning($"PlayableDirector가 비어있습니다. index: {index}");
            return;
        }

        director.stopped -= OnDirectorStopped;
        director.stopped += OnDirectorStopped;
        director.Play();
    }

    private void OnDirectorStopped(PlayableDirector aDirector)
    {
        int index = directorList.IndexOf(aDirector);
        aDirector.stopped -= OnDirectorStopped;
        OnCutsceneFinished?.Invoke(index);
    }
}
