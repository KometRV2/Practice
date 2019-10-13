using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoiParam
{
    public System.Action OnCheckScoopClearAction;
    public System.Action OnCheckScoopFailAction;
}

public class Poi : MonoBehaviour
{
    private Transform m_Kami;
    private RaycastHit[] m_Hits;
    private System.Action m_OnCheckScoopClearAction;
    private System.Action m_OnCheckScoopFailAction;

    private static readonly float SCOOP_CLEAR_HEIGHT = 0.16f;
    private GameObject obj3;
    [SerializeField]
    private float radius = 0.01f;

    [SerializeField]
    private float dis = 0.01f;

    private List<GoldFish> m_ScoopGoldFishList = new List<GoldFish>();
    public void Initialize()
    {
        m_Kami = transform.Find("kami");
        transform.SetParent(Camera.main.transform);
        // var obj1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // obj1.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        // obj1.transform.position = m_Kami.position;
        // obj1.name = "obj1";

        // var obj2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // obj2.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        // obj2.transform.position = m_Kami.position + m_Kami.up * 0.01f;
        // obj2.name = "obj2";
    }

    public void SetParam(PoiParam param)
    {
        m_OnCheckScoopClearAction = param.OnCheckScoopClearAction;
        m_OnCheckScoopFailAction = param.OnCheckScoopFailAction;
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

    void OnDrawGizmos()
    {
        if(m_Kami != null)
        {
            Gizmos.DrawRay (m_Kami.position, m_Kami.up * dis);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Fish"))
        {
            var goldFish = other.gameObject.GetComponent<GoldFish>();
            goldFish.OnScoop();
            if(!m_ScoopGoldFishList.Contains(goldFish))
            {
                m_ScoopGoldFishList.Add(goldFish);
            }
        }
    }

    void OnCollisionStay(Collision other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Fish"))
        {
            GoldFish goldFish = other.gameObject.GetComponent<GoldFish>();
            if(m_ScoopGoldFishList.Contains(goldFish))
            {
                goldFish.UpdateOnPoi();
                CheckScoopClear(goldFish);
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Fish"))
        {
            var goldFish = other.gameObject.GetComponent<GoldFish>();
            if(m_ScoopGoldFishList.Contains(goldFish))
            {
                m_ScoopGoldFishList.Remove(goldFish);
            }
            m_OnCheckScoopFailAction?.Invoke();
        }
    }

    private void CheckScoopClear(GoldFish goldFish)
    {
        if(goldFish.transform.localPosition.y > SCOOP_CLEAR_HEIGHT)
        {
            m_OnCheckScoopClearAction?.Invoke();
        }
        else
        {
            m_OnCheckScoopFailAction?.Invoke();
        }
    }
}
