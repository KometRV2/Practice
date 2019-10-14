using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLogDetailButtonItem : MonoBehaviour
{
    private System.Action m_ButtonAction;
    public LogType m_LogType;
    
    public void Initialize(LogType logType, LogData logData, System.Action action)
    {
        m_LogType = logType;
        Text buttonText = GetComponentInChildren<Text>(true);
        buttonText.text = logData.Condition;
        m_ButtonAction = action;
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        if(m_ButtonAction != null)
        {
            m_ButtonAction();
        }
    }
}
