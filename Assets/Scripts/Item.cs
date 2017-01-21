using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	[SerializeField]
	private string itemName;
	private float initialX;
	private float initialY; 

	// Use this for initialization
	void Start () {
		initialX = transform.position.x;
		initialY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionStay2D(Collision2D other){
		if (other.gameObject.tag != "Player") {
			transform.parent = other.transform;
		}
	}

	void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.tag != "Player") {
			transform.parent = null;
		}
	}

	public void ResetPosition(){
		Vector2 vector = new Vector2 (initialX, initialY);
		transform.position = vector;
		gameObject.SetActive(true);
	}
}
