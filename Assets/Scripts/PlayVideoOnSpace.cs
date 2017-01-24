using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVideoOnSpace : MonoBehaviour {
	void Start()
	{
		UnityEngine.UI.RawImage r = GetComponent<UnityEngine.UI.RawImage>();
		MovieTexture movie = (MovieTexture)r.mainTexture;

		if (movie.isPlaying) {
			movie.Pause();
		}
		else {
			movie.Play();
		}
	}
}