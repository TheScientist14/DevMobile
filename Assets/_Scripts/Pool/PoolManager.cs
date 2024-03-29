using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    private List<PoolBase<Poolable>> _poolsToInstantiate;

    private void Start()
    {
        InitPools();
    }

    private void InitPools()
    {
        foreach (PoolBase<Poolable> pool in _poolsToInstantiate)
        {
            Instantiate(pool, transform);
        }
    }

}
