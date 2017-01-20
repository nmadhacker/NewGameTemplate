using UnityEngine;
using System.Collections;

namespace SceneTransitions {

	public class BasePopUpController : MonoBehaviour {
		
		protected BasePopUpModel model;
		
		protected virtual void Awake () {
			model = PopUpManager.Instance.currentModel;
			if (model != null) {
				PopUpManager.Instance.RegisterPopUp(model.id,this);
			}
		}
		
		protected virtual void OnClose() {
			PopUpManager.Instance.ClosePopUp(model.id);
		}

		void Update () {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				OnBackButton();
			}
		}

		// by default, closes current popup
		protected virtual void OnBackButton () {
			OnClose();
		}
	}
}