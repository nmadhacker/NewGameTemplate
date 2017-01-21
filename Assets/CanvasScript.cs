using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour {

	public int hp = 20;
	public int we = 10;
	Text hpBar;
	Text weBar;

	void Awake () {
		hpBar = gameObject.GetComponent<Text> ();
		weBar = gameObject.GetComponent<Text> ();
	}

	// Use this for initialization
	void Start () {
		hpBar.text = "";
		weBar.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i <= hp; i++) {
			hpBar.text += "|";
		}
		for (int j = 0; j <= we; j++) {
			weBar.text += "|";
		}

		if (Input.GetKey (KeyCode.B)) {
			hp--;
			Debug.Log ("Recibe daño");
		}

		if (Input.GetKey (KeyCode.N)) {
			we--;
			Debug.Log ("Lanza una onda");
		}
	}
}