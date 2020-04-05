using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CallAnt m_CallAnt = new CallAnt();
    private bool m_IsStart;
    
    void Awake()
    {
        m_CallAnt.OnAwake(this);
    }

    void Update()
    {
        if(!m_IsStart && Input.GetKeyDown(KeyCode.Return))
        {
            m_CallAnt.StartCallAnt();
            m_IsStart = true;
        }
    }
}
