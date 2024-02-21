using UnityEngine;

public abstract class Poolable : MonoBehaviour
{
    public virtual void OnCreate()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnRecycle()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnLeavePool()
    {
        Destroy(gameObject);
    }
}
