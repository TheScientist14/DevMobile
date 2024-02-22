using Proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupBulletTest : BulletPoolingTest
{
    BulletGroupObjectPooler _pool;
    private void Start()
    {
        _pool = BulletGroupObjectPooler.SharedInstance;
    }

    protected override void OnDeactivate()
    {
        _pool.UnloadObject(this);
    }
}
