using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLogPage : DebugContentsBase
{
    private enum DispLogType
    {
        Normal,
        Warning,
        Error,
    }

    private Button m_ResetButton;

    private Button m_LogNormalButton;

    private Button m_LogWarningButton;

    private Button m_LogErrorButton;

    private RectTransform m_DetailItemButtonParent;

    private Text m_LogDetailText;

    private GameObject m_LogDetailItemRef;

    private List<GameObject> m_LogNormalItems = new List<GameObject>();
    private List<GameObject> m_LogWarningItems = new List<GameObject>();
    private List<GameObject> m_LogErrorItems = new List<GameObject>();

    private DispLogType m_DispLogType = DispLogType.Normal;

    private void Awake()
    {
        m_ResetButton = this.transform.Find("ResetButton").GetComponent<Button>();
        m_LogNormalButton = this.transform.Find("LogNormalButton").GetComponent<Button>();
        m_LogWarningButton = this.transform.Find("WarningButton").GetComponent<Button>();
        m_LogErrorButton = this.transform.Find("ErrorButton").GetComponent<Button>();
        m_DetailItemButtonParent = this.transform.Find("ScrollView/Viewport/Content").GetComponent<RectTransform>();
        m_LogDetailText = this.transform.Find("Panel/LogDetailText").GetComponent<Text>();
        m_LogDetailItemRef = ResourceManager.LoadObject("Debug/DebugLogDetailButtonItem");

        m_ResetButton.onClick.AddListener(OnClickResetButton);
        m_LogNormalButton.onClick.AddListener(OnClickLogNormalButton);
        m_LogWarningButton.onClick.AddListener(OnClickWarningButton);
        m_LogErrorButton.onClick.AddListener(OnClickErrorButton);
    }

    public override void Initialize()
    {
        base.Initialize();
        CreateDebugDetailButtonItem(LogHandler.I.LogNormalDataList, m_LogNormalItems, LogType.Log);
        CreateDebugDetailButtonItem(LogHandler.I.LogWarningDataList, m_LogWarningItems, LogType.Warning);
        CreateDebugDetailButtonItem(LogHandler.I.LogErrorDataList, m_LogErrorItems, LogType.Error);
        OnClickLogNormalButton();
    }

    private void CreateDebugDetailButtonItem(List<LogData> logDatas, List<GameObject> targetList, LogType logType)
    {
        GameObject itemObj = null;
        DebugLogDetailButtonItem item = null;
        for(int i = logDatas.Count - 1, il = 0; i  >= il; i--)
        {
            itemObj = GameObject.Instantiate(m_LogDetailItemRef, Constants.VECTOR3_ZERO, Quaternion.identity, m_DetailItemButtonParent);
            item = itemObj.GetComponent<DebugLogDetailButtonItem>();
            targetList.Add(itemObj);
            string stackTrace = logDatas[i].StackTrace;
            string systemStackTrace = logDatas[i].SystemStackTrace;
            item.Initialize(logType, logDatas[i], () => 
            {
                OnClickLogDetailButton(systemStackTrace);
            });
        }
    }

    private void Update()
    {
        CreateDiffDetailButtonItem(LogHandler.I.LogNormalDataList, m_LogNormalItems, LogType.Log);
        CreateDiffDetailButtonItem(LogHandler.I.LogWarningDataList, m_LogWarningItems, LogType.Warning);
        CreateDiffDetailButtonItem(LogHandler.I.LogErrorDataList, m_LogErrorItems, LogType.Error);
        DispLogTypeCheck();
    }

    private void DispLogTypeCheck()
    {
        switch(m_DispLogType)
        {
            case DispLogType.Normal:
                SetActiveNormalItem(true);
                SetActiveWarningItem(false);
                SetActiveErrorItem(false);
            break;
            case DispLogType.Warning:
                SetActiveNormalItem(false);
                SetActiveWarningItem(true);
                SetActiveErrorItem(false);
            break;
            case DispLogType.Error:
                SetActiveNormalItem(false);
                SetActiveWarningItem(false);
                SetActiveErrorItem(true);
            break;
        }
    }

    private void CreateDiffDetailButtonItem(List<LogData> logDatas, List<GameObject> targetList, LogType logType)
    {
        int targetListCount = targetList.Count;
        int createCount = logDatas.Count - targetListCount;
        if(createCount > 0)
        {
            GameObject itemObj = null;
            DebugLogDetailButtonItem item = null;
            for(int i = createCount - 1; i >= 0; i--)
            {
                itemObj = GameObject.Instantiate(m_LogDetailItemRef, Constants.VECTOR3_ZERO, Quaternion.identity, m_DetailItemButtonParent);
                itemObj.transform.SetAsFirstSibling();
                item = itemObj.GetComponent<DebugLogDetailButtonItem>();
                string stackTrace = logDatas[i + targetListCount].StackTrace;
                string systemStackTrace = logDatas[i + targetListCount].SystemStackTrace;
                item.Initialize(logType, logDatas[i + targetListCount], () => 
                {
                    OnClickLogDetailButton(systemStackTrace);
                });
                targetList.Add(itemObj);
            }
        }
    }

    private void OnClickLogDetailButton(string text)
    {
        m_LogDetailText.text = text;
    }

    private void OnClickLogNormalButton()
    {
        m_DispLogType = DispLogType.Normal;
    }

    private void OnClickWarningButton()
    {
        m_DispLogType = DispLogType.Warning;
    }

    private void OnClickErrorButton()
    {
        m_DispLogType = DispLogType.Error;
    }

    private void SetActiveNormalItem(bool isActive)
    {
        for(int i = 0, il = m_LogNormalItems.Count; i < il; i++)
        {
            m_LogNormalItems[i].SetActive(isActive);
        }
    }

    private void SetActiveWarningItem(bool isActive)
    {
        for(int i = 0, il = m_LogWarningItems.Count; i < il; i++)
        {
            m_LogWarningItems[i].SetActive(isActive);
        }
    }

    private void SetActiveErrorItem(bool isActive)
    {
        for(int i = 0, il = m_LogErrorItems.Count; i < il; i++)
        {
            m_LogErrorItems[i].SetActive(isActive);
        }
    }

    private void DestoryLogNormalItem()
    {
        for(int i = 0, il = m_LogNormalItems.Count; i < il; i++)
        {
            Destroy(m_LogNormalItems[i].gameObject);
        }
        m_LogNormalItems.Clear();
    }

    private void DestoryLogWarningItem()
    {
        for(int i = 0, il = m_LogWarningItems.Count; i < il; i++)
        {
            Destroy(m_LogWarningItems[i].gameObject);
        }
        m_LogWarningItems.Clear();
    }

    private void DestoryLogErrorItem()
    {
        for(int i = 0, il = m_LogErrorItems.Count; i < il; i++)
        {
            Destroy(m_LogErrorItems[i].gameObject);
        }
        m_LogErrorItems.Clear();
    }

    private void OnClickResetButton()
    {
        LogHandler.I.ResetLog();
        DestoryLogNormalItem();
        DestoryLogWarningItem();
        DestoryLogErrorItem();
        m_LogDetailText.text = "";
    }
}
