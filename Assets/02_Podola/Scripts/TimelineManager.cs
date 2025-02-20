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
