using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Proto
{
    [CreateAssetMenu(menuName = "Scriptable/Movements")]
    public class MovementParameters : ScriptableObject
    {
        [Header("Floor movements")]
        [SerializeField] private float _speed;
        public float Speed { get { return _speed; } }


        #region RigidBody2D
        [Header("RigidBody2D")]

        [SerializeField] private bool _simulated;
        public bool Simulated { get { return _simulated; } }

        [SerializeField][Range(0f, 300f)] private float _mass;
        public float Mass { get { return _mass; } }

        [SerializeField][Range(0f, 10f)] private float _linearDrag;
        public float LinearDrag { get { return _linearDrag; } }

        [SerializeField][Range(0f, 10f)] private float _angularDrag;
        public float AngularDrag { get { return _angularDrag; } }

        [SerializeField][Range(-10, 20)] private float _gravityScale;
        public float GravityScale { get { return _gravityScale; } }

        [SerializeField] private bool _freezeXPosition;
        public bool FreezeXPosition { get { return _freezeXPosition; } }

        [SerializeField] private bool _freezeYPosition;
        public bool FreezeYPosition { get { return _freezeYPosition; } }

        [SerializeField] private bool _freezeZRotation;
        public bool FreezeZRotation { get { return _freezeZRotation; } }

        #endregion

        #region Physic Material
        [Header("Physic Material")]
        [SerializeField] private float _friction;
        public float Friction { get { return _friction; } }

        [SerializeField] private float _bounciness;
        public float Bounciness { get { return _bounciness; } }
        #endregion
    }
}