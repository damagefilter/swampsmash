using UnityEngine;
using System.Collections;
using Pathfinding;

public class EnemyWalkingBehaviour : MonoBehaviour
{

	[SerializeField]
	protected ShootingMob actor;
	[SerializeField]
    protected float speed;
    [SerializeField]
    protected float jumpStrength;
    [SerializeField]
    protected Seeker seeker;
	//The waypoint we are currently moving towards
	protected int currentWaypoint = 0;
	public float repathRate = 0.5f;
	protected float lastRepath = -9999;
	//The calculated path
	public Path path;
	protected Waypoint targetPosition;
//	private Vector3 targetPosition;

	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 0.15f;

    protected Collider2D actorCollider;
    protected Collider2D[] collisionCheckBuffer = new Collider2D[3];
    protected bool inAir;

    public void OnPathComplete (Path p)
	{
		p.Claim (this);
		if (!p.error) {
			if (path != null) {
				path.Release (this);
			}
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;
		} else {
			p.Release (this);
			Debug.Log ("Oh noes, the target was not reachable: " + p.errorLog);
		}
		
	}

    public virtual void Start() {
        this.actorCollider = actor.GetComponentInChildren<Collider2D>();
    }

    public void FixedUpdate() {
        int groundHits = Physics2D.OverlapCircleNonAlloc(this.actorCollider.transform.position, 0.2f, collisionCheckBuffer, LayerMask.NameToLayer("World"));
        bool inair = true;
        for (int i = 0; i < groundHits; ++i) {
            if (collisionCheckBuffer[i].gameObject != this.actorCollider.gameObject) {
                inair = false;
                break;
            }
        }
        this.inAir = inair;
    }
	
	public virtual void Update ()
	{
        // Makes sure the entity is facing the right way
		actor.Animator.SetFloat("Horizontal", actor.Rigidbody.velocity.x);

		if (Time.time - lastRepath > repathRate && seeker.IsDone ()) {
            FindNewTarget(Waypoint.WaypointType.EDGE, Waypoint.WaypointType.WALKABLE);
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
		
		if (currentWaypoint > path.vectorPath.Count) return; 
		if (currentWaypoint == path.vectorPath.Count) {
			// Debug.Log ("End Of Path Reached");
			currentWaypoint++;
			return;
		}

        var waypointPos = path.vectorPath[currentWaypoint];

        Vector3 dir = this.CalculateDirectionVector(actor.Position, waypointPos);
        actor.UpdateMovement(dir * this.speed);
        if (Mathf.Abs(actor.Position.y - waypointPos.y) > nextWaypointDistance && !inAir) {
            actor.Rigidbody.AddForce(new Vector2(0f, dir.y * jumpStrength));
        }

        // For this to work the actor must be on z = 0 as well as the waypoint.
        // Otherwise distance checks may not be reliable
        if ((currentWaypoint < path.vectorPath.Count) && ((path.vectorPath[currentWaypoint] - actor.Position).sqrMagnitude < this.nextWaypointDistance * this.nextWaypointDistance)) {
            Debug.Log("Targeting next waypoint.");
            currentWaypoint++;
        }
	}

    protected Vector2 CalculateDirectionVector(Vector2 from, Vector2 to) {
        return (to - from).normalized;
    }

    protected void FindNewTarget(params Waypoint.WaypointType[] wpTypes) {
		// If we have entities in range, select the closest one as target point.
		Vector2 target = Vector2.zero;
		float closestDistance = -1f;
		foreach (var entity in actor.Entities) {
			if (entity == null) {
				continue;
			}
			float distance = Vector2.Distance(actor.Position, entity.Position);
			if (closestDistance < 0 || distance < closestDistance) {
				closestDistance = distance;
				target = entity.Position;
			}
		}
		if (target != Vector2.zero) {
			targetPosition = WaypointRegistry.Instance.GetClosest(target, targetPosition, wpTypes);
            Debug.DrawLine(actor.Position, targetPosition.Position, Color.red, repathRate);
            //			targetPosition = target;
            return;
		}
		
		Vector3 v;
		var spawn = actor.Position;
		float angle = UnityEngine.Random.Range(-90, 90); // Results in a 180 half-circle arc
        float diameter = UnityEngine.Random.Range(1.5f, 5.5f);
        v.x = spawn.x + diameter * Mathf.Sin(angle * Mathf.Deg2Rad);
		v.y = spawn.y + diameter * Mathf.Cos(angle * Mathf.Deg2Rad);
		v.z = 0;
		target = AstarPath.active.GetNearest(v).clampedPosition;
		targetPosition = WaypointRegistry.Instance.GetClosest(target, targetPosition, wpTypes);
        Debug.DrawLine(actor.Position, targetPosition.Position, Color.cyan, repathRate);
	}
}
