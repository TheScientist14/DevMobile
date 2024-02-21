using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable // <T>
{
    /// <summary>
    /// Called when the object gets pooled
    /// </summary>
    public void OnObjectPool();

    /// <summary>
    /// Called when the object gets back in the pool
    /// </summary>
    public void OnObjectUnpool();

    /// <summary>
    /// Called when the pool gets cleared/destroyed
    /// </summary>
    public void OnLeavingPool();
}
