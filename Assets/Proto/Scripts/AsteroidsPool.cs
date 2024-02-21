using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsPool : Poolable
{
    protected static PoolHandler m_asteroidPool = new PoolHandler();
    


    public override List<T> GetPooledObjects<T>(int NumberOfObjectsToPool)
    {
        throw new System.NotImplementedException();
    }

    public override T GetPoolObject<T>()
    {
        throw new System.NotImplementedException();
    }
}
