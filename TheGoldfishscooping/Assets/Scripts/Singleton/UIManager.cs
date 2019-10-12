using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public Camera UICamera;
    
    [SerializeField]
    private GameObject m_Stacker;

    public void CreateYesNoDialog(string text, System.Action OnOKAction, System.Action OnNoAction)
    {
        TimeManager.I.SetTimeScale(0f);
        UIDialog uiDialog = ResourceManager.CreateUI<UIDialog>("UIDialog", m_Stacker.transform);
        uiDialog.Initialize(text, OnOKAction, OnNoAction);
    }

    public void CreateSystemMenu()
    {
        TimeManager.I.SetTimeScale(0f);
        // UISystemMenu uiDialog = ResourceManager.CreateUI<UISystemMenu>("UISystemMenu", m_Stacker.transform);
        // uiDialog.Initialize();
    }
}
