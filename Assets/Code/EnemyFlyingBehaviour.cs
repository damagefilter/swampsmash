using UnityEngine;
using System.Collections;
using Pathfinding;

public class EnemyFlyingBehaviour : EnemyWalkingBehaviour
{
	public override void Update ()
	{
		actor.Animator.SetFloat("Horizontal", actor.Rigidbody.velocity.x);

		if (Time.time - lastRepath > repathRate && seeker.IsDone ()) {
			FindNewTarget(Waypoint.WaypointType.AIR);
			if (targetPosition != null) {
				lastRepath = Time.time + Random.value * repathRate * 0.5f;
				seeker.StartPath (transform.position, targetPosition.Position, OnPathComplete);
			}
			else {
				Debug.Log ("FindTarget didn't return a node to go to");
			}
		}
		
		if (path == null) {
			//We have no path to move after yet
			return;
		}
		
		if (currentWaypoint > path.vectorPath.Count) {
			return; 
		}
		if (currentWaypoint == path.vectorPath.Count) {
			Debug.Log ("End Of Path Reached");
			currentWaypoint++;
			return;
		}

		//Direction to the next waypoint
		Vector3 dir = (path.vectorPath [currentWaypoint] - transform.position).normalized;
		actor.Rigidbody.AddForce(dir * speed);
		if (Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]) <= nextWaypointDistance) {
			Debug.Log ("Targeting next waypoint.");
			currentWaypoint++;
			return;
		}
	}
}
