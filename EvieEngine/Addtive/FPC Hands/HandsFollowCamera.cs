using UnityEngine;

/// <summary>
/// Плавное следование объекта (например, рук) за камерой с настройками задержки/демпфинга.
/// Повесьте этот скрипт на GameObject рук (parent) и укажите камеру (Camera transform).
/// </summary>
public class HandsFollowCamera : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Трансформ камеры, за которой следят руки. Если пусто — попытается взять Camera.main.")]
    public Transform cameraTransform;

    [Header("Position Follow")]
    [Tooltip("Локальное смещение рук относительно камеры (в локальных координатах камеры).")]
    public Vector3 localPositionOffset = new Vector3(0.2f, -0.2f, 0.5f);
    [Tooltip("Скорость следования по позиции (чем выше — тем быстрее догоняет).")]
    public float positionSmoothTime = 0.08f;
    [Tooltip("Если расстояние больше этого — объект мгновенно телепортируется к камере (удобно при телепортации игрока).")]
    public float snapDistance = 2f;

    [Header("Rotation Follow")]
    [Tooltip("Эйлеры смещения вращения, добавляемые к камере (в градусах).")]
    public Vector3 rotationOffsetEuler = Vector3.zero;
    [Tooltip("Скорость плавного поворота (чем выше — тем быстрее).")]
    public float rotationLerpSpeed = 10f;
    [Tooltip("Если угол между текущим и целевым вращением больше — сделать мгновенный snap.")]
    public float snapAngle = 90f;

    [Header("Options")]
    [Tooltip("Если true — используем локальные смещения (offsets) в локальной системе камеры. Если false — в мировых координатах.")]
    public bool useLocalOffsets = true;

    // Внутренние переменные для SmoothDamp
    private Vector3 velocity = Vector3.zero;
    private Quaternion targetRotation;
    private Quaternion rotationOffsetQuat;

    void Reset()
    {
        cameraTransform = Camera.main ? Camera.main.transform : null;
    }

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main ? Camera.main.transform : null;

        rotationOffsetQuat = Quaternion.Euler(rotationOffsetEuler);

        // Инициализируем позицию/вращение сразу под камеру, чтобы не было резкого дерганья при старте.
        SnapToCamera();
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // --- Позиция ---
        Vector3 desiredWorldPos;
        if (useLocalOffsets)
        {
            // локальное смещение относительно камеры: камера.TransformPoint(offset)
            desiredWorldPos = cameraTransform.TransformPoint(localPositionOffset);
        }
        else
        {
            desiredWorldPos = localPositionOffset; // здесь это понимается как мировая цель
        }

        // Snap при большой дистанции (например, телепортация игрока)
        if (Vector3.Distance(transform.position, desiredWorldPos) > snapDistance)
        {
            transform.position = desiredWorldPos;
            velocity = Vector3.zero;
        }
        else
        {
            // плавное следование
            transform.position = Vector3.SmoothDamp(transform.position, desiredWorldPos, ref velocity, positionSmoothTime);
        }

        // --- Вращение ---
        // Целевое вращение: камера.rotation * offset
        Quaternion desiredRotation = cameraTransform.rotation * rotationOffsetQuat;

        // Snap по углу если нужно
        float angle = Quaternion.Angle(transform.rotation, desiredRotation);
        if (angle > snapAngle)
        {
            transform.rotation = desiredRotation;
        }
        else
        {
            // плавный slerp
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 1f - Mathf.Exp(-rotationLerpSpeed * Time.deltaTime));
        }
    }

    /// <summary>
    /// Мгновенно поставить объект в позицию/поворот камеры (с учетом offset).
    /// </summary>
    public void SnapToCamera()
    {
        if (cameraTransform == null) return;

        rotationOffsetQuat = Quaternion.Euler(rotationOffsetEuler);

        if (useLocalOffsets)
            transform.position = cameraTransform.TransformPoint(localPositionOffset);
        else
            transform.position = localPositionOffset;

        transform.rotation = cameraTransform.rotation * rotationOffsetQuat;
        velocity = Vector3.zero;
    }
}
