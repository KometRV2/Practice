using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private Transform m_InGameUIStacker;
    private UIInGameMain m_UIInGameMain;
    private GoldFishManager m_GoldFishManager;
    private Poi m_Poi;
    private Utuwa m_Utuwa;

    private T GetUIClass<T>(string path)
    {
        Transform targetUITrans = m_InGameUIStacker.Find(path);
        if(targetUITrans == null)
        {
            Debug.LogError(path + "が存在しません");
        }
        T getUIClass = targetUITrans.GetComponent<T>();
        if(getUIClass == null)
        {
            Debug.LogError(getUIClass + "がアタッチさせていません");
        }
		return getUIClass;
    }

    void Start()
    {
        m_InGameUIStacker = this.transform.Find("InGameUIStacker");
        m_UIInGameMain = GetUIClass<UIInGameMain>("UIInGameMain");
        m_GoldFishManager = this.transform.Find("GoldFishManager").GetComponent<GoldFishManager>();
        m_Poi = this.transform.Find("Poi").GetComponent<Poi>();
        m_Utuwa = this.transform.Find("Utuwa").GetComponent<Utuwa>();

        m_UIInGameMain.Initialize();
        m_GoldFishManager.Initialize();
        m_Poi.Initialize();
        m_Utuwa.Initialize();

        GoldFishManagerParam goldFishManagerParam = new GoldFishManagerParam()
        {
            GetFishRoot = m_Utuwa.GetFishRoot,
            OnEndUtuwaMoveAction = m_Utuwa.EndScoop,
        };
        m_GoldFishManager.SetParam(goldFishManagerParam);

        PoiParam poiParam = new PoiParam()
        {
            OnCheckScoopClearAction = m_UIInGameMain.ChangeScoopImageClear,
            OnCheckScoopFailAction = m_UIInGameMain.ChangeScoopImageFail,
        };
        m_Poi.SetParam(poiParam);

        UtuwaParam utuwaParam = new UtuwaParam()
        {
            OnCompleteMoveUtuwaAction = () => 
            {
                m_Poi.EndScoopRotatePoi();

            },
            OnCompleteScoopAction = m_Poi.EndScoopFinalRotatePoi,
        };
        m_Utuwa.SetParam(utuwaParam);

        UIInGameMainParam inGameMainParam = new UIInGameMainParam()
        {
            UtuwaAction = m_Utuwa.SetUtuwaPosition,
            SliderAction = m_Poi.RotatePoi,
        };
        m_UIInGameMain.SetParam(inGameMainParam);
    }

    void Update()
    {
        
    }
}
