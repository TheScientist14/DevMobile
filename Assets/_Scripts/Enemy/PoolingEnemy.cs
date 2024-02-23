using UnityEngine;

public interface IRecyclable
{
    public void Recycle();
}

public class PoolingEnemy : Poolable, IRecyclable
{
    [SerializeField] private string m_EnemyName = "Enemy";
    public string EnemyName => m_EnemyName;

    private EnemiesPoolManager m_poolManager;

    private void Start()
    {
        m_poolManager = EnemiesPoolManager.SharedInstance;
    }

    public void Recycle()
    {
        m_poolManager.UnloadEnemy(this);
    }
}
