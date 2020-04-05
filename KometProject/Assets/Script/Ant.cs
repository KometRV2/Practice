using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    private Vector3 m_MoveDir;

    [SerializeField]
    private float m_Speed = 1.0f;

    public void Initialize(Vector3 dir)
    {
        m_MoveDir = dir;
    }

    void Update()
    {
        transform.position += m_MoveDir * m_Speed * Time.deltaTime;
    }
}
