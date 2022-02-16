using UnityEngine;

namespace Game.Scripts
{
    public class SurfaceMovement : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private Transform _rayCastPoint;
        [SerializeField] private float _rayDistance = 5f;
        private Vector3 _normal;
        private bool IsGround => _normal != Vector3.zero;

        public bool CheckGround()
        {
            SetNormal();
            return IsGround;
        }

        private void SetNormal()
        {
            _normal = Physics.Raycast(_rayCastPoint.position, Vector3.down, out var hit, _rayDistance, _groundLayer)
                ? hit.collider.transform.up
                : Vector3.zero;
        }

        public Vector3 Project(Vector3 forward) => forward - Vector3.Dot(forward, _normal) * _normal;
        // private void OnCollisionEnter(Collision other)
        // {
        //     if (other.gameObject.layer != _groundLayer) return;
        //     _normal = other.collider.transform.up;
        // }
        //
        // public Vector3 Project(Vector3 forward) => forward - Vector3.Dot(forward, _normal) * _normal;

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.white;
        //     Gizmos.DrawLine(transform.position, transform.position + _normal * 3);
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawLine(transform.position, transform.position + Project(transform.forward));
        // }
    }
}