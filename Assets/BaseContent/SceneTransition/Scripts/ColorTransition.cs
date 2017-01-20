using UnityEngine;
using System.Collections;

namespace SceneTransitions {
		
	public class ColorTransition : BaseSceneTransition {
		
		public float duration = 1f;
		public AnimationCurve curve;
		public bool playOnAwake = false;
		public Camera uiCamera;
		
		private Texture defaultGradient;
		private float defaultDuration;
        private Renderer mRenderer;

        void Awake () {
            // save default duration and mask            
            mRenderer = this.GetComponent<Renderer>();
			defaultGradient = mRenderer.material.GetTexture("_MainTex");
			defaultDuration = duration;
		}
		
		void Start() {
			Resize();
			if(playOnAwake) PlayFadeIn();
		}
		
		public override void SetMaterial(Material mat){            
            mRenderer.material = mat;
		}
		
		public void InitTransitionIn(){
			mRenderer.enabled = true;
			mRenderer.material.SetFloat("_Cutoff", 0);
		}
		
		public void InitTransitionOut(){
			mRenderer.material.SetFloat("_Cutoff", 1);
			mRenderer.enabled = false;
		}
		
		public void InvertMask(){
			mRenderer.material.SetTextureOffset("_MainTex",new Vector2(1,0));
			mRenderer.material.SetTextureScale("_MainTex",new Vector2(-1,1));
		}
		
		public void RestoreMask(){
			mRenderer.material.SetTextureOffset("_MainTex",new Vector2(0,0));
			mRenderer.material.SetTextureScale("_MainTex",new Vector2(1,1));
		}
		
		public void SetTransitionParams (Texture2D mask, float duration) {
			if (mask != null) { // replace default texture with new one
				mRenderer.sharedMaterial.SetTexture("_MainTex",mask);
			}
			else {
				mRenderer.sharedMaterial.SetTexture("_MainTex",defaultGradient);
			}
			
			if (duration > 0) {
				this.duration = duration;
			}
			else {
				this.duration = defaultDuration;
			}
		}
		
		public override void PlayFadeIn( Texture2D transitionMask = null, float duration = -1 ) {	
			if (!mRenderer.material.shader.isSupported) {
				Debug.LogError("Transition shader is not supported on this device!");
				return;
			}
			
			SetTransitionParams(transitionMask,duration);
			
			Resize();
			base.PlayFadeIn();
			mRenderer.enabled = true;
			mRenderer.material.SetFloat("_Cutoff", 0);
			StartCoroutine(StartTweeningFade(false));
		}
		
		public override void PlayFadeOut( Texture2D transitionMask = null, float duration = -1 ){
			
			SetTransitionParams(transitionMask,duration);
			
			Resize();
			base.PlayFadeOut();
			mRenderer.enabled = true;
			mRenderer.material.SetFloat("_Cutoff", 1);
			StartCoroutine(StartTweeningFade(true));
		}
		
		protected virtual IEnumerator StartTweeningFade(bool fadeOut) 
		{
			IsBusy = true;
			
			float startTime = Time.realtimeSinceStartup;
			float elapsedTime = 0;
			while(elapsedTime < duration) {
				elapsedTime = Time.realtimeSinceStartup - startTime; // updates elapsed time
				yield return 0;
				float val = (!fadeOut) ? curve.Evaluate(elapsedTime / duration) : 1 - curve.Evaluate(elapsedTime / duration);
				mRenderer.material.SetFloat("_Cutoff",val);
			}
			if (!fadeOut){
				NotifyFadeInComplete();
				mRenderer.enabled = false;
			}
			else{
				NotifyFadeOutComplete();
			}
			RestoreMask();
			this.IsBusy = false;
		}
		
		public override void Resize () {
			
			float screenWidth = uiCamera.pixelWidth, screenHeight = uiCamera.pixelHeight;
			float xRatio = Mathf.Max(screenWidth / screenHeight, 1);
			float yRatio = Mathf.Max(screenHeight / screenWidth, 1);
			transform.localScale = new Vector3(xRatio, yRatio, 1);
			Debug.Log("CurrentScale: x[" + transform.localScale.x + "], y[" + transform.localScale.y + "], z[" + transform.localScale.z + "]");
		}
	}
}