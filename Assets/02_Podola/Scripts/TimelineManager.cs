using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public PlayableDirector director;
    public List<Cutscene> cutsceneList;

    [System.Serializable]
    public class Cutscene
    {
        public string cutsceneName;
        public float startTime;
        public float endTime;
    }

    [Header("Events")]
    public UnityEvent onCutsceneEnd;

    void Awake()
    {
        director.playOnAwake = false;
    }

    public void PlayCutscene(string cutsceneName)
    {
        Cutscene cutscene = null;
        foreach(var cs in cutsceneList)
        {
            if(cs.cutsceneName == cutsceneName)
            {
                cutscene = cs;
                break;
            }
        }

        StartCutscene(cutscene);
    }

    private void StartCutscene(Cutscene cutscene)
    {
        director.time = cutscene.startTime;
        director.Play();

        // 종료 시간까지의 시간을 계산해 자동 정지 예약
        float duration = cutscene.endTime - cutscene.startTime;
        Invoke("StopCutscene", (float)duration);
    }

    // 컷씬이 종료되었을 때 호출되는 콜백
    private void StopCutscene()
    {
        director.Stop();
        onCutsceneEnd.Invoke();
    }
}
