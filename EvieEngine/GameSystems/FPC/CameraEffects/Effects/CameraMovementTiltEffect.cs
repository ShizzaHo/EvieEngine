using EvieEngine.FPC;
using UnityEngine;

namespace EvieEngine.CameraEffects
{
    public class CameraMovementTiltEffect : CameraEffect
    {
        private Rigidbody rb;

        private float maxTiltAngle = 5f; // Максимальный угол наклона (в градусах)
        private float tiltSpeed = 10f; // Скорость сглаживания
        private float currentTilt = 0f; // Текущее значение наклона

        public override void Initialize()
        {
            rb = FPCCamera.Instance.gameObject.GetComponentInParent<Rigidbody>();
        }

        public override (Vector3 posOffset, Vector3 rotOffset) Evaluate(float deltaTime)
        {
            if (rb == null)
                return (Vector3.zero, Vector3.zero);

            // Скорость в локальных координатах камеры
            Vector3 localVel = FPCCamera.Instance.transform.InverseTransformDirection(rb.linearVelocity);

            // Целевой наклон в противоположную сторону движения по оси X
            float targetTilt = Mathf.Clamp(-localVel.x * maxTiltAngle, -maxTiltAngle, maxTiltAngle);

            // Плавное движение текущего наклона к целевому
            currentTilt = Mathf.MoveTowards(currentTilt, targetTilt, tiltSpeed * deltaTime);

            // Возвращаем только поворот по Z (крен)
            return (Vector3.zero, new Vector3(0f, 0f, currentTilt));
        }
    }
}