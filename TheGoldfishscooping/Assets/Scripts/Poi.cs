using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poi : MonoBehaviour
{
    private Transform m_Kami;
    private RaycastHit[] m_Hits;

    void Start()
    {
        m_Kami = transform.Find("kami");
        var obj1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj1.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        obj1.transform.position = m_Kami.position;
        obj1.name = "obj1";

        var obj2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj2.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        obj2.transform.position = m_Kami.position + m_Kami.up * 0.01f;
        obj2.name = "obj2";
    }

    private GameObject obj3;
    [SerializeField]
    private float radius = 0.01f;

    void Update()
    {
        m_Hits = Physics.SphereCastAll(m_Kami.position, radius, m_Kami.up.normalized, dis, 1 << LayerMask.NameToLayer("Fish"));
        if(m_Hits.Length > 0)
        {
            for(int i = 0, il = m_Hits.Length; i < il; i++)
            {
                if(obj3 == null)
                {
                    obj3 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    obj3.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                    obj3.transform.position = m_Hits[i].point;
                    obj3.name = "obj3";
                }

                Debug.LogError(m_Hits[i].point);
                GoldFish fish = m_Hits[i].collider.GetComponent<GoldFish>();
                fish.OnScoop();
            }
        }
    }

    [SerializeField]
    private float dis = 0.01f;

    void OnDrawGizmos()
    {
        if(m_Kami != null)
        {
            Gizmos.DrawRay (m_Kami.position, m_Kami.up * dis);
        }
    }
}
