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
        
        m_Utuwa = this.transform.Find("Utuwa").GetComponent<Utuwa>();
        m_Utuwa.transform.SetParent(Camera.main.transform);

        UIInGameMainParam inGameMainParam = new UIInGameMainParam()
        {
            OnUtuwaAction = null,
        };
        m_UIInGameMain.Initialize(inGameMainParam);

        m_Poi = this.transform.Find("Poi").GetComponent<Poi>();
        PoiParam poiParam = new PoiParam()
        {
            OnCheckScoopClearAction = m_UIInGameMain.ChangeScoopImageClear,
        };
        m_Poi.Initialize(poiParam);
        m_Poi.transform.SetParent(Camera.main.transform);
        m_UIInGameMain.SetSliderAction(m_Poi.RotatePoi);
    }

    void Update()
    {
        
    }
}
