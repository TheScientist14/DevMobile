using System.Collections.Generic;
using UnityEngine;

public class PoolBase : MonoBehaviour
{
    protected Queue<GameObject> _pooledObjects = new Queue<GameObject>();

    [SerializeField]
    protected GameObject _objectToPool;

    [SerializeField]
    protected int _amountToPool = 5;

    [SerializeField]
    private bool _increasable = true;

    public GameObject GetPooledObject()
    {
        if (_pooledObjects.Count > 0)
            return _pooledObjects.Dequeue();

        if (_increasable)
        {
            GameObject tmp = Instantiate(_objectToPool, transform);

            _pooledObjects.Enqueue(tmp);

            return tmp;
        }
        
        return null;
    }

    public List<GameObject> GetPooledObjectsInList(int numberOfObjectNeeded)
    {
        List<GameObject> list = new List<GameObject>(numberOfObjectNeeded);
        while (list.Count < numberOfObjectNeeded)
        {
            list.Add(GetPooledObject());
        }
        return list;
    } 

    public void PopulatePool()
    {
        GameObject tmp;

        for (int i = 0; i < _amountToPool; i++)
        {
            tmp = Instantiate(_objectToPool, transform);
            tmp.SetActive(false);
            _pooledObjects.Enqueue(tmp);
        }
    }

    public void ResetPool()
    {
        _pooledObjects.Clear();
    }
   
}
