using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SceneTransitions {

	public class TransitionFactory : Singleton<TransitionFactory>
	{
		public enum TransitionType {
			None,
			ColorTransition,
			ScreenShotTransition,
			BlurTransition
		}
		
		public Dictionary<TransitionType,BaseSceneTransition> transition {get; private set;}
		
		public IEnumerator TryLoadCurrentColorTransition(){
			
			if (transition == null) {
				transition = new Dictionary<TransitionType, BaseSceneTransition>();
			}
			
			if (transition.ContainsKey(TransitionType.ColorTransition)){ 
				yield break; // color transition already cached
			}
			
			// Create new ColorTransition
			var op = Resources.LoadAsync<GameObject>("TransitionPrefabs/TransitionColor");
			while (!op.isDone) {
				yield return null;
			}
			
			GameObject go = (GameObject)GameObject.Instantiate(op.asset);
			transition[TransitionType.ColorTransition] = go.GetComponent<ColorTransition>();
			go.transform.SetParent(this.gameObject.transform, false);
			DontDestroyOnLoad(go);
			
			yield break;
		}
		
		public IEnumerator TryLoadCurrentScreenShotTransition(){
			
			if (transition == null) {
				transition = new Dictionary<TransitionType, BaseSceneTransition>();
			}
			
			if (transition.ContainsKey(TransitionType.ScreenShotTransition)) {
				yield break; // transition already cached
			}
			
			/*var op = Resources.LoadAsync<GameObject>("TransitionPrefabs/ImageTransition");
			while (!op.isDone) {
				yield return null;
			}*/

			GameObject prefab = Resources.Load<GameObject>("TransitionPrefabs/ImageTransition");
			
			GameObject go = (GameObject)GameObject.Instantiate(prefab/*op.asset*/);
			transition[TransitionType.ScreenShotTransition] = go.GetComponent<ScreenShotTransition>();
			go.transform.SetParent(this.gameObject.transform,false);
			DontDestroyOnLoad(go);
			
			yield break;
		}
		
		public IEnumerator TryLoadTransition(TransitionType transitionType) {
			switch (transitionType) {
			case TransitionType.ColorTransition:
				yield return StartCoroutine(TryLoadCurrentColorTransition());
				yield break;
			case TransitionType.ScreenShotTransition:
				yield return StartCoroutine(TryLoadCurrentScreenShotTransition());
				break;
			default:
				yield break;
			}
		}
		
		public void DisableAllTransitions(TransitionType exceptThisOne = TransitionType.None){
			
			if (transition == null || transition.Count == 0) {
				return; // there is nothing to disable
			}
			
			foreach(var transitionObject in transition) {
				if (!transitionObject.Key.Equals(exceptThisOne)) {
					((transitionObject.Value) as ColorTransition).InitTransitionOut();
					Debug.Log(string.Format("Disabled transition type: {0}",transitionObject.Key));
				}
			}
		}

		public BaseSceneTransition GetTransitionOfGivenType(TransitionType transitionType) {
			BaseSceneTransition toReturn;
			if (!transition.TryGetValue(transitionType, out toReturn)){			
				lastUsedTransitionType = transitionType;
			}
			return toReturn;
		}

		public TransitionType lastUsedTransitionType {get; private set;}
		public BaseSceneTransition lastUsedTransition {
			get 
			{ 
				BaseSceneTransition toReturn;
				transition.TryGetValue(lastUsedTransitionType, out toReturn); 
				return toReturn;
			} 
		}
	}
}