using UnityEngine;
using System.Collections;

namespace SceneTransitions {
	[System.Serializable]
	public class TransitionInfo {
		public TransitionFactory.TransitionType type;
		public Texture2D mask;
		[Range(1,10)]
		public float durationSeconds;
	}
}