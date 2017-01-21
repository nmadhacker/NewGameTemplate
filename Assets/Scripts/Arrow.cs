using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public Transform sprite;
	public Vector2 fDir;
	public float fPower;
	//Vector3 oP;

	Rigidbody2D rb;

	float disappearTimer = .1f;
	//bool disappear = false;

	public bool isOnFire = false;

	public void Initialize (Vector2 v)
	{

		rb = GetComponent<Rigidbody2D> ();
		rb.AddForce (fDir.normalized * fPower);

	}

	void OnCollisionEnter2D(Collision2D c)
	{

        Joint2D joint;

        print("arrow hit");

		rb = GetComponent<Rigidbody2D> ();
        joint = GetComponent<FixedJoint2D>();
        //rb.isKinematic = true;

        //rb.bodyType = RigidbodyType2D.Kinematic;

        //rb.velocity = Vector2.zero;
        //rb.freezeRotation = true;


        Destroy(joint);
        Destroy(rb);

		Collider2D[] cols = transform.parent.GetComponentsInChildren<Collider2D> ();

		foreach (Collider2D col in cols) {

			col.enabled = false;
		}

		transform.parent.parent = c.transform;



		StartCoroutine (destroyArrow());
	}

	public void setOnFire()
	{
		SpriteRenderer[] sprites = transform.parent.GetComponentsInChildren<SpriteRenderer> ();

		foreach (SpriteRenderer s in sprites) {
		
			s.color = Color.red;
		}

		isOnFire = true;
	}

	IEnumerator destroyArrow()
	{
		yield return new WaitForSeconds (disappearTimer);
		Destroy (transform.parent.gameObject);
	}
}
