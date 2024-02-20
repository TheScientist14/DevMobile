using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTest : MonoBehaviour
{
    [SerializeField]
    private float _Speeeeeeeeed = 125.0f;
    [SerializeField]
    private float _LifeTime = 4.0f;
    [SerializeField]
    private bool _IsDebugging;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * _Speeeeeeeeed;
        Destroy(gameObject, _LifeTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damagable = collision.gameObject.GetComponent<IDamageable>();
        if (_IsDebugging)
            Debug.Log(collision.gameObject.name);
        if (damagable != null)
            damagable.TakeDamage(DamageCauser.Bullets, _Speeeeeeeeed, transform.position);
        Destroy(gameObject);
    }
}
