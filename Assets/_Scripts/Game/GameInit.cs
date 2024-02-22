using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    [SerializeField] private PlayerMovement m_Player;

    [SerializeField] private Transform m_StartPos;
    [SerializeField] private AnimationCurve m_ScaleAnimation;

    private GameStateMachine m_StateMachine;

    private bool m_Animate = false;
    private float m_AnimationTime = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        m_StateMachine = GameManager.Get().GetStateMachine();
		m_StateMachine.OnStateChange += _OnStateChange;
	}

	private void Update()
	{
        if(m_Animate)
        {
            m_AnimationTime += Time.deltaTime;

            if(m_AnimationTime > m_ScaleAnimation.length)
            {
                m_Player.transform.localScale = Vector3.one;
                m_Animate = false;
				m_Player.enabled = true;
				m_StateMachine.SetState(new PlayGameState());
				return;
            }

            m_Player.transform.localScale = Vector3.one * m_ScaleAnimation.Evaluate(m_AnimationTime);
        }
	}

	private void _OnStateChange(IGameState iPrevState, IGameState iCurState)
    {
        if(iCurState is InitGameState)
            InitAnimation();
    }

    private void InitAnimation()
    {
		m_Animate = true;
        m_AnimationTime = 0;
        m_Player.transform.position = m_StartPos.position;
        m_Player.gameObject.SetActive(true);
        m_Player.enabled = false;
	}
}
