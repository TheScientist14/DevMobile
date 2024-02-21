using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

[System.Serializable]
public class PoolHandler
{
    protected Queue<Poolable> m_poolQueue = new Queue<Poolable>();

    [SerializeField]
    protected Poolable m_poolObject;

    public Poolable GetPooledObject()
    {
        if (m_poolQueue.Count <= 0)
        {
            // Poolable newObject = m_poolObject.Instantiate(m_poolObject);
            // CREATE IPoolable object?
            // m_poolQueue.Enqueue(newObject);
            return null;
        }
        else
        {
            return m_poolQueue.Dequeue();
        }
    }

    public void AddObjectToPool(Poolable poolableObject)
    {
        if (poolableObject is null) return;
        m_poolQueue.Enqueue(poolableObject);
    }

    public void ClearPool()
    {
        foreach(IPoolable pool in m_poolQueue)
        {
            pool.OnLeavingPool();
        }
        m_poolQueue.Clear();

    }
}
