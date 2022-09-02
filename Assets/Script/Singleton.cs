using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	#region static variable
	private static bool _isDestroy = false;
    private static object _lock = new object();
    private static T _instance;
	#endregion static variable

	#region property
	public static T Instance
	{
		get
		{
			if (_isDestroy)
			{
				return null;
			}
			lock (_lock)
			{
				if (_instance == null)
				{
					_instance = (T)FindObjectOfType(typeof(T));
					{
						if (_instance == null)
						{
							var singletonObject = new GameObject();
							_instance = singletonObject.AddComponent<T>();
							singletonObject.name = typeof(T).ToString() + "(SingleTon)";

							DontDestroyOnLoad(singletonObject);
						}
					}
				}
				return _instance;
			}
		}
	}
	#endregion property

	#region MonoBehaviour
	private void OnApplicationQuit()
	{
		_isDestroy = true;
	}
	private void OnDestroy()
	{
		_isDestroy = true;
	}
	#endregion MonoBehaviour
}
