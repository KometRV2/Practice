using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInGameMainParam
{
    public System.Action<float> SliderAction;
    public System.Action UtuwaAction;
}

public class UIInGameMain : MonoBehaviour
{
    [SerializeField]
    private Slider m_PoiSlider;

    [SerializeField]
    private Image m_PoiMeter;

    [SerializeField]
    private Button m_UtuwaButton;

    private Image m_UtuwaButtonImage;
    private System.Action<float> m_OnSliderAction;
    private System.Action m_OnUtuwaAction;
    private bool m_IsWait;
    private Vector3 m_MeterScale;

    public void Initialize()
    {
        m_PoiSlider.onValueChanged.AddListener(OnSliderAction);
        m_UtuwaButton.onClick.AddListener(OnClickUtuwaButton);

        m_UtuwaButtonImage = m_UtuwaButton.image;
        ChangeScoopImageFail();
    }

    public void SetParam(UIInGameMainParam param)
    {
        m_OnUtuwaAction = param.UtuwaAction;
        m_OnSliderAction = param.SliderAction;
    }

    private void OnSliderAction(float value)
    {
        m_OnSliderAction?.Invoke(value);
    }

    private void OnClickUtuwaButton()
    {
        m_OnUtuwaAction?.Invoke();
        ChangeScoopImageFail();
        m_IsWait = true;
        DG.Tweening.DOVirtual.DelayedCall(1.0f, () => 
        {
            m_IsWait = false;
        });
    }

    public void ChangeScoopImageClear()
    {
        if(!m_IsWait)
        {
            Color color = m_UtuwaButtonImage.color;
            color.a = 1.0f;
            m_UtuwaButtonImage.color = color;
            m_UtuwaButton.enabled = true;
        } 
    }

    public void ChangeScoopImageFail()
    {
        Color color = m_UtuwaButtonImage.color;
        color.a = 0.5f;
        m_UtuwaButtonImage.color = color;
        m_UtuwaButton.enabled = false;
    }

    public void UpdatePoiMeter(float value)
    {
        m_MeterScale = m_PoiMeter.transform.localScale;
        m_MeterScale.x = value;
        m_PoiMeter.transform.localScale = m_MeterScale;
    }
}
