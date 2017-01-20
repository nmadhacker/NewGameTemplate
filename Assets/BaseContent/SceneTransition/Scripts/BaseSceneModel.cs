using UnityEngine;
using System.Collections;

namespace SceneTransitions {

	public class BaseSceneModel {
		public SceneRegistry sceneName;
        /// <summary>
        /// Returns valid scene name. If scene is not set, returns first scene
        /// </summary>
        public string SceneName
        {
            get
            {
                return sceneName != SceneRegistry.None ? sceneName.ToString() : UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).name;
            }
        }
		public BaseSceneModel previous;
		public bool isGoingBack;
		
		// a scene specific value that the transition manager uses to de-dup the scene stack.
		// Two models with the same _scene will normally match. If the values of historyRef 
		// don't match (with .Equals()) for those two matching _scenes then they are considered separate.
		public object	historyRef;
	}
}
