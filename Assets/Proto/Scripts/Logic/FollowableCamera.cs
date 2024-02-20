using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Proto
{
    public class FollowableCamera : MonoBehaviour
    {
        [SerializeField] Transform m_CameraTarget;

        private void Update()
        {
            if (m_CameraTarget is null) return;
            if (m_CameraTarget.IsDestroyed()) return;
            Vector3 targetPos = m_CameraTarget.position;
            targetPos.z = transform.position.z;
            transform.position = targetPos;
        }
    }
}