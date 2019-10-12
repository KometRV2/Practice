using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDialog : MonoBehaviour
{
    [SerializeField]
    private Text m_DialogText;

    [SerializeField]
    private Button m_OKButton;

    [SerializeField]
    private Button m_NoButton;

    private System.Action m_OnOKAction;
    private System.Action m_OnNoAction;

    public void Initialize(string text, System.Action OnOKAction, System.Action OnNoAction)
    {
        m_DialogText.text = text;
        m_OnOKAction = OnOKAction;
        m_OnNoAction = OnNoAction;

        m_OKButton.onClick.AddListener(OnClickOKAction);
        m_NoButton.onClick.AddListener(OnClickNoAction);
    }

    private void OnClickOKAction()
    {
        if(m_OnOKAction != null)
        {
            m_OnOKAction();
        }
        ReStart();
    }

    private void OnClickNoAction()
    {
        if(m_OnNoAction != null)
        {
            m_OnNoAction();
        }
        ReStart();
    }

    private void ReStart()
    {
        Destroy(this.gameObject);
        TimeManager.I.SetTimeScale(1f);
        SoundManager.I.PlayButtonSE();
    }
}
