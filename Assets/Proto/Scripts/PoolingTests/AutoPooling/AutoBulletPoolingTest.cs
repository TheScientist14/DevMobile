using System.Collections.Generic;
using UnityEngine;

namespace Proto
{
    public class AutoBulletPoolingTest : BulletPoolingTest
    {
        private static Queue<AutoBulletPoolingTest> _pool = new Queue<AutoBulletPoolingTest>();
        
        public static AutoBulletPoolingTest m_prefab;
        public static AutoBulletPoolingTest Create()
        {
            if (_pool.Count == 0)
            {
                AutoBulletPoolingTest createdBullet = Instantiate(m_prefab);
                createdBullet.OnCreate();
                return createdBullet;
            }
            AutoBulletPoolingTest pooledBullet = _pool.Dequeue();
            pooledBullet.OnCreate();
            return pooledBullet;
        }

        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();
            _pool.Enqueue(this);
        }
    }
}

