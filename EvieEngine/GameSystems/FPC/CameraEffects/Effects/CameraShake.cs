using EvieEngine.CameraEffects;
using UnityEngine;

public class CameraShake : CameraEffect
{
    private float duration;
    private float magnitude;
    private float elapsed = 0f;

    public CameraShake(float duration, float magnitude)
    {
        this.duration = duration;
        this.magnitude = magnitude;
    }

    public override (Vector3 posOffset, Vector3 rotOffset) Evaluate(float deltaTime)
    {
        elapsed += deltaTime;
        if (elapsed >= duration)
        {
            IsFinished = true;
            return (Vector3.zero, Vector3.zero);
        }

        float intensity = (1f - elapsed / duration) * magnitude;
        Vector3 pos = Random.insideUnitSphere * intensity * 0.05f;
        Vector3 rot = new Vector3(
            Random.Range(-intensity, intensity),
            Random.Range(-intensity, intensity),
            Random.Range(-intensity, intensity)
        );

        return (pos, rot);
    }
}