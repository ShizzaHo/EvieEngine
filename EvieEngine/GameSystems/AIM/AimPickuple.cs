using UnityEngine;

namespace EvieEngine.AIM
{
    public class AimPickuple : MonoBehaviour
    {
        public float pickupRange = 5f;
        public float moveSpeed = 10f;
        public float holdDistance = 3f;

        private Camera cam;
        private Rigidbody heldObject;
        private Vector3 holdOffset;

        void Start()
        {
            cam = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0)) // Поднять объект
            {
                TryPickup();
            }

            if (Input.GetMouseButtonUp(0)) // Отпустить объект
            {
                Release();
            }

            if (heldObject)
            {
                MoveHeldObject();
            }
        }

        void TryPickup()
        {
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
            {
                // Проверяем тег объекта
                if (!hit.collider.CompareTag("pickuple")) return;

                Rigidbody rb = hit.collider.attachedRigidbody;
                if (rb != null && !rb.isKinematic)
                {
                    heldObject = rb;
                    heldObject.linearDamping = 10; // увеличиваем сопротивление, чтобы объект не ускакал
                    heldObject.angularDamping = 10;
                    holdOffset = hit.point - heldObject.position;
                }
            }
        }

        void Release()
        {
            if (heldObject)
            {
                heldObject.linearDamping = 0;
                heldObject.angularDamping = 0.05f;
                heldObject = null;
            }
        }

        void MoveHeldObject()
        {
            Vector3 targetPos = cam.transform.position + cam.transform.forward * holdDistance - holdOffset;
            Vector3 direction = targetPos - heldObject.position;
            heldObject.linearVelocity = direction * moveSpeed;
        }
    }
}