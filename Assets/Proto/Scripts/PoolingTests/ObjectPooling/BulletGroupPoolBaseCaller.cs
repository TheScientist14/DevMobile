using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGroupPoolBaseCaller : TestBulletSpawnerBase
{
    BulletGroupObjectPooler bulletGroupObjectPooler;
    private void Start()
    {
        bulletGroupObjectPooler = BulletGroupObjectPooler.SharedInstance;
    }
    protected override void OnSingleSpawnCalled()
    {
        bulletGroupObjectPooler.GetPooledObject();
    }
}
