using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootingMob : BaseEntity {

	[SerializeField] private string enemyTag;
	[SerializeField] private TriggerSensor sensor;
	[SerializeField] private ShootingDevice weapon;
	[SerializeField] private ParticleSystem deathAnimation;
	[SerializeField] private ParticleSystem damageEffect;
	[SerializeField] private AudioClip damageSound;
	[SerializeField] private AudioClip deathSound;
	

	[SerializeField] private SwordBehaviour bowBehaviour;
	public SwordBehaviour BowBehaviour {
		get {
			return bowBehaviour;
		}
	}

	[SerializeField] private Rigidbody2D rb;
	public Rigidbody2D Rigidbody {
		get {
			return rb;
		}
	}

	[SerializeField] private List<BaseEntity> entities;
	public List<BaseEntity> Entities {
		get {
			return entities;
		}
	}

	[SerializeField] private Animator animator;
	public Animator Animator {
		get {
			return animator;
		}
	}

	private PickupDropper dropper;

	// Use this for initialization
	void Start () {
		entities = new List<BaseEntity>();
		sensor.SetCollisionEnterCallback(TriggerEnter2D);
		sensor.SetCollisionExitCallback(TriggerExit2D);
		MobCounter.Instance.PlusOneToAlive();
		animator = GetComponent<Animator>();
		this.dropper = GetComponent<PickupDropper>();
	}

	public override void RemoveHealth(float value) {
		base.RemoveHealth(value);
		damageEffect.Play();
		PlaySound(false);

	}
	
	// Update is called once per frame
	void Update () {
		// Trace line of sight to enemy entities
		foreach (var entity in entities) {
			// This z changing makes the line caster ignore the own collider at all as it requires min depth of 0
			var pos = this.transform.position;
			pos.z = -1;
			this.transform.position = pos;
			var hit = Physics2D.Linecast(this.transform.position, entity.Position, LayerMask.GetMask("World", "Player"), 0);
			pos.z = 0;
			this.transform.position = pos;
			Debug.DrawLine(this.transform.position, hit.point);
			if (hit.distance > 0.01f) {
				if (hit.collider.transform.root.gameObject.GetComponentInChildren<BaseEntity>() == entity) {
					// target entity is visible.
					var direction = (Vector2)entity.Position - hit.point;
					weapon.Shoot(direction);
					if (bowBehaviour != null) {
						bowBehaviour.MoveSpriteTo(-direction);
					}
				}
			}
		}
	}

    private void TriggerEnter2D(Collider2D other) {
		var entity = other.transform.root.gameObject.GetComponentInChildren<BaseEntity>();
		if (entity != null) {
			if (entity.tag == enemyTag) {
				if (!entities.Contains(entity)) {
					entities.Add (entity);
				}
			}
		}
	}

	private void TriggerExit2D(Collider2D other) {
		var entity = other.transform.root.gameObject.GetComponentInChildren<BaseEntity>();
		if (entity != null) {
			if (entities.Contains(entity)) {
				entities.Remove(entity);
			}
		}
	}

	private void PlaySound(bool playDeath) {
		var sound = new GameObject();
		sound.transform.position = this.Position;
		var source = sound.AddComponent<AudioSource>();
		source.clip = playDeath ? deathSound : damageSound;
		source.volume = 0.34f;
		source.Play();
		Destroy (sound, source.clip.length);
	}

	public void OnDestroy() {
		if (MobCounter.Instance) {
			MobCounter.Instance.MinusOneToAlive();
		}

		deathAnimation.transform.parent = null;
		deathAnimation.Play();
		Destroy (deathAnimation.gameObject, deathAnimation.duration);

		PlaySound(true);

		if (Application.isPlaying && dropper != null) {
			dropper.DropPickups(this.Position);
		}
	}
}
