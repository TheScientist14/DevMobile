using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSyncer : MonoBehaviour
{
    public ProjectileData m_ProjectileData;

    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    public SpriteRenderer ProjectileRenderer => m_SpriteRenderer;
    [SerializeField] private BoxCollider2D m_BoxCollider;
    public BoxCollider2D ProjectileCollider => m_BoxCollider;
    // [SerializeField] private Rigidbody2D m_Rigidbody2;

    [SerializeField] private DamageDealer m_DamageDealer;
    public DamageDealer ProjectileDamageDealer => m_DamageDealer;

    [SerializeField] private ProjectileMovement m_ProjectileMovement;
    public ProjectileMovement ProjectileMovement => m_ProjectileMovement;
    
    public void SynchronizeWithProjectileData()
    {
        if (m_SpriteRenderer != null)
            m_SpriteRenderer.sprite = m_ProjectileData.Sprite;
        if (m_BoxCollider != null)
            m_BoxCollider.size = m_ProjectileData.ColliderSize;
        if (m_DamageDealer != null)
            m_DamageDealer.ChangeDamageParams(m_ProjectileData.Damage, m_ProjectileData.SelfDestructOnCollision, m_ProjectileData.DamageOverTime, m_ProjectileData.DamagePerHit, m_ProjectileData.HitRate);
        if (m_ProjectileMovement != null)
            m_ProjectileMovement.SetMovingSpeed(m_ProjectileData.Speed);
    }

    public void SynchronizeWithProjectileData(ProjectileData data)
    {
        if (data != null)
        {
            m_ProjectileData = data;
            SynchronizeWithProjectileData();
        }
    }
}
