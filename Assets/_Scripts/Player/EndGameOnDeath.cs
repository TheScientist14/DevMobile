using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LivingEntity))]
public class EndGameOnDeath : MonoBehaviour
{
	private GameStateMachine m_StateMachine;

	// Start is called before the first frame update
	private void Start()
	{
		LivingEntity livingEntity = GetComponent<LivingEntity>();
		livingEntity.RegisterOnDeathEvent(_OnDeath);

		m_StateMachine = GameManager.Get().GetStateMachine();
	}

	private IEnumerator DelaySetEndState()
	{
		yield return new WaitForSeconds(1);
		m_StateMachine.SetState(new EndGameState());
	}

	private void _OnDeath()
	{
		StartCoroutine(DelaySetEndState());
	}
}
