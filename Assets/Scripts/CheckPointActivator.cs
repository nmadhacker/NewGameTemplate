using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointActivator : ExecuteOnContact {
	[SerializeField] string name;

	void MarkCheckPoint(){
		GameManager.Instance.SetCheckPoint (this.gameObject.transform.position);
		AkSoundEngine.SetState ("CheckPoints", name);
	}
}
