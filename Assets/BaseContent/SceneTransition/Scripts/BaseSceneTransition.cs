using UnityEngine;

namespace SceneTransitions {

	public class BaseSceneTransition : MonoBehaviour
	{
		private bool transitionInComplete, transitionOutComplete; 
		
		public bool IsBusy{get;set;}
		
		public string CurrentTransitionRunning {
			get{
				if (!this.IsBusy){
					return string.Empty;
				}
				if (!this.IsTransitionInComplete){
					return "TransitionIn";
				}
				else if (!this.IsTransitionOutComplete){
					return "TransitionOut";
				}
				return "Unknown ...";
			}
		}
		
		public bool IsTransitionInComplete {get{return this.transitionInComplete;}}
		public bool IsTransitionOutComplete {get{return this.transitionOutComplete;}}
		
		public virtual void PlayFadeIn( Texture2D transitionMask = null, float duration = -1 ){
			transitionInComplete = false;
			Debug.Log("Playing transition fade in");
		}
		
		public virtual void PlayFadeOut(Texture2D transitionMask = null, float duration = -1){
			transitionOutComplete = false;
			Debug.Log("Playing transition fade out");
		}
		
		public virtual void Resize(){}
		public virtual void SetMaterial(Material mat){}
		
		protected void NotifyFadeInComplete(){
			Debug.Log("TransitionInCompleted!");
			transitionInComplete = true;
		}
		protected void NotifyFadeOutComplete(){
			Debug.Log("TransitionOutCompleted!");
			transitionOutComplete = true;
		}
	}
}