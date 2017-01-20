using UnityEngine;
using System.Collections;
using SceneTransitions;

public class LoadingSceneController : BaseSceneController {

	public class LoadingSceneModel : BaseSceneModel {
		public BaseSceneModel nextSceneModel;
	}

	public SceneRegistry sceneToLoad;
	public UnityEngine.UI.Text percentText;
	public UnityEngine.UI.Slider percentSlider;

	LoadingSceneModel sceneModel;
	BaseSceneTransition transition;

	protected override void Awake ()
	{
		base.Awake ();
		this.percentText.text = string.Format ("{0}/{1}", 0, 100);
		this.percentSlider.minValue = 0;
		this.percentSlider.maxValue = 100;
		this.percentSlider.value = 0;
	}

	protected override IEnumerator Start ()
	{
		// starts loading new scene
		sceneModel = this.model as LoadingSceneModel;

		if (sceneModel == null) {
			sceneModel = new LoadingSceneModel();
			sceneModel.sceneName = SceneRegistry.LoadingScene;
			sceneModel.nextSceneModel = new BaseSceneModel();
			sceneModel.nextSceneModel.sceneName = sceneToLoad;
		}

		yield return StartCoroutine (base.Start ());
		StartCoroutine (StartLoadingNewScene ());
	}

	IEnumerator StartLoadingNewScene () {
        
        AsyncOperation asyncOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneModel.nextSceneModel.SceneName);
		asyncOp.allowSceneActivation = false;	

		Debug.Log("Waiting for level to complete ...");
		while (asyncOp.progress < 0.9f) {
			int currentPercent = Mathf.RoundToInt(asyncOp.progress * 100);
			percentText.text = string.Format ("{0}/{1}", currentPercent, 100);
			percentSlider.value = currentPercent;
			yield return null;
		}

		SceneManager.Instance.currentSceneDM = this.sceneModel.nextSceneModel;

		percentText.text = string.Format ("{0}/{1}", 100, 100);
		percentSlider.value = 100;

		Debug.Log("Preparing FadeOut/FadeIn");
		yield return StartCoroutine(PrepareFadeInOut(asyncOp));
	}

	IEnumerator PrepareFadeInOut(AsyncOperation loadLevel){

		yield return StartCoroutine(TransitionFactory.Instance.TryLoadTransition(transitionInfo.type));
		var transitionBehaviour = TransitionFactory.Instance.transition[transitionInfo.type];
		if (transitionInfo.type == TransitionFactory.TransitionType.ColorTransition) {
			transitionBehaviour.PlayFadeOut(transitionInfo.mask, transitionInfo.durationSeconds);
			while (!transitionBehaviour.IsTransitionOutComplete) {
				yield return null;
			}
		}
		else if (transitionInfo.type == TransitionFactory.TransitionType.ScreenShotTransition){
			yield return StartCoroutine( (transitionBehaviour as ScreenShotTransition).TakeSnapshot() );
			(transitionBehaviour as ScreenShotTransition).InitTransitionIn();
		}
		
		yield return new WaitForSeconds(0.5f);
		
		loadLevel.allowSceneActivation = true;
		yield return 0; // wait one more frame
		yield return loadLevel;
		Debug.Log("New Level loaded successfull");	
	}
}
