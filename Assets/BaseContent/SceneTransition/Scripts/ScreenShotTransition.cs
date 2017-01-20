using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace SceneTransitions {

	public class ScreenShotTransition : ColorTransition
	{
		public enum ScreenShotTechnique {
			ReadPixels,
			RenderToTexture
		}
		
		public Texture2D snapshot;
		public RenderTexture renderTexture;
		public ScreenShotTechnique screenShotTechnique;
		
		private float currentRatio;
		
		public IEnumerator TakeScreenShotUsingReadPixels() {
			DateTime startTime = DateTime.Now;
			
			int width = Screen.width;
			int height = Screen.height;
			
			//Create a texture to pass to encoding
			Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24,false);
			texture.filterMode = FilterMode.Point;
			
			yield return new WaitForEndOfFrame(); //Wait for graphics to render
			Debug.Log(string.Format("Wait to next frame -> ElapsedTime: {0}",(DateTime.Now - startTime).TotalMilliseconds.ToString()));
			
			//Put buffer into texture
			texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
			Debug.Log(string.Format("ReadPixels -> ElapsedTime: {0}",(DateTime.Now - startTime).TotalMilliseconds.ToString()));
			
			//Split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
			texture.Apply();
			Debug.Log(string.Format("texture apply -> ElapsedTime: {0}",(DateTime.Now - startTime).TotalMilliseconds.ToString()));
			
			snapshot = texture;
			
			this.gameObject.GetComponent<Renderer>().material.SetTexture("_ImgTex",snapshot);
			
			DateTime endTime = DateTime.Now;
			Debug.Log(string.Format("ScreenShot Taken. Took: " + (endTime-startTime).TotalMilliseconds.ToString()));
		}
		
		public IEnumerator TakeScreenShotUsingRenderToTexture () {
			
			DateTime startTime = DateTime.Now;
			
			int width = Screen.width;
			int height = Screen.height;
			
			// Initialize and render
			if (this.renderTexture == null || !this.renderTexture.IsCreated()){
				this.renderTexture = new RenderTexture(width,height,24);
			}
			
			List<Camera> cameras = new List<Camera>(Camera.allCameras);
			cameras.Sort((a, b) => {
				return (int)(b.depth - a.depth);
			});
			// process each camera
			for(int count = 0, max = cameras.Count; count < max; ++count) {
				if (this.uiCamera == cameras[count] || !cameras[count].gameObject.activeInHierarchy) {
					continue;
				}
				// set render texture
				if (cameras[count].targetTexture != null) {
					continue; // used by someone else, skipping
				}
				
				cameras[count].targetTexture = this.renderTexture;
				cameras[count].Render();
				cameras[count].targetTexture = null; // remove reference
			}
			
			RenderTexture.active = this.renderTexture;
			
			this.gameObject.GetComponent<Renderer>().material.SetTexture("_ImgTex",this.renderTexture);
			
			DateTime endTime = DateTime.Now;
			Debug.Log(string.Format("ScreenShot Taken. Took: " + (endTime-startTime).TotalMilliseconds.ToString()));
			
			yield return 0; //new WaitForEndOfFrame();
		}
		
		public IEnumerator TakeSnapshot(){
			
			if (!this.gameObject.GetComponent<Renderer>().material.shader.isSupported){
				Debug.LogError("Unable to take screenshot because current shader is not supported on this device");
				yield break;
			}						
			
			if (this.screenShotTechnique == ScreenShotTechnique.ReadPixels) {
				yield return StartCoroutine(TakeScreenShotUsingReadPixels());
			}
			else {
				yield return StartCoroutine(TakeScreenShotUsingRenderToTexture());
			}
		}
		
		protected override IEnumerator StartTweeningFade (bool fadeOut)
		{
			yield return StartCoroutine(base.StartTweeningFade (fadeOut));
			if (this.renderTexture != null) {
				this.renderTexture.DiscardContents(); // release memory
				this.snapshot = null; // release memory
			}
		}
	}
}