using UnityEngine;
using System.Collections;

/// <summary>
/// Mixes 2 audio sources together with crossfade and some such things
/// </summary>
public class BackgroundMusicMixer : MonoBehaviour {
	#region Singleton
	private static BackgroundMusicMixer _instance;
	public static BackgroundMusicMixer Instance {
		get {
			return _instance;
		}
	}

	#endregion
	[SerializeField] private AudioClip clipOne;
	[SerializeField] private AudioClip clipTwo;

	[SerializeField] private float maxVolume = 0.45f;
	[SerializeField] private float fadeFactor = 0.8f;

	private AudioSource trackOne;
	private AudioSource trackTwo;

	// Use this for initialization
	void Start () {
		if (_instance != null) {
			Destroy (this.gameObject);
			return;
		}
		_instance = this;
		DontDestroyOnLoad(this.gameObject);
		trackOne = this.gameObject.AddComponent<AudioSource>();
		trackOne.clip = clipOne;
		trackOne.loop = true;
		trackOne.volume = maxVolume;

		trackTwo = this.gameObject.AddComponent<AudioSource>();
		trackTwo.clip = clipTwo;
		trackTwo.loop = true;
		trackTwo.volume = maxVolume;

		trackOne.Play();
	}

	public void StartTrackOne() {
		if (trackTwo.isPlaying) {
			trackTwo.Stop();
		}
		trackOne.Play();
	}

	public void StartTrackTwo() {
		if (trackOne.isPlaying) {
			trackOne.Stop();
		}
		trackTwo.Play();
	}

	/// <summary>
	/// Crossfades the first track to the second. 
	/// So second is playing after the cross fade.
	/// </summary>
	public void CrossfadeOneToTwo() {
		StartCoroutine(InternalCrossfadeAB());
	}

	public void CrossfadeTwoToOne() {
		StartCoroutine(InternalCrossfadeBA());
	}

	IEnumerator InternalCrossfadeAB() {
		bool isCrossfading = true;
		if (!trackTwo.isPlaying) {
			trackTwo.Play();
			trackTwo.volume = 0f;
		}
		while (isCrossfading) {
			trackOne.volume -= fadeFactor * Time.deltaTime;

			if (trackTwo.volume < maxVolume) {
				trackTwo.volume += fadeFactor * Time.deltaTime;
			}
			else {
				trackOne.volume = 0f;
				isCrossfading = false;
				break;
			}
			yield return null;
		}
		trackOne.Stop ();
	}

	IEnumerator InternalCrossfadeBA() {
		bool isCrossfading = true;
		if (!trackOne.isPlaying) {
			trackOne.Play();
			trackOne.volume = 0f;
		}
		while (isCrossfading) {
			trackTwo.volume -= fadeFactor * Time.deltaTime;

			if (trackOne.volume < maxVolume) {
				trackOne.volume += fadeFactor * Time.deltaTime;
			}
			else {
				isCrossfading = false;
				trackTwo.volume = 0f;
			}

			yield return null;
		}
		trackTwo.Stop ();
	}
}
