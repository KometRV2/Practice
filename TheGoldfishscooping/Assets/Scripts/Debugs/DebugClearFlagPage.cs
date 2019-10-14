using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugClearFlagPage : DebugContentsBase
{
    [SerializeField]
    private Button m_ClearResetButton;

    [SerializeField]
    private Button m_AllClearButton;

    private void Awake()
    {
        m_ClearResetButton.onClick.AddListener(OnClickResetClearButton);
        m_AllClearButton.onClick.AddListener(OnClickAllClearButton);
    }

    private void OnClickResetClearButton()
    {
        UserDataManager.I.DeleteData();
    }

    private void OnClickAllClearButton()
    {
        UserDataManager.I.SaveData();
    }
}
