using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFishParam
{
    public Dictionary<int, int> CreateFishCountDic;
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

    private List<GoldFish> m_GoldFishList = new List<GoldFish>();
    private int m_CreateFishCount = 10;

    private Dictionary<int, GameObject> m_GoldFishRef = new Dictionary<int, GameObject>();
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

    void Start()
    {
        Dictionary<int, int> createFishCountDic = new Dictionary<int, int>();
        createFishCountDic[0] = 1;
        // createFishCountDic[1] = 3;
        // createFishCountDic[2] = 2;

        CreateFishParam createFishParam = new CreateFishParam()
        {
            CreateFishCountDic = createFishCountDic
        };
        OnStart(createFishParam);
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
        goldFish.Initialize();
        m_GoldFishList.Add(goldFish);
        SetTransform(createFish.transform);
    }

    private void SetTransform(Transform fishTrans)
    {
        float rand = Random.value;
        fishTrans.localPosition = new Vector3(0, 0.0085f,0);//GetRandomPosInAquarium();
        fishTrans.localRotation = (rand < 0.5f) ? Quaternion.Euler(0, -90, 0) : Quaternion.Euler(0, 90, 0);
    }

    void Update()
    {
        for(int i = 0, il = m_GoldFishList.Count; i < il; i++)
        {
            //m_GoldFishList[i].OnUpdate();
        }
    }
}
