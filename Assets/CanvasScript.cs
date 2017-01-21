using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour {

	public int hp = 20;
	public int we = 10;
	private Text hpText;
	private Text weText;
	[SerializeField] private Slider hpSlider;
	[SerializeField] private Slider weSlider;

	void Awake () {
		hpText = gameObject.GetComponent<Text> ();
		weText = gameObject.GetComponent<Text> ();
		hpSlider.value = 10;
		weSlider.value = 100;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.B)) {
			hpSlider.value -= 1;
			Debug.Log ("Recibe daño");
		}

		if (Input.GetKey (KeyCode.N)) {
			we--;
			weSlider.value -= 25;
			Debug.Log ("Lanza una onda");
		}
	}
}