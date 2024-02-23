using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRendererNotVisisble : MonoBehaviour
{
    [SerializeField] private Transform m_GameObjectToRecycle;
    private void OnBecameInvisible()
    {
        IRecyclable Recycled = null;
        if (m_GameObjectToRecycle.gameObject.TryGetComponent(out Recycled)) {
            Recycled.Recycle();
        }
    }
}
