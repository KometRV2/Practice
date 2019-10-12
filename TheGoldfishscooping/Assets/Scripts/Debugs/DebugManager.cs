using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : Singleton<DebugManager>
{
    private Touch m_Touch;

    [SerializeField]
    private RectTransform m_Parent;

    private DebugMenuTop m_DebugMenuTop;

    private void Update()
    {
        if(Application.isEditor)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                CreateDebugMenu();
            }
        }
        else
        {
            if (Input.touchCount > 0) 
            {
                this.m_Touch = Input.touches[0];

                if (this.m_Touch.phase == TouchPhase.Began)
                { 
                    switch (Input.touchCount)
                    {
    　　　　　　　　　　　  case 3 : // 3本指でタッチ
                            CreateDebugMenu();
                        break;
                    }
                }
            }
        }
    }

    private void CreateDebugMenu()
    {
        if(m_DebugMenuTop == null)
        {
            m_DebugMenuTop = (DebugMenuTop)DebugContentsBase.CreateDebugContentsPage("Prefabs/Debug/DebugMenuTop", "デバッグメニュートップ", m_Parent);
        }
        else
        {
            Destroy(m_DebugMenuTop.gameObject);
            m_DebugMenuTop = null;
        }
    }
}
