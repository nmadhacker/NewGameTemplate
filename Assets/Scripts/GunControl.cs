using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
	public BaseWeapon currentWeapon;
	[SerializeField] Animator m_Anim;
	[SerializeField] float fireStartDelay;

	public bool IsFiring { get; private set;}

	public int waveType;

	public void Update()
	{
		if (!IsFiring && Input.GetButtonDown ("Fire1") && currentWeapon.Fire()) 
		{
			IsFiring = true;
			m_Anim.SetBool ("Fire", true);
			StartCoroutine (TriggerFireAnimation ());
		}
		if (IsFiring && Input.GetButtonUp ("Fire1")) 
		{
			m_Anim.SetBool ("Fire", false);
			IsFiring = false;
			currentWeapon.Stop ();
		}
        if(IsFiring && !GetComponent<Player>().isGrounded)
        {
            m_Anim.SetBool("Fire", false);
            IsFiring = false;
            currentWeapon.Stop();
        }
		waveTypeSelector ();
	}

	IEnumerator TriggerFireAnimation()
	{
		yield return new WaitForSeconds (fireStartDelay);
		m_Anim.SetBool ("Fire",true);
		AkSoundEngine.PostEvent ("PlayHeroAttack",this.gameObject);
		yield break;
	}

	private void waveTypeSelector() {
		if (Input.GetKeyDown (KeyCode.LeftAlt)) {
			currentWeapon.nextWave ();
		}
	}
}
