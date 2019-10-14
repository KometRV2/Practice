using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoiParam
{
    public System.Action OnCheckScoopClearAction;
    public System.Action OnCheckScoopFailAction;
    public System.Action<float> OnUpdatePoiMeterAction;
    public System.Action OnBreakPoiAction;
}

public class Poi : MonoBehaviour
{
    private Transform m_Kami;

    private RaycastHit[] m_Hits;
    private System.Action m_OnCheckScoopClearAction;
    private System.Action m_OnCheckScoopFailAction;
    private System.Action m_OnBreakPoiAction;
    private System.Action<float> m_OnUpdatePoiMeterAction;

    private static readonly float POI_METER_HP = 100;
    private static readonly float SCOOP_CLEAR_HEIGHT = 0.18f;
    
    [SerializeField]
    private Transform m_ScoopFishRoot;

    [SerializeField]
    private float radius = 0.01f;

    [SerializeField]
    private float dis = 0.01f;
    private float m_PoiKamiHP;

    private Vector3 m_BeforPos;
    private Vector3 m_CalcVec;
    private Color m_KamiMatColor;
    private Rigidbody m_Rigid;
    private Material m_KamiMat;
    private List<GoldFish> m_ScoopGoldFishList = new List<GoldFish>();
    private InGameManager m_Owner;
    private bool m_IsIgnoreMinusHP;

    public void Initialize()
    {
        m_Owner = GetComponentInParent<InGameManager>();
        m_Kami = transform.Find("kami");

        transform.SetParent(Camera.main.transform);
        m_Rigid = GetComponent<Rigidbody>();
        m_PoiKamiHP = POI_METER_HP;
        MeshRenderer meshRenderer = m_Kami.GetComponent<MeshRenderer>();
        m_KamiMat = meshRenderer.materials[0];
        m_BeforPos = this.transform.position;
        m_Owner.RegisterFailAction(RetryAction);
    }

    public void SetParam(PoiParam param)
    {
        m_OnCheckScoopClearAction = param.OnCheckScoopClearAction;
        m_OnCheckScoopFailAction = param.OnCheckScoopFailAction;
        m_OnUpdatePoiMeterAction = param.OnUpdatePoiMeterAction;
        m_OnBreakPoiAction = param.OnBreakPoiAction;
    }

    public void SetIgnoreMinusHP(bool enable)
    {
        m_IsIgnoreMinusHP = enable;
    }

    private void RetryAction()
    {
        m_PoiKamiHP = POI_METER_HP;
        SetIgnoreMinusHP(false);
        Collider[] childCols = GetComponentsInChildren<Collider>(true);
        for(int i = 0, il = childCols.Length; i < il; i++)
        {
            childCols[i].enabled = true;
        } 
    }

    public void RotatePoi(float value)
    {
        Vector3 localAngle = this.transform.localEulerAngles;
        localAngle.z = value * 90f;
        this.transform.localEulerAngles = localAngle;
    }

    public void EndScoopRotatePoi()
    {
        Vector3 localAngle = this.transform.localEulerAngles;
        localAngle.z = 90f;
        this.transform.localEulerAngles = localAngle;
        for(int i = 0, il = m_ScoopGoldFishList.Count; i < il; i++)
        {
            m_ScoopGoldFishList[i].ToUtuwa();
        }
    }

    public void EndScoopFinalRotatePoi()
    {
        Vector3 localAngle = this.transform.localEulerAngles;
        localAngle.z = 0f;
        this.transform.localEulerAngles = localAngle;
    }

    private void MinusHP(float value)
    {
        if(m_IsIgnoreMinusHP)
        {
            return;
        }

        m_PoiKamiHP -= value;
        if(m_PoiKamiHP <= 0f)
        {
            m_PoiKamiHP = 0f;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(!SceneControlManager.CheckSceneName("Main"))
        {
            return;
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Fish"))
        {
            var goldFish = other.gameObject.GetComponent<GoldFish>();
            goldFish.OnScoop();
            if(!m_ScoopGoldFishList.Contains(goldFish))
            {
                m_ScoopGoldFishList.Add(goldFish);
            }
            goldFish.transform.SetParent(m_ScoopFishRoot);

            //上から触れた場合は即アウト
            float dirY = m_Kami.position.y - other.transform.position.y;
            if(dirY >= 0f)
            {
                MinusHP(100f);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(!SceneControlManager.CheckSceneName("Main"))
        {
            return;
        }
        if(!m_Owner.IsBreakPoi)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                MinusHP(0.1f);
            }
        }
    }

    void OnCollisionStay(Collision other)
    {
        if(!SceneControlManager.CheckSceneName("Main"))
        {
            return;
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Fish"))
        {
            GoldFish goldFish = other.gameObject.GetComponent<GoldFish>();
            MinusHP(goldFish.Damage);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if(!SceneControlManager.CheckSceneName("Main"))
        {
            return;
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Fish"))
        {
            var goldFish = other.gameObject.GetComponent<GoldFish>();
            if(m_ScoopGoldFishList.Contains(goldFish))
            {
                m_ScoopGoldFishList.Remove(goldFish);
            }
            m_OnCheckScoopFailAction?.Invoke();
            goldFish.SetParentNormal();
        }
    }

    public void OnUpdate()
    {
        if(!m_Owner.IsBreakPoi)
        {
            m_CalcVec = this.transform.position - m_BeforPos;
            m_BeforPos = this.transform.position;
            for(int i = 0, il = m_ScoopGoldFishList.Count; i < il; i++)
            {
                CheckScoopClear(m_ScoopGoldFishList[i]);
            }
            m_OnUpdatePoiMeterAction?.Invoke(m_PoiKamiHP / POI_METER_HP);
            m_KamiMatColor = m_KamiMat.color;
            m_KamiMatColor.a = m_PoiKamiHP / POI_METER_HP;
            m_KamiMat.color = m_KamiMatColor;

            if(m_PoiKamiHP <= 0f)
            {
                for(int i = 0, il = m_ScoopGoldFishList.Count; i < il; i++)
                {
                    m_ScoopGoldFishList[i].SetParentNormal();
                    m_ScoopGoldFishList[i].MoveToWaterByFail();
                }

                m_PoiKamiHP = 0f;
                m_OnBreakPoiAction?.Invoke();
                Collider[] childCols = GetComponentsInChildren<Collider>(true);
                for(int i = 0, il = childCols.Length; i < il; i++)
                {
                    childCols[i].enabled = false;
                }                
            }
        }
    }

    private void CheckScoopClear(GoldFish goldFish)
    {
        if(goldFish.transform.position.y > SCOOP_CLEAR_HEIGHT)
        {
            m_OnCheckScoopClearAction?.Invoke();
        }
        else
        {
            m_OnCheckScoopFailAction?.Invoke();
        }
    }
}
