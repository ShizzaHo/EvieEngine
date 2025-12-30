using UnityEngine;
using System.Collections.Generic;
using EvieEngine.FPC;

namespace EvieEngine.CameraEffects
{
    public class CameraEffectsManager : MonoBehaviour
    {
        private List<CameraEffect> effects = new List<CameraEffect>();
        private FPCCamera owner;

        public Vector3 PositionOffset { get; private set; }
        public Vector3 RotationOffset { get; private set; }

        private void Awake()
        {
            owner = GetComponent<FPCCamera>();
        }

        public void AddEffect(CameraEffect effect)
        {
            effect.Initialize();
            effects.Add(effect);
        }

        public void RemoveEffect(CameraEffect effect)
        {
            if (effects.Contains(effect))
            {
                effects.Remove(effect);
            }
        }

        public void RemoveEffect<T>() where T : CameraEffect
        {
            for (int i = effects.Count - 1; i >= 0; i--)
            {
                if (effects[i] is T)
                {
                    effects.RemoveAt(i);
                    break; // удаляем только первый найденный
                }
            }
        }

        private void LateUpdate()
        {
            PositionOffset = Vector3.zero;
            RotationOffset = Vector3.zero;

            for (int i = effects.Count - 1; i >= 0; i--)
            {
                var e = effects[i];
                var (pos, rot) = e.Evaluate(Time.deltaTime);

                PositionOffset += pos;
                RotationOffset += rot;

                if (e.IsFinished)
                    effects.RemoveAt(i);
            }
        }
    }
}