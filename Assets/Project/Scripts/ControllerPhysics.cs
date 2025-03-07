using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.Scripts
{
    public class ControllerPhysics : MonoBehaviour
    {
        [SerializeField]
        private Transform target;
        private Rigidbody _rb;

        private Quaternion _rotDiff;

        public bool rotationLocked { get; set; }

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rotDiff = transform.rotation *
                       Quaternion.Inverse(target.rotation);
        }

        public void ResetPos()
        {
            transform.rotation = _rotDiff * target.rotation;
        }

        void FixedUpdate()
        {
            _rb.velocity = (target.position - transform.position) /
                           Time.fixedDeltaTime;

            if (rotationLocked)
            {
                _rb.freezeRotation = true;
                return;
            }

            _rb.freezeRotation = false;

            Quaternion rotationDifference = target.rotation *
                                            Quaternion.Inverse(
                                                transform.rotation);
            rotationDifference.ToAngleAxis(out float angleDifference,
                out Vector3 rotationAxis);

            Vector3 rotationDifferenceInDegrees =
                angleDifference * rotationAxis;

            _rb.angularVelocity = (rotationDifferenceInDegrees *
                Mathf.Deg2Rad / Time.fixedDeltaTime);
        }
    }
}