using UnityEngine;
using System.Collections;

/// <summary>
/// Player movement that works without
/// </summary>
using UnityStandardAssets.CrossPlatformInput;


public class PlayerMovement : MonoBehaviour {
	public float speed;
	public float jumpStrength;
	public float gravityValue;

	[Tooltip("The transform to which the player sprite belongs. Used to flip stuff around a little when gravity is reversed.")]
	public Transform playerSprite;

	public Transform groundCollider;
	public Transform headCollider;
	public AudioClip jumpSound;

	public float collisionCheckRange = 0.5f;

	public LayerMask collisionLayer;

	public SwordBehaviour sword;
	public ShootingDevice shootingDevice;

	public Animator animator;

	private Rigidbody2D localRb;
//	private Vector2 moveVector; // vector for gravity and movement left-right
	[SerializeField] private bool gravityIsFlipped;

	private Collider2D[] collisionCheckBuffer = new Collider2D[3];

	public bool HasGravityFlipped {
		get {
			return this.gravityIsFlipped;
		}
	}

	public float VerticalVelocity {
		get {
			return this.localRb.velocity.y;
		}
	}

	public bool InAir {
		get {
			return animator.GetBool("InAir");
		}
	}

	// Use this for initialization
	void Start () {
		this.localRb = this.GetComponent<Rigidbody2D>();
	}

//	void OnDrawGizmos() {
//		Gizmos.color = Color.cyan;
//		Gizmos.DrawSphere(groundCollider.transform.position, collisionCheckRange);
//		Gizmos.DrawSphere(headCollider.transform.position, collisionCheckRange);
//	}
	void FixedUpdate() {

		// If gravity is flipped, the head collider determines if we are grounded.
		if (HasGravityFlipped) {
			int groundHits = Physics2D.OverlapCircleNonAlloc(headCollider.transform.position, collisionCheckRange, collisionCheckBuffer, collisionLayer);
			bool inair = true;
			for (int i = 0; i < groundHits; ++i) {
				if (collisionCheckBuffer[i].gameObject != groundCollider.gameObject) {
					inair = false;
					break;
				}
			}
			animator.SetBool("InAir", inair);
		}
		else {
			int groundHits = Physics2D.OverlapCircleNonAlloc(groundCollider.transform.position, collisionCheckRange, collisionCheckBuffer, collisionLayer);
			bool inair = true;
			for (int i = 0; i < groundHits; ++i) {
				if (collisionCheckBuffer[i].gameObject != groundCollider.gameObject) {
					gravityIsFlipped = false;
					inair = false;
					break;
				}
			}
			animator.SetBool("InAir", inair);
		}

	}

	// Update is called once per frame
	void Update () {
		var vel = localRb.velocity;
		vel.x = CrossPlatformInputManager.GetAxis("Horizontal") * (this.speed);
		// This actually does the movement
		localRb.velocity = vel;
		animator.SetFloat("Horizontal", vel.x);
		animator.SetFloat ("Vertical", this.VerticalVelocity);
		if (vel.x != 0f) {
			animator.SetBool("Look Left", vel.x < 0);
			animator.SetBool("Look Right", vel.x > 0);
		}

		// Apply JumpForce
		if (CrossPlatformInputManager.GetButtonDown("Jump")) {
			DoJump ();
		}

		if (CrossPlatformInputManager.GetButtonDown("Fire1")) {
			DoAttack ();
		}
		Vector3 relativeInput = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"), 0f).normalized;
		sword.MoveSpriteTo(relativeInput);
	}

	private void DoJump() {
		if (!InAir) {
			PlaySound();
			animator.SetTrigger("Jump");
			float jump = 0f;
			if (this.gravityIsFlipped) {
				jump = -jumpStrength;
				this.RevertGravity(); // Reset on Jumping
			}
			else {
				jump = jumpStrength;
			}
            localRb.AddForce(new Vector2(0f, jump));
		}
	}

	private void PlaySound() {
		var sound = new GameObject();
		sound.transform.position = transform.position;
		var source = sound.AddComponent<AudioSource>();
		source.clip = jumpSound;
		source.Play();
		source.volume = 0.12f;
		Destroy (sound, source.clip.length);
	}

	void DoAttack () {
		animator.SetTrigger("Attack");
		// TODO: If flipped, do ranged weapon instead.
		// maybe use the shooting device for this
		if (!this.HasGravityFlipped) {
			sword.SwingSword();
		}
		else {
			//animator.SetBool("Look Left", vel.x < 0);
			//-animator.SetBool("Look Right", vel.x > 0);
			if (animator.GetBool("Look Left")) {
				shootingDevice.Shoot((this.transform.rotation * Vector2.left).normalized);
			}
			else {
				shootingDevice.Shoot((this.transform.rotation * Vector2.right).normalized);
			}
		}
	}

	public void FlipGravity () {
		this.gravityIsFlipped = true;
		var scale = playerSprite.localScale;
		scale.y = -scale.y;
		playerSprite.localScale = scale;
		this.localRb.gravityScale *= -1;

	}

	public void RevertGravity () {
		this.gravityIsFlipped = false;
		var scale = playerSprite.localScale;
		scale.y = Mathf.Abs (scale.y);
		playerSprite.localScale = scale;
		this.localRb.gravityScale = Mathf.Abs (this.localRb.gravityScale);
	}
}
