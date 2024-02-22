using UnityEngine;

public abstract class Poolable : MonoBehaviour
{
    /// <summary>
    /// Called when object is being pooled. Default implementation is SetActive(False).
    /// </summary>
    public virtual void OnCreate()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Called when object is going back to the pool. Default implementation is SetActive(false).
    /// </summary>
    public virtual void OnRecycle()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Called when the object is being released from pool because of a clear. Default implementation is Destroying the GO.
    /// </summary>
    public virtual void OnLeavePool()
    {
        Destroy(gameObject);
    }

    // public abstract void Recycle();
}
