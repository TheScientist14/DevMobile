using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto {
    public class AsteroidPool : PoolBase
    {
        public static AsteroidPool SharedInstance;

        [SerializeField] protected int m_numberOfAsteroidOnStart;

        private void Awake()
        {
            if (SharedInstance == null)
            {
                SharedInstance = this;
                // DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            PopulatePool();

            int asteroidsToActivate = Math.Min(m_numberOfAsteroidOnStart, _amountToPool);
            
            for (int i = 0; i < asteroidsToActivate; i++)
            {
                
            }
        }
    }
}

