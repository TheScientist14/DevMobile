using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] private float m_Speed;

    private Vector2 m_Direction;

    public void Init(Vector2 iInputDir)
    {
        m_Direction = iInputDir.normalized;
    }

    public void SetMovingSpeed(float speed)
    {
        m_Speed = speed;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 step = m_Speed * Time.fixedDeltaTime * m_Direction;
        transform.position += new Vector3(step.x, step.y, 0f);
    }
}
