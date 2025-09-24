using UnityEngine;


namespace EvieEngine.AIM
{
    public abstract class AimFeedback : MonoBehaviour
    {
        public abstract void OnFeedbackEnter();
        public abstract void OnFeedbackExit();
    }
}