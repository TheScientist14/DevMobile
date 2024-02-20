using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{

    [SerializeField]
    private List<PoolBase> _poolsToInstantiate;


    private void Start()
    {
        InitPools();
    }

    private void InitPools()
    {
        foreach (PoolBase pool in _poolsToInstantiate)
        {
            Instantiate(pool, transform);
        }
    }

}
