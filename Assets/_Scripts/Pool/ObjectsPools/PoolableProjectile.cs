using UnityEngine;

public class PoolableProjectile : Poolable, IRecyclable
{
    private ProjectilePool m_projectilePool;
    private Camera m_camera;

    private void Start()
    {
        m_projectilePool = ProjectilePool.SharedInstance;
        m_camera = Camera.main;
    }

    public void Recycle()
    {
        if (m_projectilePool != null)
            m_projectilePool.UnloadObject(this);
    }

    public void RecycleIfNotOnScreen()
    {
        if (m_camera == null)
            m_camera = Camera.main;
        if (m_camera == null) return;
        Vector2 ScreenPos = m_camera.WorldToScreenPoint(transform.position);
        bool isNotVisible = ScreenPos.x <= 0 || ScreenPos.x >= m_camera.pixelWidth || ScreenPos.y <= 0 || ScreenPos.y >= m_camera.pixelHeight;
        if (isNotVisible)
            Recycle();
        else
            gameObject.SetActive(true);
    }

    public override void OnCreate()
    {
    }
    

}
