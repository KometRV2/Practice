using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogData
{
    public string Condition;
    public string StackTrace;
    public string SystemStackTrace;
}

public class LogHandler : Singleton<LogHandler>
{
    private List<LogData> m_LogNormalDataList = new List<LogData>();
    public List<LogData> LogNormalDataList
    {
        get
        {
            return m_LogNormalDataList;
        }
    }

    private List<LogData> m_LogWarningDataList = new List<LogData>();
    public List<LogData> LogWarningDataList
    {
        get
        {
            return m_LogWarningDataList;
        }
    }

    private List<LogData> m_LogErrorDataList = new List<LogData>();
    public List<LogData> LogErrorDataList
    {
        get
        {
            return m_LogErrorDataList;
        }
    }

    void Start ()
    {
        Application.logMessageReceived += LogCallBackHandler;
    }
    
    void LogCallBackHandler(string condition, string stackTrace, LogType type)
    {
        // 取得したログの情報を処理する
        System.Diagnostics.StackTrace systemStackTrace = new System.Diagnostics.StackTrace(true);
        if(systemStackTrace == null)
        {
            Debug.LogWarning("systemStackTrace");
        }
        LogData logData = new LogData();
        logData.Condition = condition;
        logData.StackTrace = stackTrace;
        logData.SystemStackTrace = systemStackTrace.ToString();
        switch(type)
        {
            case LogType.Log:
            if(m_LogNormalDataList == null)
                {
                    Debug.LogWarning("m_LogNormalDataList");
                }
                m_LogNormalDataList.Add(logData);
                
            break;
            case LogType.Warning:
            if(m_LogWarningDataList == null)
                {
                    Debug.LogWarning("m_LogWarningDataList");
                }
                m_LogWarningDataList.Add(logData);
                
            break;
            case LogType.Error:
            case LogType.Exception:
            case LogType.Assert:
                if(m_LogErrorDataList == null)
                {
                    Debug.LogWarning("m_LogErrorDataList");
                }
                m_LogErrorDataList.Add(logData);
            break;
            default:
            break;
        }
    }

    public void ResetLog()
    {
        m_LogNormalDataList.Clear();
        m_LogWarningDataList.Clear();
        m_LogErrorDataList.Clear();
    }
}
