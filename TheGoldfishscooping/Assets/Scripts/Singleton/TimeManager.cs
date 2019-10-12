using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public float ProgressTime
    {
        get
        {
            return Time.time;
        }
    }
    
    public float DeltaTime
    {
        get
        {
            return Time.deltaTime;
        }
    }

    public float TimeScale
    {
        get
        {
            return Time.timeScale;
        }
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public bool IsStop()
    {
        return Time.timeScale == 0f;
    }
}
