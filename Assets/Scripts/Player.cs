using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Rigidbody2D myRigibody;
	private Animator[] myAnimators;

	[SerializeField]
	private float movementSpeed;

	public bool facingRight;
	private bool attack;
	private bool slide;
	private bool throws;

	[SerializeField]
	private Transform[] groundPoints;
	[SerializeField]
	private float groundRadius;
	[SerializeField]
	private LayerMask whatIsGround;

	public bool isGrounded;
	private bool jump;
	private bool jumpAttack;
	[SerializeField]
	private float jumpForce;
	[SerializeField]
	private bool airControl;

	[SerializeField]
	//private GameObject knifePref;
	//public Transform ArrowL;
	//public Transform ArrowR;
	//public Transform ArrowSpawnL;
	//public Transform ArrowSpawnR;
	//public float arrowRepeat;

	private GameObject[] items = new GameObject[3];
	[SerializeField]
	private GameObject[] finishItems = new GameObject[3];
	private int item = 0;
	[SerializeField]
	private GameObject score;


	public float HP;
	public float invTimer;
	public Transform playerHP;

	// Use this for initialization
	void Start () {
		facingRight = true;
		myRigibody = GetComponent<Rigidbody2D>();
		myAnimators = GetComponentsInChildren<Animator>();
	}

	void Update(){
		HandleInput();

		invTimer -= Time.deltaTime;
		if (invTimer <= 0) 
		{
			invTimer = 0;
		}
		if(invTimer > 0)
		{
			
		}

        if (!isGrounded)
        {
            foreach(Animator a in myAnimators)
            {
                a.SetFloat("vSpeed", myRigibody.velocity.y);
            }
        }
	}

	void FixedUpdate () {
		float horizontal = Input.GetAxis("Horizontal");
		isGrounded = IsGrounded();
		HandleMovement(horizontal);
		Flip(horizontal);
		HandleAttacks();
		HandlerLayers ();
		ResetValues();
	}

	private void HandleMovement(float horizontal){
		//myRigibody.velocity = Vector2.left; // x - 1; y - 0;
		if(myRigibody.velocity.y < 0){



			foreach (Animator a in myAnimators)
            {
                a.SetBool("land", true);
                a.SetFloat("vSpeed", myRigibody.velocity.y);
            }
				
                
		}
		if(!this.myAnimators[0].GetBool("slide") && !this.myAnimators[0].GetCurrentAnimatorStateInfo(0).IsTag("Attack") && (isGrounded || airControl)){
			myRigibody.velocity = new Vector2(horizontal*movementSpeed,myRigibody.velocity.y);	
		}
		if (isGrounded && jump) {
			isGrounded = false;
			myRigibody.AddForce(new Vector2(0,jumpForce));
			foreach (Animator a in myAnimators)
				a.SetTrigger ("jump");
		}
		if (slide && !this.myAnimators[0].GetCurrentAnimatorStateInfo (0).IsName ("Slide")) {

			foreach (Animator a in myAnimators)
			a.SetBool ("slide", true);
		}else if(!this.myAnimators[0].GetCurrentAnimatorStateInfo (0).IsName ("Slide")){
			foreach (Animator a in myAnimators)
				a.SetBool ("slide", false);
		}
		foreach (Animator a in myAnimators)
			a.SetFloat("speed",Mathf.Abs(horizontal));
	}

	private void HandleAttacks(){
		if (attack && isGrounded && !this.myAnimators[0].GetCurrentAnimatorStateInfo(0).IsTag("Attack")) {
			foreach (Animator a in myAnimators)
				a.SetTrigger("attack");
			myRigibody.velocity = Vector2.zero;
		}
		if (jumpAttack && !isGrounded && !this.myAnimators[0].GetCurrentAnimatorStateInfo (1).IsName ("JumpAttack")) {
			foreach (Animator a in myAnimators)
				a.SetBool ("jumpAttack", true);
		}
		if (!jumpAttack && !this.myAnimators[0].GetCurrentAnimatorStateInfo (1).IsName ("JumpAttack")) {
			foreach (Animator a in myAnimators)
				a.SetBool ("jumpAttack",false);
		}
		if (throws && isGrounded && !this.myAnimators[0].GetCurrentAnimatorStateInfo (0).IsName ("Throw")) {
			foreach (Animator a in myAnimators)
				a.SetTrigger ("throwTrigger");
		}
	}

	private void HandleInput(){
		if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0)) {
			jump = true;
		}
		if(Input.GetKeyDown(KeyCode.LeftShift)){
			attack = true;
			jumpAttack = true;
		}
		if (Input.GetKeyDown (KeyCode.LeftControl)) {
			slide = true;
		}
		if (Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button2)) {
			throws = true;
		}
	}

	private void Flip(float horizontal){
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
			facingRight = !facingRight;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
	}

	private void ResetValues(){
		attack = false;
		slide = false;
		jump = false;
		jumpAttack = false;
		throws = false;
	}

	private bool IsGrounded(){
		if (myRigibody.velocity.y <= 0) {
			foreach (Transform point in groundPoints) {
				Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
				foreach(Collider2D collider in colliders){
					if (collider.gameObject != gameObject) {

						foreach (Animator a in myAnimators) {
							//a.ResetTrigger ("jump");
							a.SetBool ("land",false);
                            a.SetFloat("vSpeed", myRigibody.velocity.y);
						}

						return true;
					}
				}
			}
		}
		return false;
	}

	private void HandlerLayers(){
		if (!isGrounded) {
			foreach (Animator a in myAnimators)
            {
                a.SetLayerWeight(1, 1);
                a.SetFloat("vSpeed", myRigibody.velocity.y);
            }
				
		} else {
			foreach (Animator a in myAnimators)
				a.SetLayerWeight (1,0);
		}
	}

	void OnCollisionStay2D(Collision2D other){
		if (other.gameObject.tag == "movePlatform") {
			transform.parent = other.transform;
		}
	}

	void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.tag == "movePlatform") {
			transform.parent = null;
		}
	}

	void OnCollisionEnter2D(Collision2D other){
	}

	private void verifyItems(){
		for (int i = 0; i < 3; i++) {
			if (!items [i].Equals (finishItems [i])) {
				for (int j = 0; j < 3; j++) {
					item = 0;
					//Physics2D.IgnoreCollision (items [j].GetComponent<BoxCollider2D>(),gameObject.GetComponent<BoxCollider2D>(),false);
					items [j].GetComponent<Item> ().ResetPosition ();
				}
				break;
			}
		}
	}

	public void doDamage(float d)
	{
		if (invTimer <= 0) {
			HP-=d;

			invTimer = 1f;
            StartCoroutine(flicker());

            Destroy (playerHP.GetChild (0).gameObject);
		}

		if (HP <= 0) 
		{
			Application.LoadLevel(Application.loadedLevel);
			Destroy (gameObject);
		}

	}

	IEnumerator flicker()
	{
		SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer> (); 

        for(int i = 0; i < 5; i++)
        {
            foreach (SpriteRenderer s in srs)
            {
                s.enabled = false;
            }
            yield return new WaitForSeconds(.1f);
            foreach (SpriteRenderer s in srs)
            {
                s.enabled = true;
            }
            yield return new WaitForSeconds(.1f);
        }





	}
}
