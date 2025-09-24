using UnityEngine;

namespace EvieEngine.AI
{
    [System.Serializable]
    public class Blackboard
    {
        public Transform Target;
        public Vector3 LastKnownPosition;
        public bool TargetVisible;
    }
}