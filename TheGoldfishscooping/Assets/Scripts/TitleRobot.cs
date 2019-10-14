using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleRobot : MonoBehaviour
{
    [SerializeField]
    private Transform m_RightArmRotRoot;
    Vector3 m_Rot;
    float time;

    private float m_Speed = 5.0f;
    void Update()
    {
        time += TimeManager.I.DeltaTime;
        float f = 13.5f * Mathf.Sin(time * m_Speed) + 1.5f;
        m_Rot = m_RightArmRotRoot.localEulerAngles;
        m_Rot.z = f;
        m_RightArmRotRoot.localEulerAngles = m_Rot;
    }
}
