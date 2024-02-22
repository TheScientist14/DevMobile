using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameState : IGameState
{
    private float m_PlayTime;

    public void Enter()
    {
        m_PlayTime = 0f;
    }

    public void Exit()
    {
    }

    public void Update(float iDeltaTime)
    {
        m_PlayTime += iDeltaTime;
    }

    public float GetPlayTime()
    {
        return m_PlayTime;
    }
}

