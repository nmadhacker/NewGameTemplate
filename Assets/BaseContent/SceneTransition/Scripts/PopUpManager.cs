using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SceneTransitions;

public class PopUpManager : Singleton<PopUpManager> {

	public int currentPopUpIndex { get; private set; }
	public BasePopUpModel currentModel;
	public Dictionary<int, BasePopUpModel> models = new Dictionary<int, BasePopUpModel>();
	bool isBusy;

	public bool isAnyPopUpActive { get{ return models.Count > 0 ; } }

	public int ShowPopUp (BasePopUpModel model) {

		if (isBusy) {
			return -1; // do nothing ...
		}

		currentPopUpIndex++;
		model.id = currentPopUpIndex;
		models [currentPopUpIndex] = model;
		currentModel = model;
		isBusy = true;

		StartCoroutine(ShowPopUpCoroutine(model));
		return currentPopUpIndex;
	}

	public void RegisterPopUp (int popUpId, BasePopUpController root) {
		models [popUpId].rootObject = root;
	}

	public void ClosePopUp(int popUpIndex) {
		var popUpModel = models.TryGetValue<int,BasePopUpModel> (popUpIndex);
		if (popUpModel != null && popUpModel.rootObject != null) {
			models.Remove(popUpIndex);
			Object.Destroy (popUpModel.rootObject.gameObject);
		}
	}

	IEnumerator ShowPopUpCoroutine (BasePopUpModel model) {
        var op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(model.sceneName.ToString(), UnityEngine.SceneManagement.LoadSceneMode.Additive);
		while (!op.isDone) {
			yield return null;
		}

		isBusy = false;
		yield break;
	}
}
