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
        // var obj1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // obj1.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        // obj1.transform.position = m_Kami.position;
        // obj1.name = "obj1";

        // var obj2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // obj2.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        // obj2.transform.position = m_Kami.position + m_Kami.up * 0.01f;
        // obj2.name = "obj2";
    }

    private GameObject obj3;
    [SerializeField]
    private float radius = 0.01f;

    [SerializeField]
    private float dis = 0.01f;

    private GoldFish m_GoldFish;

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
            m_GoldFish = other.gameObject.GetComponent<GoldFish>();
            m_GoldFish.OnScoop();
        }
    }

    void OnCollisionStay(Collision other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Fish"))
        {
            GoldFish goldFish = other.gameObject.GetComponent<GoldFish>();
            if(m_GoldFish == goldFish)
            {
                m_GoldFish.UpdateOnPoi();
            }
        }
    }
}
