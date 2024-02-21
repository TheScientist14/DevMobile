using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// Will first go to Init Pos, then follow its path.
public class MovingEntity : MonoBehaviour
{
	[SerializeField] private Vector3 m_InitPos;
	private bool hasStarted = false;

	[SerializeField] private string m_PathParentName;
	[SerializeField] private bool m_LoopPath = false;
	private List<Vector2> m_Waypoints = new List<Vector2>();
	private Vector2 m_CurPosOnPath;
	private int m_NextWaypointIdx = 1;
	private Vector2 m_OffsetToPath;

	[SerializeField] private float m_Speed = 1;

	// Start is called before the first frame update
	void Start()
	{
		GameObject pathesParent = GameObject.FindGameObjectWithTag("Pathes");
		Assert.IsNotNull(pathesParent, "No pathes container found, make sure it has the tag \"Pathes\".");
		Transform pathParent = pathesParent.transform.Find(m_PathParentName);
		Assert.IsNotNull(pathesParent, $"No path found with name \"{m_PathParentName}\". Make sure it is placed directly under the pathes container.");
		foreach(Transform waypoint in pathParent)
		{
			Vector2 pos = waypoint.position;
			if(m_Waypoints.Count > 0)
			{
				if((pos - m_Waypoints[m_Waypoints.Count - 1]).magnitude <= Mathf.Epsilon)
				{
					Debug.LogWarning($"Path {m_PathParentName}: waypoint {waypoint.gameObject.name} is too close from the previous one.");
					continue;
				}
			}

			m_Waypoints.Add(pos);
		}

		if(m_Waypoints.Count <= 1)
		{
			Debug.Log("No path, destroying EnemyBehaviour of " + gameObject.name);
			Destroy(this);
			return;
		}

		m_CurPosOnPath = m_Waypoints[0];
	}

	// Update is called once per frame
	void Update()
	{
		float dist = m_Speed * Time.deltaTime;
		if(!hasStarted)
		{
			dist = Introduce(dist);
			if(dist <= Mathf.Epsilon)
				return;
			hasStarted = true;
			m_OffsetToPath = (Vector2)transform.position - m_CurPosOnPath;
		}

		Progress(dist);
	}

	private float Introduce(float iDist)
	{
		Vector3 pos = transform.position;
		transform.position = Vector3.MoveTowards(pos, m_InitPos, iDist);

		if((transform.position - m_InitPos).sqrMagnitude <= Mathf.Epsilon)
			return iDist - (transform.position - pos).magnitude;
		else
			return 0;
	}

	private void Progress(float iDist)
	{
		_Progress(m_CurPosOnPath, iDist);
	}

	private void _Progress(Vector2 iPosOnPath, float iDist)
	{
		Vector2 nextWaypoint = m_Waypoints[m_NextWaypointIdx];
		Vector2 toNextWaypoint = nextWaypoint - iPosOnPath;
		float toNextWaypointLength = toNextWaypoint.magnitude;

		if(toNextWaypointLength < iDist)
		{
			NextWaypointIdx();
			_Progress(nextWaypoint, iDist - toNextWaypointLength);
		}
		else
		{
			m_CurPosOnPath = iPosOnPath + toNextWaypoint * (iDist / toNextWaypointLength);
			transform.position = m_CurPosOnPath + m_OffsetToPath;
		}
	}

	private void NextWaypointIdx()
	{
		m_NextWaypointIdx++;

		if(m_NextWaypointIdx >= m_Waypoints.Count)
		{
			m_NextWaypointIdx = 0;

			if(!m_LoopPath) // still set idx to 0 to avoid errors
				Destroy(gameObject);
		}
	}

	public void SetStartPos(Vector2 iPos)
	{
		m_InitPos = iPos;
	}
}
