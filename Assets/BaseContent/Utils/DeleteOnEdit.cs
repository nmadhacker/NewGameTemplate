using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DeleteOnEdit : MonoBehaviour {
	
	//Keep a reference to avoid an exception.
	private string gameObjectName;
	
	void Start() {
		gameObjectName = name;
	}
	
	#if UNITY_EDITOR
	void Update() {
		if (!Application.isPlaying) {
			Debug.LogWarning(string.Format("GameObject still alive, killing: " + gameObjectName));
			DestroyImmediate(gameObject);
		}
	}
	#endif
}
