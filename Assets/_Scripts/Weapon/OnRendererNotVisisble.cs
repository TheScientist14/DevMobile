using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRendererNotVisisble : MonoBehaviour
{
    [SerializeField] private PoolableProjectile m_poolableProjectile;
    private void OnBecameInvisible()
    {
        m_poolableProjectile.Recycle();
    }
}
