using UnityEngine;
using System.Collections;

namespace SceneTransitions {

	public class BaseSceneController : MonoBehaviour {
		
		public BaseSceneModel model;
		
		[Header("Transition Stuff")]
		public TransitionInfo transitionInfo;
		
		protected virtual void Awake () {
			model = SceneManager.Instance.currentSceneDM;
		}
		
		protected virtual IEnumerator Start () {
			InitializationManager.Instance.Init();
			while(InitializationManager.Instance.currentState != InitializationManager.InitializationState.Done) {
				yield return null;
			}
			
			yield return StartCoroutine(SceneInitializationCoroutine());
			yield return StartCoroutine(BeginTransitionInCoroutine());
		}
		
		/// <summary>
		/// Does all heavy stuff before start scene in-transition
		/// </summary>
		/// <returns>The initialization coroutine.</returns>
		public virtual IEnumerator SceneInitializationCoroutine() {
			yield break;
		}
		
		protected IEnumerator BeginTransitionInCoroutine() {
			
			if (SceneManager.Instance.currentSceneDM == null) {
				yield break; // there is no transition to do
			}

			// cache last transition type
			TransitionFactory.TransitionType lastUsedTransitionType = TransitionFactory.Instance.lastUsedTransitionType;

			// get new transition type
			yield return StartCoroutine(TransitionFactory.Instance.TryLoadTransition(this.transitionInfo.type));
			BaseSceneTransition transitionBehaviour = TransitionFactory.Instance.GetTransitionOfGivenType(this.transitionInfo.type);

			if (lastUsedTransitionType != TransitionFactory.TransitionType.ScreenShotTransition && transitionInfo.type == TransitionFactory.TransitionType.ScreenShotTransition) {
				yield return StartCoroutine((transitionBehaviour as ScreenShotTransition).TakeSnapshot());
			}
			
			if (this.transitionInfo.type != TransitionFactory.TransitionType.None) {

				if (transitionBehaviour == null) {
					TransitionFactory.Instance.DisableAllTransitions(this.transitionInfo.type);
					yield break; // just exist
				}

				TransitionFactory.Instance.DisableAllTransitions(this.transitionInfo.type);				
				transitionBehaviour.PlayFadeIn( this.transitionInfo.mask, this.transitionInfo.durationSeconds );
				
				while (!transitionBehaviour.IsTransitionInComplete) {
					yield return null;
				}
			}
		}
		
		void Update () {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				OnBackButton();
			}
		}
		
		// by default, goes to prior scene
		protected virtual void OnBackButton () {
			if (!PopUpManager.Instance.isAnyPopUpActive) {
				SceneManager.Instance.PopToScene(this.transitionInfo);
			}
		}
	}
}