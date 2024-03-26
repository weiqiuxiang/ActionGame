using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Project.ActionGame
{
    /// <summary>
    /// カメラに関連する処理を管理
    /// </summary>
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private CinemachineFreeLook freeLock;
        [SerializeField] private CinemachineVirtualCamera lockOnLook;
        [SerializeField] private ColliderTriggerObjectContainer lockOnTargetContainer;
        [SerializeField] private Transform playerTransform;

        private GameObject lockOnTarget = null;
        public bool IsLockOn => lockOnTarget != null;

        public void LockOn()
        {
            if (!IsLockOn)
            {
                if (lockOnTargetContainer.List.Count == 0) return;
                lockOnTarget = lockOnTargetContainer.List.OrderBy(element => Vector3.SqrMagnitude(element.transform.position - playerTransform.position)).First();
                lockOnLook.LookAt = lockOnTarget.transform;
            }
            else
            {
                LockOnRelease();
            }

            SetCameraLook();
        }

        public Vector3 VectorToTarget(bool isYZero = false)
        {
            if (!IsLockOn)
            {
                Debug.LogWarning("ロックオンする相手はない。このメソッドを呼び出すべきではない");
                return Vector3.zero;
            }
            else
            {
                Vector3 vector = lockOnTarget.transform.position - playerTransform.position;
                if (isYZero)
                {
                    vector.y = 0;
                }
                return vector;
            }
        }

        public void CheckLockOnRelease()
        {
            if (!IsLockOn) return;
            if (!lockOnTargetContainer.List.Contains(lockOnTarget))
            {
                LockOnRelease();
            }

            SetCameraLook();
        }

        private void LockOnRelease()
        {
            lockOnTarget = null;
            lockOnLook.LookAt = null;
            freeLock.m_XAxis.Value = cameraTransform.transform.localEulerAngles.y;
        }

        private void SetCameraLook()
        {
            freeLock.gameObject.SetActive(!IsLockOn);
            lockOnLook.gameObject.SetActive(IsLockOn);
        }
    }
}
