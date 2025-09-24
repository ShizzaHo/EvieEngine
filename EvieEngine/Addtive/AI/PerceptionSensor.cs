using UnityEngine;

namespace EvieEngine.AI
{
    [RequireComponent(typeof(Collider))]
    public class PerceptionSensor : MonoBehaviour
    {
        public EnemyConfig config;
        public Transform eyePoint;
        public LayerMask targetMask;
        public LayerMask obstacleMask;
        public Blackboard blackboard;

        private void Reset()
        {
            // ensure collider set to trigger in editor or via code if needed
        }

        public bool CanSee(Transform target)
        {
            if (target == null) return false;
            Vector3 dir = (target.position - eyePoint.position);
            float dist = dir.magnitude;
            if (dist > config.visionRadius) return false;

            float angle = Vector3.Angle(eyePoint.forward, dir);
            if (angle > config.visionAngle * 0.5f) return false;

            if (Physics.Raycast(eyePoint.position, dir.normalized, out RaycastHit hit, config.visionRadius, ~0))
            {
                if (((1 << hit.collider.gameObject.layer) & obstacleMask) != 0)
                {
                    // hit obstacle
                    return false;
                }
            }

            return true;
        }

        public bool IsInHearingRange(Vector3 sourcePosition)
        {
            return Vector3.Distance(transform.position, sourcePosition) <= config.hearingRadius;
        }

        // Optionally, you can implement async/per-frame checks or caching to optimize.
    }
}