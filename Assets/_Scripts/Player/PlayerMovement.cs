using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerMovement : MonoBehaviour
{
	private PlayerController m_PlayerController;

	// Scene reference
	[SerializeField] private BoxCollider2D m_PlayerBounds;

	//[TEMP] To get from some player config that regroups player data
	[SerializeField] private float m_Speed;
	[SerializeField] private float m_BoundsEffectThreshold;
	[SerializeField] private AnimationCurve m_BoundsProximitySlowdown;

	private Vector3 m_CurrentVelocity = Vector3.zero;

	private void Awake()
	{
		m_PlayerController = GetComponent<PlayerController>();
	}

	private void FixedUpdate()
	{
		Vector2 movementInputValue = m_PlayerController.GetMovementInputValue();
		Move(movementInputValue);
	}

	private void Move(Vector2 iInputDir)
	{
		Vector2 step = m_Speed * Time.fixedDeltaTime * iInputDir;
		Vector2 slowdown = ComputeSlowdown(step);

		m_CurrentVelocity = new Vector3(step.x * slowdown.x, step.y * slowdown.y, 0f);
		transform.position += m_CurrentVelocity;
	}

	private Vector2 ComputeSlowdown(Vector2 iStep)
	{
		Vector2 slowdown;
		if(iStep.x > 0)
			slowdown.x = EvaluateDirectionalSlow(transform.position.x, m_PlayerBounds.bounds.max.x);
		else
			slowdown.x = EvaluateDirectionalSlow(transform.position.x, m_PlayerBounds.bounds.min.x);

		if(iStep.y > 0)
			slowdown.y = EvaluateDirectionalSlow(transform.position.y, m_PlayerBounds.bounds.max.y);
		else
			slowdown.y = EvaluateDirectionalSlow(transform.position.y, m_PlayerBounds.bounds.min.y);

		return slowdown;
	}

	private float EvaluateDirectionalSlow(float iPos, float iLimit)
	{
		if(m_BoundsEffectThreshold > 0)
		{
			return m_BoundsProximitySlowdown.Evaluate(Mathf.Abs(iPos - iLimit) / m_BoundsEffectThreshold);
		}
		else
		{
			return Mathf.Abs(iPos - iLimit) > Mathf.Epsilon ? 1.0f : 0.0f;
		}
	}

	public Vector3 GetVelocity()
	{
		return m_CurrentVelocity;
	}
}
