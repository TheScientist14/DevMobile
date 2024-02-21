using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto
{
    public class SpaceShipControls : MonoBehaviour, IDamageable
    {
        private Rigidbody2D _rb;
        private Animator _animator;

        [Header("Movements")]
        [SerializeField]
        private float _torqueForce = 1850.0f;
        [SerializeField]
        private float _moveForce = 273.0f;
        [Header("Animations")]
        [SerializeField]
        private string _movingAnim = "Moving";
        [SerializeField]
        private string _staticAnim = "Stationnary";
        private string _currentAnim;
        [Header("Missile")]
        [SerializeField]
        private Transform _muzzleOffset;
        [SerializeField]
        private GameObject _missilePrefab;
        [SerializeField, Min(0.05f)]
        private float _shootingDelay;
        private float _ShootDelayCounter;

        [Header("Life")]
        [SerializeField]
        private int _startLifeNb;

        private int _lifes;
        /// <summary>
        /// Remaining lifes of the ship.
        /// Setter adds the value.
        /// </summary>
        public int Lifes { get { return _lifes; } set { _lifes += value; } }

        [SerializeField, Range(0.2f, 5.0f)]
        private float _invisibilityDuration = 1.5f;
        private float _invisibilityTimeCounter = 0.0f;

        [SerializeField, Range(5, 70)]
        private int _numberOfOscillations = 25;
        [SerializeField, Min(1)]
        private int _oscilationPower = 2;

        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider;

        [SerializeField]
        private GameObject _deathUIToShow;
        [SerializeField]
        private ScoreData _scores;

        private bool _canTakeDamage { get { return _invisibilityTimeCounter >= _invisibilityDuration && Lifes >= 0; } }

        [Header("Collisions")]
        [SerializeField, Layer] int _spaceShipLayer;
        [SerializeField, Layer] int _hitLayer;
        [SerializeField] float _collisionDamage = 10.0f;
        public void TakeDamage(DamageCauser damageCauser_, float damages_, Vector2 impactPoint_)
        {

            if (!_canTakeDamage)
            {
                return;
            }

            if (damageCauser_ == DamageCauser.Asteroid)
            {
                _invisibilityTimeCounter = 0.0f;
            }
            _lifes--;
            if (_lifes < 0)
            {
                if (_deathUIToShow != null)
                    _deathUIToShow.SetActive(true);
                Destroy(gameObject);
            }
            gameObject.layer = _hitLayer;
            StartCoroutine(DamageCoroutine());
        }

        IEnumerator DamageCoroutine()
        {
            while (_invisibilityTimeCounter < _invisibilityDuration)
            {
                _invisibilityTimeCounter += Time.deltaTime;
                float ratio = _invisibilityTimeCounter / _invisibilityDuration;
                float value = Mathf.Cos(Mathf.Pow(ratio, _oscilationPower) * _numberOfOscillations * Mathf.PI * 2.0f);
                _spriteRenderer.enabled = value > 0f;
                yield return 0;
            }
            _spriteRenderer.enabled = true;
            gameObject.layer = _spaceShipLayer;
        }

        private bool TryShoot()
        {
            if (_ShootDelayCounter >= _shootingDelay)
            {
                _ShootDelayCounter = 0;
                GameObject bullet = BulletsPool.SharedInstance.GetPooledObject().gameObject;
                bullet.transform.position = _muzzleOffset.position;
                bullet.transform.rotation = _muzzleOffset.rotation;
                // bullet.SetActive(true);
                // Instantiate(_missilePrefab, _muzzleOffset.position, _muzzleOffset.rotation);
                return true;
            }
            return false;
        }

        #region BUILD IN METHODS
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _scores._spaceShip = this;
        }

        private void Start()
        {
            _lifes = _startLifeNb;
            _animator.Play(_staticAnim);
            _currentAnim = _staticAnim;
            Application.targetFrameRate = Mathf.Min(60, (int)Screen.currentResolution.refreshRateRatio.value);
            gameObject.layer = _spaceShipLayer;
            _invisibilityTimeCounter = _invisibilityDuration;
        }

        void Update()
        {
            _ShootDelayCounter += Time.deltaTime;
        }

        #region COLLISIONS
        private void FixedUpdate()
        {
            bool GoingRight = Input.GetKey(KeyCode.RightArrow);
            bool GoingLeft = Input.GetKey(KeyCode.LeftArrow);
            for (int i = 0; i < Input.touchCount; ++i)
            {
                float xRatio = Input.GetTouch(i).position.x / Screen.width;
                if (xRatio > 0.5f)
                    GoingRight = true;
                else
                    GoingLeft = true;
            }
            #region Inputs
            if (GoingLeft)
            {
                _rb.AddTorque(_torqueForce);
            }
            if (GoingRight)
            {

                _rb.AddTorque(-_torqueForce);
            }
            if (GoingLeft && GoingRight)
            {
                _rb.AddForce(transform.up * _moveForce);
                if (_currentAnim != _movingAnim)
                {
                    _animator.Play(_movingAnim);
                    _currentAnim = _movingAnim;
                }

                TryShoot();
            }
            else
            {
                if (_currentAnim != _staticAnim)
                {
                    _animator.Play(_staticAnim);
                    _currentAnim = _staticAnim;
                }

            }
            #endregion
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(DamageCauser.SpaceShip, _collisionDamage, collision.contacts[0].point);
            }
        }

        #endregion
        #endregion

        public void OnDrawGizmosSelected()
        {
            var r = GetComponent<Renderer>();
            if (r == null)
                return;
            var bounds = r.bounds;
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(bounds.center, bounds.extents * 2);
        }
    }
}