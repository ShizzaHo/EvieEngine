using System;
using EvieEngine.Utils;
using UnityEngine;

namespace EvieEngine.GUNSystem
{
    public abstract class Bullet : MonoBehaviour
    {
        protected Rigidbody rb;
        [SerializeField] protected float lifetime = 3f;
        [SerializeField] protected float speed = 3f;
        public float damage = 3f;
        protected bool isActive = false;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public virtual void Shoot(Vector3 position, Vector3 direction)
        {
            transform.position = position;
            transform.rotation = Quaternion.LookRotation(direction);

            gameObject.SetActive(true);
            isActive = true;
            rb.linearVelocity = direction * speed;

            CancelInvoke();
            Invoke(nameof(Deactivate), lifetime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!isActive) return;
            OnHit(collision);
            Deactivate();
        }

        protected abstract void OnHit(Collision collision);

        protected virtual void Deactivate()
        {
            isActive = false;
            rb.linearVelocity = Vector3.zero;
            ObjectPool.Instance.Despawn(gameObject);
        }
    }
}