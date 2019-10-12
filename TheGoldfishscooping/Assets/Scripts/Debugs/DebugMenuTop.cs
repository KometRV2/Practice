using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMenuTop : DebugContentsBase
{
    [SerializeField]
    private RectTransform m_ContentsButtonRoot;
    
    [SerializeField]
    private RectTransform m_ContentsPageRoot;

    [SerializeField]
    private GameObject m_DebugContentsButtonRef;

    public override void Initialize()
    {
        base.Initialize();
        CreateContentsButton("ログ表示", "Prefabs/Debug/DebugLogPage");
        CreateContentsButton("システム表示", "Prefabs/Debug/DebugSystemPage");
        CreateContentsButton("デバッグアクション表示", "Prefabs/Debug/DebugActionPage");
        CreateContentsButton("クリアフラグ\n表示", "Prefabs/Debug/DebugClearFlagPage");
    }

    private void CreateContentsButton(string contentsName, string prefabName)
    {
        GameObject buttonObj = GameObject.Instantiate(m_DebugContentsButtonRef, Constants.VECTOR3_ZERO, Quaternion.identity, m_ContentsButtonRoot);
        Button button = buttonObj.GetComponent<Button>();
        Text buttonText = buttonObj.gameObject.GetComponentInChildren<Text>(true);
        buttonText.text = contentsName;
        button.onClick.AddListener(() => 
        {
            DebugContentsBase.CreateDebugContentsPage(prefabName, contentsName, m_ContentsPageRoot);
        });
    }
}
