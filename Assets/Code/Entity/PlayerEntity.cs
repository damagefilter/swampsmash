using UnityEngine;
using System.Collections;

/// <summary>
/// Just for tagging the player entity
/// </summary>
public class PlayerEntity : BaseEntity {
	[SerializeField] private ParticleSystem damageEffect;
	[SerializeField] private AudioClip damageSound;
	[SerializeField] private AudioClip deathSound;
	[SerializeField] private ParticleSystem deathEffect;

	public override void RemoveHealth(float value) {
		this.currentHealth -= value;
		damageEffect.Play();
		PlaySound(false);

		if (this.currentHealth <= 0) {
			new GameOverHook(MobCounter.Instance.TotalMobsKilled, (int)TheTime.Instance.TotalTimeElapsed).Call ();
			Destroy (this.gameObject);

		}
	}

	public void OnDestroy() {
		deathEffect.Play ();
		PlaySound(true);
		Destroy (deathEffect.gameObject, deathEffect.duration);
	}

	private void PlaySound(bool playDeath) {
		var sound = new GameObject();
		sound.transform.position = this.Position;
		var source = sound.AddComponent<AudioSource>();
		source.clip = playDeath ? deathSound : damageSound;
		source.Play();
		source.volume = 0.34f;
		Destroy (sound, source.clip.length);
	}
}
