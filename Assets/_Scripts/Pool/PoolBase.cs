using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class PoolBase : MonoBehaviour
{

    protected List<GameObject> _pooledObjects;

    [SerializeField]
    protected GameObject _objectToPool;

    [SerializeField]
    protected int _amountToPool;

    [SerializeField]
    private bool _increasable;

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                return _pooledObjects[i];
            }
        }

        if (_increasable)
        {
            GameObject tmp = Instantiate(_objectToPool, transform);

            _pooledObjects.Add(tmp);

            return tmp;
        }
        

        

        return null;
    }

    public void PopulatePool()
    {
        _pooledObjects = new List<GameObject>();
        GameObject tmp;

        for (int i = 0; i < _amountToPool; i++)
        {
            tmp = Instantiate(_objectToPool, transform);
            tmp.SetActive(false);
            _pooledObjects.Add(tmp);
        }
    }

    public void ResetPool()
    {
        _pooledObjects.Clear();
    }
   
}
