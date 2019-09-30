using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookChara : MonoBehaviour
{
    [SerializeField]
    private Transform m_LookTarget;
    
    private Vector3 m_Dir;
    void Update()
    {
        m_Dir = m_LookTarget.position - this.transform.position;
        this.transform.forward = m_Dir;
    }
}
