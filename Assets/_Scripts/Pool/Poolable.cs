using UnityEngine;
using System.Collections.Generic;

public abstract class Poolable : MonoBehaviour, IPoolable
{
    public abstract T GetPoolObject<T>() where T : Poolable;

    public abstract List<T> GetPooledObjects<T>(int NumberOfObjectsToPool) where T : Poolable;

    public virtual void OnObjectPool()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnObjectUnpool()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnLeavingPool()
    {
        Destroy(gameObject);
    }
}
