using Proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGroupObjectPooler : PoolBase<GroupBulletTest>
{
    public static BulletGroupObjectPooler SharedInstance;

    private void Awake()
    {
        if (SharedInstance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            SharedInstance = this;
        }
    }
}
