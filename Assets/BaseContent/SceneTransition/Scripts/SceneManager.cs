using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SceneTransitions {

	public class SceneManager : Singleton<SceneManager>
    {
        public const SceneRegistry baseScene = SceneRegistry.None;
		public const SceneRegistry loadingSceneName = SceneRegistry.LoadingScene;
		public SceneRegistry sceneName { get { return currentSceneDM.sceneName; } }
		public BaseSceneModel currentSceneDM {get; set;}
		public SceneRegistry forcedNextScene;
		
		bool isLoadingScene;

		private void RemoveDuplicateScenes(SceneRegistry scene, BaseSceneModel sceneDM) {
			if(currentSceneDM == null)
				return;
			
			// remove duplicates in the stack...
			BaseSceneModel oldScene = null;
			BaseSceneModel olderScene = currentSceneDM;
			int sceneCounter = 0;
			while(olderScene != null) {
				// base comparison
				bool matching = olderScene.sceneName == scene;
				
				// If there was a scene match, and the new scene has a history ref, check that it matches olderScene history ref. Otherwise, it is not a match.
				if(matching && sceneDM.historyRef != null) {
					if(!sceneDM.historyRef.Equals(olderScene.historyRef))
						matching = false;
				}
				
				if(matching) {
					// Allow a single duplicate of each scene
					sceneCounter++;
					if( sceneCounter == 2 ) {
						oldScene.previous = olderScene.previous;
						break;
					}
				}			
				oldScene = olderScene;
				olderScene = olderScene.previous;
			}
		}

		public void RegisterSceneRoot(BaseSceneController sceneController) {
			this.currentSceneDM = sceneController.model;
		}

		public void SetForcedNextScene(SceneRegistry forcedScene) {
			this.forcedNextScene = forcedScene;
		}
		public void ClearForcedNextScene() {
			this.forcedNextScene = SceneRegistry.None;
		}
		
		public void PushToScene(SceneRegistry sceneName, TransitionInfo transitionInfo, bool keepInHistory = true, bool requiresLoading = false) {
			BaseSceneModel newModel = new BaseSceneModel();
			newModel.sceneName = sceneName;
			PushToScene(newModel, transitionInfo, keepInHistory, requiresLoading);
		}
		
		public void PushToScene ( BaseSceneModel sceneModel, TransitionInfo transitionInfo, bool keepInHistory, bool requiresLoading = false ) {
			if (isLoadingScene) {
				Debug.LogWarning ("Already loading scene");
				return; // don't load scene is we are already loading one ...
			}
			
			isLoadingScene = true;
			
			if (keepInHistory) {
				RemoveDuplicateScenes(sceneModel.sceneName,sceneModel);
			}
			
			if (currentSceneDM != null) {
				sceneModel.previous = keepInHistory ? currentSceneDM : currentSceneDM.previous;
			}	
			
			LoadingSceneController.LoadingSceneModel loadingSceneModel = null;
			
			if (requiresLoading) {
				loadingSceneModel = new LoadingSceneController.LoadingSceneModel ();
				loadingSceneModel.nextSceneModel = sceneModel;
				loadingSceneModel.sceneName = loadingSceneName;
			}
			
			SceneRegistry outScene = currentSceneDM != null ? currentSceneDM.sceneName : SceneRegistry.None;
			Debug.Log(string.Format("Push to scene from {0} to {1}",outScene,sceneModel.sceneName));
			
			currentSceneDM = requiresLoading ? loadingSceneModel : sceneModel;
			
			StartCoroutine(LoadSceneCoroutine(transitionInfo));		
		}
		
		IEnumerator LoadSceneCoroutine (TransitionInfo transitionInfo) {	
			
			switch(transitionInfo.type) {
			case TransitionFactory.TransitionType.ColorTransition:
				yield return StartCoroutine( TransitionFactory.Instance.TryLoadCurrentColorTransition() );
				break;
			case TransitionFactory.TransitionType.ScreenShotTransition:
				yield return StartCoroutine( TransitionFactory.Instance.TryLoadCurrentScreenShotTransition() );
				break;
			}

			ColorTransition transition = TransitionFactory.Instance.transition[transitionInfo.type] as ColorTransition;
			
			if (transitionInfo.type == TransitionFactory.TransitionType.ColorTransition) {
				transition.PlayFadeOut();
				while(!transition.IsTransitionOutComplete){
					yield return null;
				}
			}
			else if (transitionInfo.type == TransitionFactory.TransitionType.ScreenShotTransition) {
				yield return StartCoroutine((transition as ScreenShotTransition).TakeSnapshot());
				transition.InitTransitionIn();
				
				if (currentSceneDM.isGoingBack) {
					transition.InvertMask();
				}
			}

            

            AsyncOperation asyncOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(currentSceneDM.SceneName);
			asyncOp.allowSceneActivation = false;
			while (asyncOp.progress < 0.9f) {
				yield return null;
			}	
			
			asyncOp.allowSceneActivation = true;
			
			yield return asyncOp;
			Debug.Log (string.Format ("Scene loaded: {0}", currentSceneDM.sceneName));
			
			isLoadingScene = false;
		}
		
		public void PopToScene(TransitionInfo transitionInfo, bool requiresLoading = false) {
			if (isLoadingScene) {
				Debug.LogWarning ("Already loading scene");
				return; // don't load scene is we are already loading one ...
			}
			
			isLoadingScene = true;
			
			BaseSceneModel defaultModel = new BaseSceneModel{sceneName = SceneRegistry.None};
			
			BaseSceneModel previousModel = currentSceneDM != null ? currentSceneDM.previous : null;
			if (previousModel == null) {
				previousModel = defaultModel;
			}

			previousModel.isGoingBack = true;
			
			LoadingSceneController.LoadingSceneModel loadingSceneModel = null;
			
			if (requiresLoading) {
				loadingSceneModel = new LoadingSceneController.LoadingSceneModel ();
				loadingSceneModel.sceneName = loadingSceneName;
				loadingSceneModel.nextSceneModel = previousModel;
			}
			
			currentSceneDM = requiresLoading ? loadingSceneModel : previousModel;
			
			StartCoroutine(LoadSceneCoroutine(transitionInfo));
		}
	}
}