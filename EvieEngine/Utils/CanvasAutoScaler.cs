using TriInspector;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class AutoCanvasScaler : MonoBehaviour
{
    [Title("Базовое разрешение (от которого считаем)")]
    public Vector2 referenceResolution = new Vector2(1920, 1080);

    [Title("Минимальный и максимальный Scale Factor")]
    public float minScale = 0.5f;
    public float maxScale = 2.0f;

    private CanvasScaler canvasScaler;

    void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
    }

    void Update()
    {
        float scaleX = Screen.width / referenceResolution.x;
        float scaleY = Screen.height / referenceResolution.y;

        float targetScale = Mathf.Min(scaleX, scaleY);

        targetScale = Mathf.Clamp(targetScale, minScale, maxScale);

        canvasScaler.scaleFactor = targetScale;
    }
}
