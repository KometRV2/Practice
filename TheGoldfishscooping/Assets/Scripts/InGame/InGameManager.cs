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
    private bool m_IsBreakPoi;
    public bool IsBreakPoi
    {
        get
        {
            return m_IsBreakPoi;
        }
    }

    private System.Action m_FailAction;
    public void RegisterFailAction(System.Action action)
    {
        m_FailAction += action;
    }

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
            OnEndUtuwaMoveAction = () => 
            {
                m_Utuwa.EndScoop();
                m_GoldFishManager.GetFish();
                m_UIInGameMain.UpdateLimitFishCount(m_GoldFishManager.RemainFishCount);
                if(m_GoldFishManager.RemainFishCount == 0)
                {
                    m_UIInGameMain.OnClearResult();
                }
            },
        };
        m_GoldFishManager.SetParam(goldFishManagerParam);

        PoiParam poiParam = new PoiParam()
        {
            OnCheckScoopClearAction = m_UIInGameMain.ChangeScoopImageClear,
            OnCheckScoopFailAction = m_UIInGameMain.ChangeScoopImageFail,
            OnUpdatePoiMeterAction = m_UIInGameMain.UpdatePoiMeter,
            OnBreakPoiAction = OnFailAction
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
            UtuwaAction = () => 
            {
                m_Utuwa.SetUtuwaPosition();
                m_Poi.SetIgnoreMinusHP(true);
            },
            SliderAction = m_Poi.RotatePoi,
            FishCount = m_GoldFishManager.CreateFishCount
        };
        m_UIInGameMain.SetParam(inGameMainParam);
    }

    private void Update()
    {
        m_Poi.OnUpdate();
        m_Utuwa.OnUpdate();
    }

    private void OnFailAction()
    {
        m_IsBreakPoi = true;
        UIManager.I.CreateYesNoDialog("やぶれた！\nやりなおしますか？", () => 
        {
            m_IsBreakPoi = false;
            m_FailAction?.Invoke();
            SoundManager.I.PlayButtonSE();
        }, () => 
        {
            SceneControlManager.I.LoadScene("Main");
            SoundManager.I.PlayButtonSE();
        });
    }
}
