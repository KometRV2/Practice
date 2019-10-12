using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	protected static T instance;
	public static T I {
		get {
			if (instance == null) {
				instance = (T)FindObjectOfType (typeof(T));
				
				if (instance == null) {
					Debug.LogWarning (typeof(T) + "is nothing");
				}
			}
			
			return instance;
		}
	}
	
	protected void Awake()
	{
		CheckInstance();
		OnAwake();
	}
	
	protected bool CheckInstance()
	{
		if( instance == null)
		{
			instance = (T)this;
			DontDestroyOnLoad(gameObject);
			return true;
		}else if( I == this )
		{
			DontDestroyOnLoad(gameObject);
			return true;
		}

		Destroy(this);
		Destroy(this.gameObject);
		return false;
	}

	public virtual void OnAwake(){}
}