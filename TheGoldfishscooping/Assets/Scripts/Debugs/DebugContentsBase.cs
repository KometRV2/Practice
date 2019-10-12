using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugContentsBase : MonoBehaviour
{
    public virtual void Initialize()
    {
        Button exitButton = this.transform.Find("ExitButton").GetComponent<Button>();
        exitButton.onClick.AddListener(() => 
        {
            Destroy(this.gameObject);
            Destroy(this);
        });
    }

    public static DebugContentsBase CreateDebugContentsPage(string prefabPath, string titleName, Transform parent)
    {
        DebugContentsBase contentsBase = ResourceManager.CreateObject<DebugContentsBase>(prefabPath, parent);
        contentsBase.Initialize();
        Text titleText = contentsBase.transform.Find("TitleText").GetComponent<Text>();
        titleText.text = titleName;
        return contentsBase;
    }
}
