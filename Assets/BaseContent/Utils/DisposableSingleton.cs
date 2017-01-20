using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisposableSingleton<T> : MonoBehaviour where T: class {
	
	protected static T _instance = null;
	
	//===================================================
	public static T Instance
	{
		get {
			if(_instance == null)
				_instance = FindObjectOfType(typeof(T)) as T;
			
			if( _instance == null ) {
				Debug.LogError( "DisposableSingleton: " + typeof(T) + " instance is null." );
			}
			return _instance;
		}
	}

    public static bool Exists { get { return _instance != null; } }
	
	//===================================================
	public static void Instantiate()
	{
		_instance = Instance;
		SingletonManager.AddDisposableSingleton(_instance as Object);
	}
	
	//===================================================
	private void Awake()
	{
		if( _instance == this as T ) {
			// somebody has already used the instance getter, so we skip this...
			return;
		}
		if( _instance != null ) {
			Debug.LogError(string.Format( "{0}: There are two instances of {0}", typeof(T) ));
		}
		Instantiate();
	}
	
	//===================================================
	protected virtual void OnDestroy () {
		if (_instance == null) {
			return;
		}
		
		SingletonManager.RemoveDisposableSingleton((_instance as Object).ToString(),false);
		_instance = null;
	}
}
