using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneControlManager : Singleton<SceneControlManager>
{
    private enum State
    {
        NONE = -1,
        FADE_IN = 0,
        FADE_OUT = 1,
        LOADING = 2,
    }

    [SerializeField]
    private Image m_Fader;

    [SerializeField]
    private GameObject m_LoadContents;

    [SerializeField]
    private Slider m_LoadSlider;

    private AsyncOperation m_Async;
    private State m_State = State.NONE;
    private Color m_FadeColor;
    private string m_LoadSceneName;

    private void Update()
    {
        if(m_State == State.NONE)
        {
            return;
        }

        switch(m_State)
        {
            case State.FADE_IN:
                m_FadeColor = m_Fader.color;
                m_FadeColor += new Color(0,0,0, Time.deltaTime);
                m_Fader.color = m_FadeColor;
                CheckLoadScene();
            break;
            case State.FADE_OUT:
                m_FadeColor = m_Fader.color;
                m_FadeColor -= new Color(0,0,0, Time.deltaTime);
                m_Fader.color = m_FadeColor;
                CheckFadeOutScene();
            break;
        }
    }

    private void CheckLoadScene()
    {
        if(m_Fader.color.a >= 1)
        {
            m_State = State.LOADING;
            m_LoadContents.SetActive(true);
            StartCoroutine(LoadSceneAsync(m_LoadSceneName));
        }
    }

    /// <summary>
    /// フェード明け
    /// </summary>
    private void CheckFadeOutScene()
    {
        if(m_Fader.color.a <= 0f)
        {
            m_State = State.NONE;
            m_Async = null;
            StartNextSceneControll(SceneManager.GetActiveScene().name);
        }
    }

    public void LoadScene(string sceneName)
    {
        m_State = State.FADE_IN;
        m_LoadSceneName = sceneName;
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        m_Async = SceneManager.LoadSceneAsync(sceneName);
        while(!m_Async.isDone)
        {
            var progressVal = Mathf.Clamp01(m_Async.progress / 0.9f);
            m_LoadSlider.value = progressVal;
            yield return null;
        }
        m_State = State.FADE_OUT;
        m_LoadContents.SetActive(false);
    }

    private void StartNextSceneControll(string nextSceneName)
    {
        switch(nextSceneName)
        {
            case "Main":
                GameObject inGameManagerObj = GameObject.Find("InGameManager");
                // InGameManager inGameManager = inGameManagerObj.GetComponent<InGameManager>();
                // inGameManager.OnStart(m_InGameParam);
            break;
            default:
            break;
        }
    }
}
