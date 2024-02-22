using UnityEngine;

public class ProjectilePool : PoolBase<PoolableProjectile>
{
    public static ProjectilePool SharedInstance;

    private void Awake()
    {
        if (SharedInstance == null)
        {
            SharedInstance = this;
        }
        else
        {
            Destroy(this);
        }
    }

}
