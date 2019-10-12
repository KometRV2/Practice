using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;

public class GoldFish : MonoBehaviour
{
    public enum FishState
    {
        NONE = -1,
        IDLE,
        SWIMMING,
        STRUGGLE,
    }

    [SerializeField]
    private float RotateSpeed = 10f;
    private float m_Timer;
    private float m_NextInterval;
    private float m_MoveSpeed;

    private FishState m_State = FishState.NONE;
    private Rigidbody m_RigidBody;
    
    private Vector3 m_MoveDis;
    private Vector3 m_MoveDir;
    private Vector3 m_NextTargetPos;

    public void Initialize()
    {
        m_State = FishState.IDLE;
        m_NextInterval = Random.Range(0, 3);
        m_RigidBody = GetComponent<Rigidbody>();
    }

    public void OnUpdate()
    {
        m_Timer += TimeManager.I.DeltaTime;
        switch(m_State)
        {
            case FishState.IDLE:
                if(m_Timer > m_NextInterval)
                {
                    m_State = FishState.SWIMMING;
                    SetSwimInfo();
                    m_Timer = 0f;
                }
            break;
            case FishState.SWIMMING:
            break;
            case FishState.STRUGGLE:
            break;
            default:
            break;
        }
    }

    private void SetSwimInfo()
    {
        m_NextTargetPos = GetNextPos();
        m_MoveSpeed = Random.Range(0.03f, 0.06f);
        m_NextInterval = m_MoveDis.magnitude / m_MoveSpeed;
        m_RigidBody.DOMove(m_NextTargetPos, m_NextInterval).OnUpdate(() => 
        {
            Quaternion rot = Quaternion.LookRotation(m_MoveDir);
            rot = Quaternion.Slerp(this.transform.rotation, rot, Time.deltaTime * RotateSpeed);
            m_RigidBody.rotation = rot;
            }).OnComplete(() => 
        {
            m_State = FishState.IDLE;
            m_Timer = 0f;
        });
    }

    private Vector3 GetNextPos()
    {
        m_MoveDis = new Vector3(Random.Range(-0.3f, 0.3f), 0f, Random.Range(-0.05f, 0.05f));
        Vector3 nextPos = this.transform.localPosition + m_MoveDis;
        while(Mathf.Abs(nextPos.x) > Mathf.Abs(GoldFishManager.AQUARIUM_RANGE_X[0]) || Mathf.Abs(nextPos.z) > Mathf.Abs(GoldFishManager.AQUARIUM_RANGE_Z[0]))
        {
            m_MoveDis = new Vector3(Random.Range(-0.3f, 0.3f), 0f, Random.Range(-0.05f, 0.05f));
            nextPos = this.transform.localPosition + m_MoveDis;
        }
        m_MoveDir = nextPos - this.transform.position;
        return nextPos;
    }

    public void OnScoop()
    {
        Debug.LogError(this.gameObject.name);
    }
}
