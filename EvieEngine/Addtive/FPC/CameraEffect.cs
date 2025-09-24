using EvieEngine.FPC;
using UnityEngine;

namespace EvieEngine.CameraEffects
{
    public abstract class CameraEffect
    {
        public bool IsFinished { get; protected set; } = false;

        public virtual void Initialize()
        {
           
        }

        public abstract (Vector3 posOffset, Vector3 rotOffset) Evaluate(float deltaTime);
    }
}