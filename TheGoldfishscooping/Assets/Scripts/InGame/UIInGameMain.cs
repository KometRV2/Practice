using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInGameMainParam
{
    public System.Action OnUtuwaAction;
}

public class UIInGameMain : MonoBehaviour
{
    [SerializeField]
    private Slider m_PoiSlider;

    [SerializeField]
    private Button m_UtuwaButton;

    private Image m_UtuwaButtonImage;
    private System.Action<float> m_OnSliderAction;
    private System.Action m_OnUtuwaAction;
    private bool m_IsWait;

    public void Initialize(UIInGameMainParam param)
    {
        m_OnUtuwaAction = param.OnUtuwaAction;
        m_PoiSlider.onValueChanged.AddListener(OnSliderAction);
        m_UtuwaButton.onClick.AddListener(OnClickUtuwaButton);

        m_UtuwaButtonImage = m_UtuwaButton.image;
        ChangeScoopImageFail();
    }

    public void SetSliderAction(System.Action<float> sliderAction)
    {
        m_OnSliderAction = sliderAction;
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
        } 
    }

    public void ChangeScoopImageFail()
    {
        Color color = m_UtuwaButtonImage.color;
        color.a = 0.5f;
        m_UtuwaButtonImage.color = color;
    }
}
