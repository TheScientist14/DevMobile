using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto
{
    public class BulletTest : MonoBehaviour
    {
        [SerializeField]
        private float _Speeeeeeeeed = 125.0f;
        [SerializeField]
        private float _LifeTime = 4.0f;
        [SerializeField]
        private bool _IsDebugging;


        Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        void OnEnable()
        {
            _rigidbody2D.velocity = transform.up * _Speeeeeeeeed;
            // Destroy(gameObject, _LifeTime);
            StartCoroutine(DeactivateCoroutine());
        }

        private IEnumerator DeactivateCoroutine()
        {
            yield return new WaitForSeconds(_LifeTime);
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
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