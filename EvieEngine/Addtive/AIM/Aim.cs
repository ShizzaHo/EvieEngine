using TriInspector;
using UnityEngine;
using System.Collections.Generic;

namespace EvieEngine.AIM
{
    public class Aim : MonoBehaviour
    {
        [Title("Настройки")] 
        public float maxDistance = 1.5f;
        public KeyCode interactKey = KeyCode.E;

        [Title("Обратная связь")] 
        public bool enableFeedback = true;

        private Camera cam;
        private AimTrigger currentTarget;
        private List<AimFeedback> activeFeedbacks = new List<AimFeedback>();

        private List<AimFeedback> allFeedbacks = new List<AimFeedback>();

        private void Awake()
        {
            cam = GetComponent<Camera>();
            if (cam == null)
            {
                Debug.LogError("Aim должен быть прикреплен к объекту с камерой!");
                enabled = false;
            }
        }

        private void Start()
        {
            allFeedbacks.AddRange(FindObjectsOfType<AimFeedback>());
        }

        private void Update()
        {
            CheckAim();
            HandleInput();
        }

        private void CheckAim()
        {
            AimTrigger newTarget = null;
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                newTarget = hit.collider.GetComponent<AimTrigger>();
            }

            if (newTarget != currentTarget)
            {
                ExitCurrentTarget();
                currentTarget = newTarget;
                EnterNewTarget();
            }
        }

        private void EnterNewTarget()
        {
            if (currentTarget == null) return;

            activeFeedbacks.Clear();

            foreach (var feedback in allFeedbacks)
            {
                activeFeedbacks.Add(feedback);
            }

            foreach (var feedback in activeFeedbacks)
            {
                feedback.OnFeedbackEnter();
            }
        }

        private void ExitCurrentTarget()
        {
            if (activeFeedbacks.Count == 0) return;

            foreach (var feedback in activeFeedbacks)
            {
                feedback.OnFeedbackExit();
            }

            activeFeedbacks.Clear();
        }

        private void HandleInput()
        {
            if (currentTarget != null && Input.GetKeyDown(interactKey))
            {
                currentTarget.OnAimInteract();
            }
        }

        private void OnDestroy()
        {
            ExitCurrentTarget();
        }
    }
}