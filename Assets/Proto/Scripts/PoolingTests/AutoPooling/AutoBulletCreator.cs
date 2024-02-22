using UnityEngine;

namespace Proto
{
    public class AutoBulletCreator : TestBulletSpawnerBase
    {
        [SerializeField] private AutoBulletPoolingTest _bulletPrefab;

        private void Awake()
        {
            if (_bulletPrefab != null && AutoBulletPoolingTest.m_prefab is null) 
            {
                AutoBulletPoolingTest.m_prefab = _bulletPrefab;
            }
        }

        protected override void OnSingleSpawnCalled()
        {
            AutoBulletPoolingTest createdBullet = AutoBulletPoolingTest.Create();
            createdBullet.transform.SetParent(transform);
        }
    }
}

