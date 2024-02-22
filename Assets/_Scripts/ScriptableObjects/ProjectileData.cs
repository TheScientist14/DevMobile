using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectile/New Projectile Data")]
public class BulletData : ScriptableObject
{

    [SerializeField]
    private float _speed;
    public float Speed => _speed;


    [SerializeField]
    private Vector2 _colliderSize;
    public Vector2 ColliderSize => _colliderSize;


    [SerializeField]
    private Sprite _sprite;
    public Sprite Sprite => _sprite;


    [SerializeField]
    private float _damage;
    public float  Damage => _damage;


    [SerializeField] private bool _selfDestructOnCollision = true;
    public bool SelfDestructOnCollision => _selfDestructOnCollision;


    [SerializeField] private bool _damageOverTime;
    public bool DamageOverTime => _damageOverTime;


    [ShowIf("_damageOverTime")]
    [SerializeField] private float _damagePerHit;
    public float DamagePerHit => _damagePerHit;

    [ShowIf("_damageOverTime")]
    [SerializeField] private float _hitRate = 1.0f;
    public float HitRate => _hitRate;

}
