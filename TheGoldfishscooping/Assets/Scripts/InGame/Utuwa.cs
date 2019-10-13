using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;

public class UtuwaParam
{
    public System.Action OnCompleteMoveUtuwaAction;
    public System.Action OnCompleteScoopAction;
}

public class Utuwa : MonoBehaviour
{
    //private Vector3 m_InitLocalPos;
    private static readonly Vector3 SCOOP_END_POS = new Vector3(0, -0.068f, 0.642f);

    private System.Action m_OnCompleteMoveUtuwaAction;
    private System.Action m_OnCompleteScoopAction;

    private Vector3 m_Position;
    private Vector3 m_InitLocalPosition;

    [SerializeField]
    private Vector3 toVec = new Vector3(-0.13f, -0.5f, 0.4f);

    private static readonly float WATER_HEIGHT = 0.04f;
    private bool m_IsScooping;
    private Transform m_GetFishRoot;
    public Transform GetFishRoot
    {
        get
        {
            return m_GetFishRoot;
        }
    }

    public void Initialize()
    {
        this.transform.SetParent(Camera.main.transform);
        this.transform.position = Camera.main.transform.position + toVec;
        //m_InitLocalPos = this.transform.position;
        m_InitLocalPosition = this.transform.localPosition;
        m_GetFishRoot = this.transform.Find("GetFishRoot");
    }

    public void SetParam(UtuwaParam param)
    {
        m_OnCompleteMoveUtuwaAction = param.OnCompleteMoveUtuwaAction;
        m_OnCompleteScoopAction = param.OnCompleteScoopAction;
    }

    void Update()
    {
        if(m_IsScooping)
        {
            return;
        }

        this.transform.localPosition = m_InitLocalPosition;
        if(this.transform.position.y < WATER_HEIGHT)
        {
            m_Position = this.transform.position;
            m_Position.y = WATER_HEIGHT;
            this.transform.position = m_Position;
        }
    }

    // public void OnUpdate()
    // {
    //     m_Position = this.transform.localPosition;
    //     if(m_Position.y < INIT_POS.y)
    //     {
    //         m_Position.y = INIT_POS.y;
    //         this.transform.localPosition = m_Position;
    //     }
    // }

    public void SetUtuwaPosition()
    {
        m_IsScooping = true;
        this.transform.DOLocalMove(SCOOP_END_POS, 0.3f).SetEase(Ease.Linear).OnComplete(() => 
        {
            m_OnCompleteMoveUtuwaAction?.Invoke();
            // DOVirtual.DelayedCall(0.7f, () => 
            // {
            //     EndScoop();
            // });
        });
    }

    public void EndScoop()
    {
        this.transform.DOLocalMove(m_InitLocalPosition, 0.3f).SetEase(Ease.Linear).OnComplete(() => 
        {
            m_OnCompleteScoopAction?.Invoke();
            m_IsScooping = false;
        });
        
    }
}
