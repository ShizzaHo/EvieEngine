using UnityEngine;
using TriInspector;

namespace EvieEngine.Utils
{
    public class ObjectRoll : MonoBehaviour
    {
        [Title("Настройки роллинга")]
        [Tooltip("Максимальный угол крена (в градусах).")]
        public float maxRollAngle = 500f;

        [Title("Скорость регулировки броска")]
        public float rollSpeed = 10f;

        [Title("Настройки изменения положения")]
        [Tooltip("Величина колебания зависит от движения мыши.")]
        public float swayAmount = 0.02f;

        [Tooltip("Регулировка скорости раскачивания.")]
        public float swaySpeed = 5f;

        [Tooltip("Максимальное расстояние раскачивания.")]
        public float maxSwayDistance = 0.1f;

        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private float targetZRotation;
        private float currentZRotation;
        private Vector3 targetPosition;
        private Vector3 currentPosition;

        void Start()
        {
            initialPosition = transform.localPosition;
            initialRotation = transform.localRotation;
        }

        public void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime;

            // Calculate target rotation based on mouse movement
            targetZRotation = -mouseX * maxRollAngle;
            targetZRotation = Mathf.Clamp(targetZRotation, -maxRollAngle, maxRollAngle);

            // Smoothly interpolate to the target rotation
            currentZRotation = Mathf.Lerp(currentZRotation, targetZRotation, rollSpeed * Time.deltaTime);

            // Apply roll rotation
            transform.localRotation = initialRotation * Quaternion.Euler(0, 0, currentZRotation);

            // Calculate weapon sway based on mouse movement
            Vector3 swayOffset = new Vector3(-mouseX * swayAmount, -mouseY * swayAmount, 0);
            swayOffset = Vector3.ClampMagnitude(swayOffset, maxSwayDistance);

            // Smoothly interpolate to the target position
            targetPosition = initialPosition + swayOffset;
            currentPosition = Vector3.Lerp(currentPosition, targetPosition, swaySpeed * Time.deltaTime);

            // Apply sway position
            transform.localPosition = currentPosition;
        }
    }
}