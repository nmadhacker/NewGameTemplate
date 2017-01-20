using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingletonManager {
	
	public static Dictionary<string,Object> disposableSingletons {get;private set;}
	
	public static void AddDisposableSingleton(Object disposableSingleton) {
		if (disposableSingletons == null) {
			disposableSingletons = new Dictionary<string, Object>();
		}
		
		disposableSingletons[disposableSingleton.ToString()] = disposableSingleton;
	}
	public static void RemoveDisposableSingleton(string disposableSingletonName, bool destroy) {
		
		if (disposableSingletons == null) {
			Debug.LogWarning(string.Format("Unable to remove: {0}, because list of singletons is empty",disposableSingletonName));
			return;				
		}
		
		if (disposableSingletons.ContainsKey(disposableSingletonName)) {
			if (destroy) {
				GameObject.DestroyImmediate(disposableSingletons[disposableSingletonName]); // destroy singleton
				Debug.Log(string.Format("Deleted instance of: {0}",disposableSingletonName));
			}
			disposableSingletons.Remove(disposableSingletonName);
		}
	}

	/// <summary>
	/// Removes all disposable singletons
	/// </summary>
	public static void RemoveAllDisposableSingletons() {
		Dictionary<string, UnityEngine.Object> objectsToDestroy = SingletonManager.disposableSingletons;
		if (objectsToDestroy == null || objectsToDestroy.Count == 0)
		{ // sanity check
			return;
		}

		string[] keys = objectsToDestroy.Keys.ToArray();
		foreach (string item in keys)
		{
			SingletonManager.RemoveDisposableSingleton(item, true);
		}
	}
	
	private static GameObject _gameObject = null;
	public static GameObject gameObject {
		get {
			if(_gameObject == null) {
				// note that this object name is used by iOS native code to find Singletons.
				// If you change it, be sure to change references in native code too!
				_gameObject = new GameObject("-SingletonManager");
				Object.DontDestroyOnLoad(_gameObject);
        _gameObject.hideFlags = HideFlags.DontSaveInEditor;
				_gameObject.AddComponent<DeleteOnEdit>();
			}
			return _gameObject;
		}
	}
}