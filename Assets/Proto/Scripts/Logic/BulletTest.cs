using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto
{
    public class BulletTest : Poolable
    {
        private static Queue<BulletTest> _pool = new Queue<BulletTest>();

        [SerializeField]
        private float _Speeeeeeeeed = 125.0f;
        [SerializeField]
        private float _LifeTime = 4.0f;
        [SerializeField]
        private bool _IsDebugging;

        [SerializeField]
        private static BulletTest _prefab;

        Rigidbody2D _rigidbody2D;

        public static BulletTest Create()
        {
            if(_pool.Count == 0)
            {
                BulletTest go = Instantiate(_prefab);
                return go;
            }
            return _pool.Dequeue();
        }

        public void Recycle()
        {
            _pool.Enqueue(this);
        }

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void InitializeBullet()
        {
            gameObject.SetActive(true);
            _rigidbody2D.velocity = transform.up * _Speeeeeeeeed;
            // Destroy(gameObject, _LifeTime);
            StartCoroutine(DeactivateCoroutine());
        }

        private IEnumerator DeactivateCoroutine()
        {
            yield return new WaitForSeconds(_LifeTime);
            gameObject.SetActive(false);
        }
        public override void OnCreate()
        {
            
        }
        public override void OnRecycle()
        {
            StopAllCoroutines();
            base.OnRecycle();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDamageable damagable = collision.gameObject.GetComponent<IDamageable>();
            if (_IsDebugging)
                Debug.Log(collision.gameObject.name);
            if (damagable != null)
                damagable.TakeDamage(DamageCauser.Bullets, _Speeeeeeeeed, transform.position);
            // Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}