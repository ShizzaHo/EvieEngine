using EvieEngine.FPC;
using UnityEngine;

namespace EvieEngine.CameraEffects
{
    public class CameraRunningShakeEffect : CameraEffect
    {
        private Rigidbody rb;
        private FPCController fpc;

        private float bobTimer = 0f;
        private float effectWeight = 0f; // Вес эффекта (0 = нет, 1 = полный)

        private float bobSpeed = 20f; // Частота покачивания
        private float bobAmount = 0.1f; // Вертикальная амплитуда
        private float swayAmount = 0.5f; // Горизонтальная амплитуда
        private float fadeSpeed = 5f; // Скорость затухания

        public override void Initialize()
        {
            rb = FPCCamera.Instance.gameObject.GetComponentInParent<Rigidbody>();
            fpc = FPCCamera.Instance.gameObject.GetComponentInParent<FPCController>();
        }

        public override (Vector3 posOffset, Vector3 rotOffset) Evaluate(float deltaTime)
        {
            if (rb == null || fpc == null)
                return (Vector3.zero, Vector3.zero);

            // Плавно наращиваем или затухаем эффект
            float targetWeight = fpc.isRunning ? 1f : 0f;
            effectWeight = Mathf.Lerp(effectWeight, targetWeight, deltaTime * fadeSpeed);

            if (effectWeight < 0.01f) // Слишком маленький эффект — отключаем
                return (Vector3.zero, Vector3.zero);

            bobTimer += deltaTime * bobSpeed;

            float verticalOffset = Mathf.Sin(bobTimer) * bobAmount * effectWeight;
            float horizontalSway = Mathf.Sin(bobTimer * 0.5f) * swayAmount * effectWeight;

            Vector3 posOffset = new Vector3(0f, verticalOffset, 0f);
            Vector3 rotOffset = new Vector3(0f, 0f, horizontalSway * 10f);

            return (posOffset, rotOffset);
        }
    }
}