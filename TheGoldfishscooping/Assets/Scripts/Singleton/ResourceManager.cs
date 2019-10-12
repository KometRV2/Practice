using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static T CreateUI<T> (string name, Transform parent = null)
    {
        GameObject refObj = (GameObject)Resources.Load("Prefabs/UI/" + name);
        GameObject Obj = GameObject.Instantiate(refObj, Constants.VECTOR3_ZERO, Quaternion.identity, parent);
        Obj.transform.localPosition = Constants.VECTOR3_ZERO;
		return Obj.GetComponent<T>();
	}

    public static T CreateObject<T> (string path, Transform parent = null)
    {
        GameObject refObj = (GameObject)Resources.Load(path);
        GameObject Obj = GameObject.Instantiate(refObj, Constants.VECTOR3_ZERO, Quaternion.identity);
        Obj.transform.localPosition = Constants.VECTOR3_ZERO;
        Obj.transform.localScale = Constants.VECTOR3_ONE;
        Obj.transform.SetParent(parent);
		return Obj.GetComponent<T>();
	}

    public static GameObject CreateGameObject(string path, Transform parent = null)
    {
        GameObject refObj = (GameObject)Resources.Load(path);
        return GameObject.Instantiate(refObj, Constants.VECTOR3_ZERO, Quaternion.identity, parent);
    }

    public static GameObject LoadObject(string path)
    {
        GameObject refObj = (GameObject)Resources.Load("Prefabs/" + path);
        return refObj;
    }
}
