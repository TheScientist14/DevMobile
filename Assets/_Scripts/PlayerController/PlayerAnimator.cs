using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimator : MonoBehaviour
{
	private PlayerMovement m_PlayerMovement;

	[SerializeField] private Animator m_SpaceshipAnimator;

	// Start is called before the first frame update
	void Start()
	{
		m_PlayerMovement = GetComponent<PlayerMovement>();
	}

	// Update is called once per frame
	void Update()
	{
		m_SpaceshipAnimator.SetFloat("HorizontalMovement", m_PlayerMovement.GetVelocity().x);
	}
}
