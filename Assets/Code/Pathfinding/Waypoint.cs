using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {

	public enum WaypointType {
		WALKABLE,
		EDGE,
		OBSERVER,
		AIR
	}

	public void Start() {
		WaypointRegistry.Instance.Add(this);
	}

	public void OnDestroy() {
		WaypointRegistry.Instance.Remove(this);
	}

	[SerializeField] private WaypointType type;

	public bool IsWalkable {
		get {
			return type == WaypointType.WALKABLE;
		}
	}

	public bool IsEdge {
		get {
			return type == WaypointType.EDGE;
		}
	}

	public bool IsObserverPoint {
		get {
			return type == WaypointType.OBSERVER;
		}
	}

	public Vector2 Position {
		get {
			return (Vector2)this.transform.position;
		}
	}

	public WaypointType Type {
		get {
			return type;
		}
	}
}
