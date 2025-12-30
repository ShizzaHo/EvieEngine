using EvieEngine.FPC;
using UnityEngine;

namespace EvieEngine.CameraEffects
{
    public class CameraMovementShakeEffect : CameraEffect
    {
        private Rigidbody rb;
        private FPCController fpc;

        private float bobSpeed = 4f; // Частота покачивания
        private float bobAmount = 0.05f; // Амплитуда по вертикали
        private float swayAmount = 0.05f; // Амплитуда по горизонтали
        private float fadeSpeed = 5f; // Скорость появления/затухания эффекта

        private float bobTimer = 0f;
        private float currentAmount = 0f; // Текущая сила эффекта (0..1)

        public override void Initialize()
        {
            rb = FPCCamera.Instance.gameObject.GetComponentInParent<Rigidbody>();
            fpc = FPCCamera.Instance.gameObject.GetComponentInParent<FPCController>();
        }

        public override (Vector3 posOffset, Vector3 rotOffset) Evaluate(float deltaTime)
        {
            if (fpc.isRunning == true)
                return (Vector3.zero, Vector3.zero);

            if (rb == null)
                return (Vector3.zero, Vector3.zero);

            Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            float speed = horizontalVel.magnitude;

            // Плавное изменение силы эффекта
            float targetAmount = speed > 0.1f ? 1f : 0f;
            currentAmount = Mathf.MoveTowards(currentAmount, targetAmount, fadeSpeed * deltaTime);

            // Продолжаем движение таймера только при движении
            if (speed > 0.1f)
                bobTimer += deltaTime * bobSpeed * speed;

            // Считаем смещения
            float verticalOffset = Mathf.Sin(bobTimer) * bobAmount * currentAmount;
            float horizontalOffset = Mathf.Cos(bobTimer * 0.5f) * swayAmount * currentAmount;

            return (new Vector3(horizontalOffset, verticalOffset, 0f), Vector3.zero);
        }
    }
}