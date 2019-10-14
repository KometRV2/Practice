using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUIManager : MonoBehaviour
{
    [SerializeField]
    private Text m_TapToStartText;

    [SerializeField]
    private Button m_TapToStartButton;

    private float m_Time;
    private float m_Speed = 1.0f;

    void Start()
    {
        m_TapToStartButton.onClick.AddListener(OnClickTapToStartButton);

        GoldFishManager fishManager = ResourceManager.CreateObject<GoldFishManager>("Prefabs/GoldFishManager");
        fishManager.InitializeFromTitle();
    }

    void Update()
    {
        m_TapToStartText.color = GetAlphaColor (m_TapToStartText.color);
    }

    private void OnClickTapToStartButton()
    {
        SceneControlManager.I.LoadScene("Main");
        SoundManager.I.PlayButtonSE();
    }

    //Alpha値を更新してColorを返す
    private Color GetAlphaColor (Color color) 
    {
        m_Time += Time.deltaTime * 5.0f * m_Speed;
        color.a = Mathf.Sin (m_Time) * 0.5f + 0.5f;

        return color;
    }
}
