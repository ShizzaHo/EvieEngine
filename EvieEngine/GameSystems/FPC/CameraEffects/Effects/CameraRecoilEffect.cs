using UnityEngine;
using EvieEngine.FPC;

namespace EvieEngine.CameraEffects
{
    public class CameraRecoilEffect : CameraEffect
    {
        private Vector2 recoil;
        private Vector2 current;
        private float returnSpeed;

        public CameraRecoilEffect(Vector2 initialRecoil, float returnSpeed = 10f)
        {
            this.recoil = initialRecoil;
            this.returnSpeed = returnSpeed;
        }

        public override (Vector3 posOffset, Vector3 rotOffset) Evaluate(float deltaTime)
        {
            // Плавно возвращаемся к нулю
            current = Vector2.Lerp(current, Vector2.zero, deltaTime * returnSpeed);

            // Устанавливаем как вращение камеры (X — вверх/вниз, Y — влево/вправо)
            Vector3 rot = new Vector3(-current.x, current.y, 0f);

            return (Vector3.zero, rot);
        }

        public void AddRecoil(Vector2 extra)
        {
            recoil += extra;
            current += extra;
        }
    }
}