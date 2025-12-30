using UnityEngine;

namespace EvieEngine.CameraEffects
{
    public class CameraJumpEffect : CameraEffect
    {
        private float duration = 0.6f; // длительность эффекта
        private float elapsed = 0f;

        private float amplitude = 0.05f; // амплитуда смещения камеры вниз/вверх
        private float rotationAmplitude = 4f; // амплитуда наклона камеры вперёд/назад

        public CameraJumpEffect(float strength = 1f)
        {
            amplitude *= strength;
            rotationAmplitude *= strength;
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

            // Кривая движения: sin, чтобы ушло вниз и чуть подпрыгнуло вверх
            float bounce = Mathf.Sin(t * Mathf.PI * 1.5f) * (1f - t);

            Vector3 posOffset = new Vector3(0f, -amplitude * bounce, 0f);
            Vector3 rotOffset = new Vector3(-rotationAmplitude * bounce, 0f, 0f);

            return (posOffset, rotOffset);
        }
    }
}