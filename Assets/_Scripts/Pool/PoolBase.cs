using System.Collections.Generic;
using UnityEngine;

public class PoolBase<T> : MonoBehaviour
    where T : Poolable
{
    protected Queue<T> _pooledObjects = new Queue<T>();

    [SerializeField]
    protected T _objectToPool;

    [SerializeField]
    protected int _amountToPool = 5;

    [SerializeField]
    private bool _increasable = true;

    public void SetObjectToPool(T newObjectToPool)
    {
        _objectToPool = newObjectToPool;
    }

    public T GetPooledObject()
    {
        if (_pooledObjects.Count > 0)
        {
            T ObjectToPool = _pooledObjects.Dequeue();
            ObjectToPool.OnCreate();
            return ObjectToPool;
        }

        if (_increasable)
        {
            T tmp = Instantiate(_objectToPool, transform);
            tmp.OnCreate();
            return tmp;
        }
        return null;
    }

    public void UnloadObject(T objectToUnload)
    {
        if (objectToUnload is null) return;
        _pooledObjects.Enqueue(objectToUnload);
        objectToUnload.OnRecycle();
    }

    public void UnloadObjects(T[] objectsToUnload)
    {
        foreach (T objectToUnload in objectsToUnload)
        {
            if (objectToUnload is null) continue;
            objectToUnload.OnRecycle();
            _pooledObjects.Enqueue(objectToUnload);
        }
    }

    public List<T> GetPooledObjectsInList(int numberOfObjectNeeded)
    {
        List<T> list = new List<T>(numberOfObjectNeeded);
        while (list.Count < numberOfObjectNeeded)
        {
            list.Add(GetPooledObject());
        }
        return list;
    } 

    public void PopulatePool()
    {
        T tmp;
        for (int i = 0; i < _amountToPool; i++)
        {
            tmp = Instantiate(_objectToPool, transform);
            tmp.gameObject.SetActive(false);
            _pooledObjects.Enqueue(tmp);
        }
    }

    public void ResetPool()
    {
        foreach (var objInPool in _pooledObjects)
        {
            objInPool.OnLeavePool();
        }
        _pooledObjects.Clear();
    }

    protected virtual void Start()
    {
        PopulatePool();
    }
}
