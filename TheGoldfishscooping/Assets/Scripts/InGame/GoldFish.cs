using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG;
using DG.Tweening;

public class GoldFishParam
{
    public System.Action OnEndUtuwaMoveAction;
}

public class GoldFish : MonoBehaviour
{
    public enum FishState
    {
        NONE = -1,
        IDLE,
        SWIMMING,
        STRUGGLE,
        TOUTUWA,
        INUTUWA,
    }

    [SerializeField]
    private float RotateSpeed = 10f;

    private float m_Timer;
    private float m_NextInterval;
    private float m_MoveSpeed;
    private float m_Damage;
    public float Damage
    {
        get
        {
            return m_Damage;
        }
    }
    

    private FishState m_State = FishState.NONE;
    private Rigidbody m_RigidBody;
    
    private Vector3 m_MoveDis;
    private Vector3 m_MoveDir;
    private Vector3 m_NextTargetPos;
    private bool m_IsScooping;
    private bool m_IsScooped;

    [SerializeField]
    private Vector3 localGravity = Vector3.up * - 0.7f;

    private Transform m_GetFishRoot;
    private CapsuleCollider m_Collider;
    private System.Action m_OnEndUtuwaMoveAction;
    private Transform m_Owner;
    public void Initialize(int fishType)
    {
        m_State = FishState.IDLE;
        m_NextInterval = Random.Range(0, 3);
        m_RigidBody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<CapsuleCollider>();
        m_Owner = this.transform.parent;
        m_Damage = GoldFishManager.DAMAGE_FROM_TYPE[fishType];
    }

    public void SetParam(GoldFishManagerParam param)
    {
        m_GetFishRoot = param.GetFishRoot;
        m_OnEndUtuwaMoveAction = param.OnEndUtuwaMoveAction;
    }

    public void SetParentNormal()
    {
        this.transform.SetParent(m_Owner);
    }

    public void MoveToWaterByFail()
    {
        this.transform.DOLocalMoveY(0.03f, 0.6f);
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
            case FishState.TOUTUWA:
            break;
            case FishState.INUTUWA:
            break;
            default:
            break;
        }
    }

    private void FixedUpdate () 
    {
        if(m_IsScooping)
        {
            SetLocalGravity ();
        }
    }

    private void SetLocalGravity()
　　{
        m_RigidBody.AddForce (localGravity, ForceMode.Acceleration);
    }

    private void SetSwimInfo()
    {
        m_NextTargetPos = m_IsScooped ? GetUtuwaMovePos() : GetNextPos();
        m_MoveSpeed = Random.Range(0.03f, 0.06f);
        m_NextInterval = m_MoveDis.magnitude / m_MoveSpeed;
        //if(!m_IsScooped){return;}
        this.transform.DOLocalMove(m_NextTargetPos, m_NextInterval).OnUpdate(() => 
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

    private Vector3 GetUtuwaMovePos()
    {
        Vector2 randPos = Random.insideUnitCircle * 0.15f;
        Vector3 newPos = new Vector3(randPos.x, 0.04f, randPos.y);
        m_MoveDir = m_GetFishRoot.transform.position + newPos - this.transform.position;
        return newPos;
    }

    public void OnScoop()
    {
        m_State = FishState.STRUGGLE;
    }

    public void OutWater()
    {
        if(!SceneControlManager.CheckSceneName("Main"))
        {
            return;
        }
        m_IsScooping = true;
    }

    public void InWater()
    {
        if(!SceneControlManager.CheckSceneName("Main"))
        {
            return;
        }

        m_State = FishState.IDLE;
        m_IsScooping = false;
        StartCoroutine(StopRigid(() => 
        {
            Vector3 pos = this.transform.localPosition;
            pos.y = 0.02f;
            this.transform.localPosition = pos;
        }));
    }

    private IEnumerator StopRigid(System.Action onComlpete = null)
    {
        m_RigidBody.isKinematic = true;
        yield return null;
        m_RigidBody.isKinematic = false;
        Vector3 rot = this.transform.localEulerAngles;
        rot.x = 0f;
        rot.z = 0f;
        this.transform.localEulerAngles = rot;
        onComlpete?.Invoke();
    }

    [SerializeField]
    private float power = 0.001f;

    public void UpdateOnPoi()
    {
        // m_RigidBody.AddForce(new Vector3(power * TimeManager.I.DeltaTime, 0f, 0f), ForceMode.Force);
        // Debug.LogError("update");
    }

    public void ToUtuwa()
    {
        m_Collider.enabled = false;
        m_State = FishState.TOUTUWA;
        this.transform.SetParent(m_GetFishRoot);
        this.transform.DOLocalMoveY(0.1f, 0.5f).OnComplete(() => 
        {
            StartCoroutine(StopRigid(() => 
            {
                m_State = FishState.IDLE;
                m_IsScooping = false;
                m_IsScooped = true;
                m_Timer = 0f;
                m_OnEndUtuwaMoveAction?.Invoke();
            }));
        });
    }
}
