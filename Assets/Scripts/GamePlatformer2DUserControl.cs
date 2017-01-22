using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlatformer2DUserControl : UnityStandardAssets._2D.Platformer2DUserControl {

	[SerializeField] private GunControl gunControl;

	protected override void Awake ()
	{
		base.Awake ();
		gunControl = GetComponent<GunControl> ();
	}

//	protected override void Update () 
//	{
//		base.Update ();
//	}

	protected override void FixedUpdate ()
	{
		float h = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxis("Horizontal");
		//Debug.Log (gunControl.IsFiring);
		if (!gunControl.IsFiring) {
			m_Character.Move (h, m_Jump);
			m_Jump = false;
		}
	}
}
