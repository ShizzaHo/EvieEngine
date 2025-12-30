using EvieEngine.CameraEffects;
using EvieEngine.FPC;
using UnityEngine;

public class CameraTiltOnLook : CameraEffect
{
    private float currentTilt = 0f;      // Текущий угол наклона (roll)
    private float tiltVelocity = 0f;     // Текущая скорость изменения наклона
    private float maxTilt = 5f;           // Максимальный угол наклона в градусах
    private float tiltSensitivity = 2f;   // Чувствительность — насколько сильно влияет скорость поворота
    private float returnSpeed = 5f;       // Скорость возврата к 0

    private float lastYaw;                // Угол поворота камеры в предыдущем кадре

    public override void Initialize()
    {
        lastYaw = FPCCamera.Instance.transform.eulerAngles.y;
    }

    public override (Vector3 posOffset, Vector3 rotOffset) Evaluate(float deltaTime)
    {
        // Вычисляем изменение угла по оси Y (yaw)
        float currentYaw = FPCCamera.Instance.transform.eulerAngles.y;
        float yawDelta = Mathf.DeltaAngle(lastYaw, currentYaw) / deltaTime;
        lastYaw = currentYaw;

        // Рассчитываем желаемый наклон — противоположный направлению поворота
        float targetTilt = Mathf.Clamp(-yawDelta * tiltSensitivity, -maxTilt, maxTilt);

        // Плавно интерполируем текущий наклон к целевому
        currentTilt = Mathf.SmoothDamp(currentTilt, targetTilt, ref tiltVelocity, 1f / returnSpeed, Mathf.Infinity, deltaTime);

        // Возвращаем смещения: только наклон вокруг оси Z
        return (Vector3.zero, new Vector3(0f, 0f, currentTilt));
    }
}