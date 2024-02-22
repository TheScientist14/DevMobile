using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto
{
public class BulletPoolingTest : Poolable
{
    float _spawnDistance = 10.0f;

    float _lifeTime = 10.0f;

    float _lifeTimeCounter = 0.0f;

    public override void OnCreate()
    {
        transform.position = Random.insideUnitCircle * _spawnDistance;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(-180.0f, 180.0f));
        _lifeTimeCounter = _lifeTime;
        base.OnCreate();
    }

    protected virtual void OnDeactivate()
    {
        OnRecycle();
    }

    private void Update()
    {
        _lifeTimeCounter -= Time.deltaTime;
        if (_lifeTimeCounter < 0 )
        {
            OnDeactivate();
        }
    }


}
}
