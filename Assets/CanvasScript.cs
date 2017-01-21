using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour {

	[SerializeField] private Slider hpSlider;
	[SerializeField] private Slider weSlider;

	void Awake () {
		hpSlider.value = 100;
		weSlider.value = 100;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		hpSlider.value = CharacterStats.Instance.Health;
		weSlider.value = CharacterStats.Instance.Energy;
	}
}