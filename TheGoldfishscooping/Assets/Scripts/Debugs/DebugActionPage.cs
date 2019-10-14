using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.XR.ARFoundation;
using System.Runtime.InteropServices;

public class DebugActionPage : DebugContentsBase
{
#if UNITY_IPHONE
    [DllImport ("__Internal")]
    private static extern void playSystemSound(int n);
#endif

    [SerializeField]
    private Button m_DebugActionButton1;

    [SerializeField]
    private Button m_DebugActionButton2;

    [SerializeField]
    private Button m_DebugActionButton3;

    [SerializeField]
    private Button m_DebugActionButton4;

    [SerializeField]
    private Button m_DebugActionButton5;

    [SerializeField]
    private Button m_DebugActionButton6;

    [SerializeField]
    private Button m_DebugActionButton7;

    [SerializeField]
    private Button m_DebugActionButton8;

    [SerializeField]
    private Button m_DebugActionButton9;

    [SerializeField]
    private InputField m_InputField;

    [SerializeField]
    private Button m_InputFieldButton;

    // private ARSessionOrigin m_ARSessionOrigin;
    // private ARPlaneManager m_ARPlaneManager;
    // private ARRaycastManager m_ARRayCastManager;
    // private ARInputManager m_ARInputManager;
    // private ARCameraManager m_ARCameraManager;
    // private ARCameraBackground m_ARCameraBackground;
    // private ARSession m_ARSession;
    private void Awake()
    {
        m_DebugActionButton1.onClick.AddListener(OnButtonAction1);
        m_DebugActionButton2.onClick.AddListener(OnButtonAction2);
        m_DebugActionButton3.onClick.AddListener(OnButtonAction3);
        m_DebugActionButton4.onClick.AddListener(OnButtonAction4);
        m_DebugActionButton5.onClick.AddListener(OnButtonAction5);
        m_DebugActionButton6.onClick.AddListener(OnButtonAction6);
        m_DebugActionButton7.onClick.AddListener(OnButtonAction7);
        m_DebugActionButton8.onClick.AddListener(OnButtonAction8);
        m_DebugActionButton9.onClick.AddListener(OnButtonAction9);
        m_InputFieldButton.onClick.AddListener(OnButtonInputFieldButton);

        // m_ARSessionOrigin    = GetARComponent<ARSessionOrigin>("ARSessionOrigin");
        // m_ARPlaneManager     = GetARComponent<ARPlaneManager>("ARSessionOrigin");
        // m_ARRayCastManager   = GetARComponent<ARRaycastManager>("ARSessionOrigin");
        // m_ARInputManager     = GetARComponent<ARInputManager>("ARSession");
        // m_ARSession          = GetARComponent<ARSession>("ARSession");
        // m_ARCameraManager    = GetARComponent<ARCameraManager>("ARCamera");
        // m_ARCameraBackground = GetARComponent<ARCameraBackground>("ARCamera");
    }

    private T GetARComponent<T>(string objectName)
    {
        GameObject obj = GameObject.Find(objectName);
        T component = obj.GetComponent<T>();
        return component;
    }

    private void OnButtonAction1()
    {
        // m_ARSessionOrigin.enabled = !m_ARSessionOrigin.enabled;
        // Debug.LogError("m_ARSessionOriginを" + m_ARSessionOrigin.enabled  + "にしました");
    }

    private void OnButtonAction2()
    {
        // m_ARPlaneManager.enabled = !m_ARPlaneManager.enabled;
        // Debug.LogError("m_ARPlaneManagerを" + m_ARPlaneManager.enabled  + "にしました");
    }

    private void OnButtonAction3()
    {
        // m_ARRayCastManager.enabled = !m_ARRayCastManager.enabled;
        // Debug.LogError("m_ARRayCastManagerを" + m_ARRayCastManager.enabled  + "にしました");
    }

    private void OnButtonAction4()
    {
        // m_ARInputManager.enabled = !m_ARInputManager.enabled;
        // Debug.LogError("m_ARInputManagerを" + m_ARInputManager.enabled  + "にしました");
    }

    private void OnButtonAction5()
    {
        // m_ARSession.enabled = !m_ARSession.enabled;
        // Debug.LogError("m_ARSessionを" + m_ARSession.enabled  + "にしました");
    }

    private void OnButtonAction6()
    {
        // m_ARCameraManager.enabled = !m_ARCameraManager.enabled;
        // Debug.LogError("m_ARCameraManagerを" + m_ARCameraManager.enabled  + "にしました");
    }

    private void OnButtonAction7()
    {
        // m_ARCameraBackground.enabled = !m_ARCameraBackground.enabled;
        // Debug.LogError("m_ARCameraBackgroundを" + m_ARCameraBackground.enabled  + "にしました");
    }

    private void OnButtonAction8()
    {
    }

    private void OnButtonAction9()
    {
    }

    private void OnButtonInputFieldButton()
    {
        int value = int.Parse(m_InputField.text);
#if UNITY_IOS && !UNITY_EDITOR
        playSystemSound(value);
#endif
    }

}
