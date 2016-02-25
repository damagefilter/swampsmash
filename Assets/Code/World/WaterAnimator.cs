using UnityEngine;
using System.Collections;

public class WaterAnimator : MonoBehaviour {

	[SerializeField] private float planeOffset = 1.85f;
	[SerializeField] private float waveLength = 1.85f;

	[SerializeField] private float backplaneSpeed = 0.5f;
	[SerializeField] private float middlePlaneSpeed = 0.25f;
	[SerializeField] private float frontPlaneSpeed = 0.15f;

	[Tooltip("All sprites that will be animated as one.")]
	[SerializeField] private SpriteRenderer[] sprites;

	private SpriteRenderer[] negativeOffsetPlanes;
	private SpriteRenderer[] positiveOffsetPlanes;

	private Vector2[] originalStartPositions;


	// Use this for initialization
	void Start () {
		// For starters, create 2 new versions with offsetted x coordinates
		this.negativeOffsetPlanes = new SpriteRenderer[sprites.Length];
		this.positiveOffsetPlanes = new SpriteRenderer[sprites.Length];
		this.originalStartPositions = new Vector2[sprites.Length]; // All we need. We can get the other start positions by applying the offsets again
		for(int i = 0; i < sprites.Length; ++i) {
			this.originalStartPositions[i] = sprites[i].transform.position;
			// Make sure sprites parents transform is this water animator
			sprites[i].transform.parent = this.transform;
			// Create a new plane for this
			var negative = (GameObject)Instantiate(sprites[i].gameObject, sprites[i].transform.position, sprites[i].transform.rotation);

			// Prepare the offset
			var npos = negative.transform.position;
			npos.x -= planeOffset;
			// Apply the offset
			negative.transform.position = npos;
			// Get the sprite renderer to adjust the order
			var nrenderer = negative.GetComponent<SpriteRenderer>();
			nrenderer.sortingOrder = sprites[i].sortingOrder - 10;
			negativeOffsetPlanes[i] = nrenderer;

			var positive = (GameObject)Instantiate(sprites[i].gameObject, sprites[i].transform.position, sprites[i].transform.rotation);
			// Make sure sprites parents transform is this water animator
			positive.transform.parent = this.transform;
			// Prepare the offset
			var ppos = positive.transform.position;
			ppos.x += planeOffset;
			// Apply the offset
			positive.transform.position = ppos;
			// Get the sprite renderer to adjust the order
			var prenderer = positive.GetComponent<SpriteRenderer>();
			prenderer.sortingOrder = sprites[i].sortingOrder + 10;
			positiveOffsetPlanes[i] = prenderer;


		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < sprites.Length; ++i) {
			// Move the background planes
			var original = this.originalStartPositions[i];
			var backPos = original;
			backPos.x -= planeOffset;

			backPos.x = backPos.x + Mathf.Sin(Time.time * this.backplaneSpeed) * waveLength;
			this.negativeOffsetPlanes[i].transform.position = backPos;

			// Move the moddle planes
			var middlePos = original;
			
			middlePos.x = middlePos.x + Mathf.Sin(Time.time * this.middlePlaneSpeed) * waveLength;
			this.sprites[i].transform.position = middlePos;

			// Move the front planes
			// Move the moddle planes
			var frontPos = original;
			
			frontPos.x = frontPos.x + Mathf.Sin(Time.time * this.frontPlaneSpeed) * waveLength;
			this.positiveOffsetPlanes[i].transform.position = frontPos;
		}
	}
}
