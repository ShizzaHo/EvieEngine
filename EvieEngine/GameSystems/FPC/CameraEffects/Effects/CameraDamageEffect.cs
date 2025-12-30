using EvieEngine.FPC;
using UnityEngine;

namespace EvieEngine.CameraEffects
{
    public class CameraDamageEffect : CameraEffect
    {
        private float duration = 0.3f;
        private float elapsed = 0f;

        private Vector3 initialShakeOffset;
        private Vector3 initialShakeRotation;

        public CameraDamageEffect(float intensity = 1f)
        {
            // Генерируем случайные смещения один раз при создании
            initialShakeOffset = Random.insideUnitSphere * 0.2f * intensity;
            initialShakeRotation = Random.insideUnitSphere * 8f * intensity; // в градусах
        }

        public override (Vector3 posOffset, Vector3 rotOffset) Evaluate(float deltaTime)
        {
            elapsed += deltaTime;
            float t = elapsed / duration;

            if (t >= 1f)
            {
                IsFinished = true;
                return (Vector3.zero, Vector3.zero);
            }

            // Простое линейное затухание (можно заменить на t = 1 - (1 - t)^2 для плавного)
            float fade = 1f - t;

            Vector3 posOffset = initialShakeOffset * fade;
            Vector3 rotOffset = initialShakeRotation * fade;

            return (posOffset, rotOffset);
        }
    }
}