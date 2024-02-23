public class PoolableProjectile : Poolable, IRecyclable
{
    private ProjectilePool m_projectilePool;

    private void Start()
    {
        m_projectilePool = ProjectilePool.SharedInstance;
    }

    public void Recycle()
    {
        m_projectilePool.UnloadObject(this);
    }
}
