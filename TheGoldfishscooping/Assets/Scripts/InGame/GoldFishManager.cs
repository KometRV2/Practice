using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFishParam
{
    public Dictionary<int, int> CreateFishCountDic;
}

public class GoldFishManagerParam
{
    public Transform GetFishRoot;
    public System.Action OnEndUtuwaMoveAction;
}

public class GoldFishManager : MonoBehaviour
{
    public enum FISH_TYPE
    {
        NONE = -1,
        RED,
        MEDIUM,
        BLACK,
        MAX,
    }

    public static readonly float[] DAMAGE_FROM_TYPE = new float[3]{0.1f, 0.2f, 0.3f};
    private int m_GetFishCount;
    public int GetFishCount
    {
        get
        {
            return m_GetFishCount;
        }
    }

    private int m_CreateFishCount;
    public int CreateFishCount
    {
        get
        {
            return m_CreateFishCount;
        }
    }

    public int RemainFishCount
    {
        get
        {
            return m_CreateFishCount - m_GetFishCount;
        }
    }
    

    private List<GoldFish> m_GoldFishList = new List<GoldFish>();
    private Dictionary<int, GameObject> m_GoldFishRef = new Dictionary<int, GameObject>();
    private Dictionary<int, int> m_CreateFishCountDic = new Dictionary<int, int>(){};
    
    private Transform m_FishRoot;
    public static readonly float[] AQUARIUM_RANGE_X = new float[2]{-0.8f, 0.8f};
    public static readonly float[] AQUARIUM_RANGE_Y = new float[2]{0.04f, 0.05f};
    public static readonly float[] AQUARIUM_RANGE_Z = new float[2]{-0.38f, 0.38f};

    public static Vector3 GetRandomPosInAquarium()
    {
        return new Vector3(
            Random.Range(AQUARIUM_RANGE_X[0], AQUARIUM_RANGE_X[1]), 
            Random.Range(AQUARIUM_RANGE_Y[0], AQUARIUM_RANGE_Y[1]), 
            Random.Range(AQUARIUM_RANGE_Z[0], AQUARIUM_RANGE_Z[1]));
    }

    public void InitializeFromTitle()
    {
        m_CreateFishCountDic[0] = 10;
        m_CreateFishCountDic[1] = 10;
        m_CreateFishCountDic[2] = 10;
        foreach (var key in m_CreateFishCountDic.Keys) 
        {
            m_CreateFishCount += m_CreateFishCountDic[key];
		} 
        CreateFishParam createFishParam = new CreateFishParam()
        {
            CreateFishCountDic = m_CreateFishCountDic
        };
        OnStart(createFishParam);
    }

    public void Initialize()
    {
        m_CreateFishCountDic[0] = 5;
        m_CreateFishCountDic[1] = 3;
        m_CreateFishCountDic[2] = 2;
        foreach (var key in m_CreateFishCountDic.Keys) 
        {
            m_CreateFishCount += m_CreateFishCountDic[key];
		} 


        CreateFishParam createFishParam = new CreateFishParam()
        {
            CreateFishCountDic = m_CreateFishCountDic
        };
        OnStart(createFishParam);
    }

    public void SetParam(GoldFishManagerParam param)
    {
        for(int i = 0, il = m_GoldFishList.Count; i < il; i++)
        {
            m_GoldFishList[i].SetParam(param);
        }
    }

    public void OnStart(CreateFishParam createFishCountDic)
    {
        m_FishRoot = this.transform.Find("GoldFishRoot");

        //ロード
        for(int i = 0; i < (int)FISH_TYPE.MAX; i++)
        {
            m_GoldFishRef[i] = ResourceManager.LoadObject("GoldFishs/GOLDFISH_" + i);
        }

        //生成
        foreach (var key in createFishCountDic.CreateFishCountDic.Keys) 
        {
            for(int i = 0, il = createFishCountDic.CreateFishCountDic[key]; i < il; i++)
            {
                InitializeGoldFish(key);
            }
		}        
    }

    private void InitializeGoldFish(int key)
    {
        GameObject createFish = GameObject.Instantiate(m_GoldFishRef[key], m_FishRoot);
        GoldFish goldFish = createFish.AddComponent<GoldFish>();
        goldFish.Initialize(key);
        m_GoldFishList.Add(goldFish);
        SetTransform(createFish.transform);
    }

    private void SetTransform(Transform fishTrans)
    {
        float rand = Random.value;
        fishTrans.localPosition = /*new Vector3(0, 0.02f,-0.261f);//*/GetRandomPosInAquarium();
        fishTrans.localRotation = (rand < 0.5f) ? Quaternion.Euler(0, -90, 0) : Quaternion.Euler(0, 90, 0);
    }

    void Update()
    {
        for(int i = 0, il = m_GoldFishList.Count; i < il; i++)
        {
            m_GoldFishList[i].OnUpdate();
        }
    }

    public void GetFish()
    {
        m_GetFishCount++;
        if(m_GetFishCount == m_GoldFishList.Count)
        {
            Debug.LogError("クリア");
        }
    }
}
