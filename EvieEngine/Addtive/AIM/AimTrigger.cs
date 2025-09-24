using UnityEngine;
using UnityEngine.Events;


namespace EvieEngine.AIM
{
    public class AimTrigger : MonoBehaviour
    {
        [Header("Событие при взаимодействии")] public UnityEvent onInteract;

        // Вызывается скриптом Aim при нажатии кнопки
        public void OnAimInteract()
        {
            if (onInteract != null)
                onInteract.Invoke();
        }
    }
}