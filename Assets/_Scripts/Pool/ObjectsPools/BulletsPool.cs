using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsPool : PoolBase
{
    public static BulletsPool SharedInstance;

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

    private void Start()
    {
        PopulatePool();
    }

}
